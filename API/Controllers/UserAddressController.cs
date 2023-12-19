using API.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace API.Controllers
{
    [Route("api/User/Address")]
    [ApiController]
    public class UserAddressController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public UserAddressController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("{userID}")]
        public async Task<IActionResult> GetUserAddressByUserID(string userID)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("SQLServer-Connection")))
                {
                    await connection.OpenAsync();
                    var command = new SqlCommand("GetAddressByUserID", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserId", userID);

                    var reader = await command.ExecuteReaderAsync();

                    var results = new List<UserAddress>();

                    while (await reader.ReadAsync())
                    {
                        var userAddress = new UserAddress
                        {
                            ProvinceID = reader["ProvinceID"].ToString(),
                            ProvinceName = reader["ProvinceName"].ToString(),
                            DistrictID = reader["DistrictID"].ToString(),
                            DistrictName = reader["DistrictName"].ToString(),
                            CommuneID = reader["CommuneID"].ToString(),
                            CommuneName = reader["CommuneName"].ToString(),
                            DetailAddress = reader["detail_address"].ToString(),
                            Status = reader["status"] == DBNull.Value ? 0 : (int)reader["status"]
                        };

                        results.Add(userAddress);
                    }

                    if (results.Count == 0)
                    {
                        return NotFound();
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
