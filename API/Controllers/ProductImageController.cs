using API.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace API.Controllers
{
    [Route("api/ProductDetail")]
    [ApiController]
    public class ProductImageController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ProductImageController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpPost("insert")]
        public async Task<IActionResult> InsertProductAndImages(ProductAndImagesDto productAndImages)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("SQLServer-Connection")))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("InsertProductAndImages", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add parameters
                        command.Parameters.AddWithValue("@CategoryID", productAndImages.CategoryID);
                        command.Parameters.AddWithValue("@BrandID", productAndImages.BrandID);
                        command.Parameters.AddWithValue("@ProductID", productAndImages.ProductID);
                        command.Parameters.AddWithValue("@SizeID", productAndImages.SizeID);
                        command.Parameters.AddWithValue("@ColorID", productAndImages.ColorID);
                        command.Parameters.AddWithValue("@SoleID", productAndImages.SoleID);
                        command.Parameters.AddWithValue("@MaterialID", productAndImages.MaterialID);
                        command.Parameters.AddWithValue("@Quantity", productAndImages.Quantity);
                        command.Parameters.AddWithValue("@Price", productAndImages.Price);


                        // Create a DataTable for Images
                        DataTable imagesTable = new DataTable();
                        imagesTable.Columns.Add("name", typeof(string));
                        imagesTable.Columns.Add("url", typeof(string));
                        imagesTable.Columns.Add("status", typeof(int));

                        foreach (var image in productAndImages.Images)
                        {
                            imagesTable.Rows.Add(image.Name, image.Url, image.Status);
                        }

                        command.Parameters.AddWithValue("@Images", imagesTable);
                        command.Parameters["@Images"].SqlDbType = SqlDbType.Structured;

                        await command.ExecuteNonQueryAsync();

                        return Ok("Data inserted successfully");
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}
