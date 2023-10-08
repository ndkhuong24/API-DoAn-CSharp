using API.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace API.Controllers
{
    [Route("api/Orders")]
    [ApiController]
    public class OrderAndOrderDetailController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public OrderAndOrderDetailController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> InsertOrderAndOrderDetails(OrderDTO orderDTO)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("SQLServer-Connection")))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("InsertOrderAndOrderDetail", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add parameters
                        command.Parameters.AddWithValue("@CustomerID", orderDTO.CustomerID);
                        command.Parameters.AddWithValue("@UserID", orderDTO.UserID);
                        command.Parameters.AddWithValue("@TotalPrice", orderDTO.TotalPrice);
                        command.Parameters.AddWithValue("@TranSportFee", orderDTO.TranSportFee);
                        command.Parameters.AddWithValue("@Description", orderDTO.Description);
                        command.Parameters.AddWithValue("@VoucherID", orderDTO.VoucherID);
                        command.Parameters.AddWithValue("@DiscountPrice", orderDTO.DiscountPrice);
                        command.Parameters.AddWithValue("@FinalPrice", orderDTO.FinalPrice);
                        command.Parameters.AddWithValue("@Status", orderDTO.Status);


                        // Create a DataTable for Images
                        DataTable orderDetailTable = new DataTable();
                        orderDetailTable.Columns.Add("product_detail_id", typeof(int));
                        orderDetailTable.Columns.Add("quantity", typeof(int));
                        orderDetailTable.Columns.Add("price", typeof(int));

                        foreach (var image in orderDTO.orderDetail)
                        {
                            orderDetailTable.Rows.Add(image.ProductDetailId, image.Quantity, image.Price);
                        }

                        command.Parameters.AddWithValue("@OrderDetail", orderDetailTable);
                        command.Parameters["@OrderDetail"].SqlDbType = SqlDbType.Structured;

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
