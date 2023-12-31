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

        [HttpGet("getById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("SQLServer-Connection")))
                {
                    await connection.OpenAsync();
                    var command = new SqlCommand("SearchProductDetailById", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", id);

                    var reader = await command.ExecuteReaderAsync();

                    if (await reader.ReadAsync())
                    {
                        var productDetail = new ProductCart
                        {
                            Id = (int)reader["Id"],
                            Name = reader["Name"].ToString(),
                            Path = reader["Path"].ToString(),
                            Price = (int)reader["Price"],
                            Quantity = (int)reader["Quantity"],
                            Status = (int)reader["Status"]
                        };

                        return Ok(productDetail);
                    }
                    else
                    {
                        return NotFound("Không tìm thấy sản phẩm với ID " + id);
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Lỗi: " + ex.Message);
            }
        }
        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllProductDetail()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("SQLServer-Connection")))
                {
                    await connection.OpenAsync();
                    var command = new SqlCommand("GetProductDetail", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    var reader = await command.ExecuteReaderAsync();

                    var results = new List<ProductDetailTable>();

                    while (await reader.ReadAsync())
                    {
                        var productDetail = new ProductDetailTable
                        {
                            Id = reader.GetInt32("ProductID"),
                            Path = reader.GetString("Path"),
                            ProductName = reader.GetString("ProductName"),
                            StyleName = reader.GetString("StyleName"),
                            Quantity = reader.GetInt32("Quantity"),
                            Price = reader.GetInt32("Price"),
                            Status = reader.GetInt32("Status")
                        };

                        results.Add(productDetail);
                    }

                    return Ok(results);
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Error: " + ex.Message);
            }
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
        [HttpGet("getPDByID/{id}")]
        public async Task<IActionResult> GetPDById(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("SQLServer-Connection")))
                {
                    await connection.OpenAsync();
                    var command = new SqlCommand("GetProductDetailById", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", id);

                    var reader = await command.ExecuteReaderAsync();

                    if (await reader.ReadAsync())
                    {
                        var getPDById = new GetPDById
                        {
                            Id = (int)reader["Id"],
                            CategoryName = reader["CategoryName"].ToString(),
                            BrandName = reader["BrandName"].ToString(),
                            ProductName = reader["ProductName"].ToString(),
                            SizeName = reader["SizeName"].ToString(),
                            ColorName = reader["ColorName"].ToString(),
                            SoleName = reader["SoleName"].ToString(),
                        };

                        return Ok(getPDById);
                    }
                    else
                    {
                        return NotFound("Không tìm thấy sản phẩm với ID " + id);
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Lỗi: " + ex.Message);
            }
        }

        [HttpGet("getImageChinhById/{id}")]
        public async Task<IActionResult> GetImageChinhById(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("SQLServer-Connection")))
                {
                    await connection.OpenAsync();
                    var command = new SqlCommand("GetImageChinhProductDetail", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", id);

                    var reader = await command.ExecuteReaderAsync();

                    if (await reader.ReadAsync())
                    {
                        var imageChinh = new ImageChinh
                        {
                            Id = (int)reader["Id"],
                            Path = reader["Path"].ToString()
                        };

                        return Ok(imageChinh);
                    }
                    else
                    {
                        return NotFound("Không tìm thấy sản phẩm với ID " + id);
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Lỗi: " + ex.Message);
            }
        }

        [HttpGet("getImagePhuById/{id}")]
        public async Task<IActionResult> GetImagePhuProductDetail(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("SQLServer-Connection")))
                {
                    await connection.OpenAsync();
                    var command = new SqlCommand("GetImagePhuProductDetail", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", id);

                    var reader = await command.ExecuteReaderAsync();

                    var results = new List<ImagePhu>();

                    while (await reader.ReadAsync())
                    {
                        var imagePhu = new ImagePhu
                        {
                            Id = reader.GetInt32("Id"),
                            Path = reader.GetString("Path")
                        };

                        results.Add(imagePhu);
                    }
                    return Ok(results);
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Error: " + ex.Message);
            }
        }

        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> UpdateProductDetail(int id, int quantity, int price, int status)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("SQLServer-Connection")))
                {
                    connection.Open();
                    var command = new SqlCommand("UpdateProductDetail", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@NewId", id);
                    command.Parameters.AddWithValue("@NewQuantity", quantity);
                    command.Parameters.AddWithValue("@NewPrice", price);
                    command.Parameters.AddWithValue("@NewStatus", status);

                    await command.ExecuteNonQueryAsync();

                }

                return Ok("Success");
            }
            catch (Exception ex)
            {
                return BadRequest("Error: " + ex.Message);
            }
        }

        [HttpGet("GetProductDetailAndCart/{id}")]
        public async Task<IActionResult> GetProductDetailAndCart(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("SQLServer-Connection")))
                {
                    await connection.OpenAsync();
                    var command = new SqlCommand("GetProductDetailAndCart", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", id);

                    var reader = await command.ExecuteReaderAsync();

                    if (await reader.ReadAsync())
                    {
                        var getProductDetailAndCart = new GetProductDetailAndCart
                        {
                            ProductDetailID = (int)reader["ProductDetailID"],
                            ProductCode = reader["ProductCode"].ToString(),
                            ProductName = reader["ProductName"].ToString(),
                            ProductDescription = reader["ProductDescription"].ToString(),
                            StyleName = reader["StyleName"].ToString(),
                            CategoryName = reader["CategoryName"].ToString(),
                            BrandName = reader["BrandName"].ToString(),
                            SizeName = reader["SizeName"].ToString(),
                            ColorName = reader["ColorName"].ToString(),
                            SoleName = reader["SoleName"].ToString(),
                            MaterialName = reader["MaterialName"].ToString(),
                            Quantity = (int)reader["Quantity"],
                            Price = (int)reader["Price"],
                            Status = (int)reader["Status"]
                        };

                        return Ok(getProductDetailAndCart);
                    }
                    else
                    {
                        return NotFound("Không tìm thấy sản phẩm với ID " + id);
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Lỗi: " + ex.Message);
            }
        }

    }
}
