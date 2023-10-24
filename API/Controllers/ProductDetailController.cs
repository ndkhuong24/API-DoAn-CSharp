﻿using API.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace API.Controllers
{
    [Route("api/ProductDetail")]
    [ApiController]
    public class ProductDetailController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ProductDetailController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        [Route("upload-anh-chinh")]
        public async Task<IActionResult> UploadAnhChinh(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest("No file uploaded.");
                }

                var imageNameAnhChinh = Path.GetFileName(file.FileName);
                var imagePathAnhChinh = Path.Combine("wwwroot/images", imageNameAnhChinh).Replace("\\", "/");
                // Lưu nội dung của tệp vào thư mục
                using (var stream = new FileStream(imagePathAnhChinh, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                // Bỏ phần "wwwroot" khỏi đường dẫn
                var imagePathAnhChinhWithoutWwwroot = imagePathAnhChinh.Replace("wwwroot", "").Replace("\\", "/");

                // Trả về đường dẫn của ảnh
                string relativeImagePath = imagePathAnhChinhWithoutWwwroot;
                return Ok(new { path = relativeImagePath });
            }
            catch (Exception ex)
            {
                return BadRequest("Error: " + ex.Message);
            }
        }

        [HttpPost]
        [Route("upload-anh-phu")]
        public async Task<IActionResult> UploadAnhPhu([FromForm] List<IFormFile> files)
        {
            try
            {
                if (files == null || files.Count == 0)
                {
                    return BadRequest("No files uploaded.");
                }

                List<AnhPhuViewModel> imageList = new List<AnhPhuViewModel>();
                //var imageList = new List<AnhPhuViewModel>();

                foreach (var file in files)
                {
                    var imageName = Path.GetFileName(file.FileName);
                    var imagePath = Path.Combine("wwwroot/images", imageName).Replace("\\", "/");
                    var imagePathWithoutWwwroot = imagePath.Replace("wwwroot", "");

                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    // Tạo đối tượng ImageModel và thêm vào danh sách
                    var imageModel = new AnhPhuViewModel
                    {
                        Path = imagePathWithoutWwwroot,
                    };
                    imageList.Add(imageModel);
                }
                return Ok(imageList);
            }
            catch (Exception ex)
            {
                return BadRequest("Error: " + ex.Message);
            }
        }

        [HttpPost]
        [Route("insert")]
        public async Task<IActionResult> InsertProductDetail(ProductDetailViewModel productDetailViewModel)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("SQLServer-Connection")))
                {
                    connection.Open();
                    var command = new SqlCommand("InsertProductDetail", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@CategoryID", productDetailViewModel.CategoryID);
                    command.Parameters.AddWithValue("@BrandID", productDetailViewModel.BrandID);
                    command.Parameters.AddWithValue("@ProductID", productDetailViewModel.ProductID);
                    command.Parameters.AddWithValue("@SizeID", productDetailViewModel.SizeID);
                    command.Parameters.AddWithValue("@ColorID", productDetailViewModel.ColorID);
                    command.Parameters.AddWithValue("@SoleID", productDetailViewModel.SoleID);
                    command.Parameters.AddWithValue("@MaterialID", productDetailViewModel.MaterialID);
                    command.Parameters.AddWithValue("@Quantity", productDetailViewModel.Quantity);
                    command.Parameters.AddWithValue("@Price", productDetailViewModel.Price);


                    var dataTableAnhChinh = new DataTable();
                    dataTableAnhChinh.Columns.Add("Path");
                    dataTableAnhChinh.Rows.Add(productDetailViewModel.AnhChinh.Path);
                    command.Parameters.AddWithValue("@ImageAnhChinh", dataTableAnhChinh);

                    var dataTableAnhPhu = new DataTable();
                    dataTableAnhPhu.Columns.Add("Path");
                    foreach (var image in productDetailViewModel.AnhPhu)
                    {
                        dataTableAnhPhu.Rows.Add(image.Path);
                    }
                    command.Parameters.AddWithValue("@ImageAnhPhu", dataTableAnhPhu);
                    command.Parameters["@ImageAnhChinh"].SqlDbType = SqlDbType.Structured;
                    command.Parameters["@ImageAnhPhu"].SqlDbType = SqlDbType.Structured;

                    await command.ExecuteNonQueryAsync();

                }

                return Ok("Success");
            }
            catch (Exception ex)
            {
                return BadRequest("Error: " + ex.Message);
            }
        }
    }
}
