using API.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace API.Controllers
{
    [Route("api/Voucher")]
    [ApiController]
    public class VoucherController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public VoucherController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("getVoucherActivity/{condition}")]
        public async Task<IActionResult> GetVoucherActionResultAsync(int condition)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("SQLServer-Connection")))
                {
                    await connection.OpenAsync();
                    var command = new SqlCommand("GetVoucherActivity", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@NewCondition", condition);

                    var reader = await command.ExecuteReaderAsync();

                    var results = new List<Voucher>();

                    while (await reader.ReadAsync())
                    {
                        var voucher = new Voucher
                        {
                            Id = (int)reader["id"],
                            Code = reader["code"].ToString(),
                            Name = reader["name"].ToString(),
                            Type = (int)reader["type"],
                            Value = (double)reader["value"],
                            //MaximumValue = (int)reader["maximum_value"],
                            ConditionValue = (int)reader["condition_value"],
                            Quantity = (int)reader["quantity"],
                            StartDate = (DateTime)reader["start_date"],
                            EndDate = (DateTime)reader["end_date"]
                        };

                        if (reader["maximum_value"] != DBNull.Value)
                        {
                            voucher.MaximumValue = (int)reader["maximum_value"];
                        }
                        else
                        {
                            voucher.MaximumValue = null;
                        }
                        results.Add(voucher);
                    }
                    return Ok(results);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
