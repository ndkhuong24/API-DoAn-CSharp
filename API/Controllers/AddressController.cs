using API.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace API.Controllers
{
    [Route("api/Address")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AddressController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("{addressID}")]
        public async Task<IActionResult> GetAddressbyId(int addressID)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("SQLServer-Connection")))
                {
                    await connection.OpenAsync();
                    var command = new SqlCommand("GetAddressById", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@AddressID", addressID);

                    var reader = await command.ExecuteReaderAsync();

                    if (await reader.ReadAsync())
                    {
                        var address = new Address
                        {
                            Id = (int)reader["id"],
                            UserID = (int)reader["user_id"],
                            //UserID = reader["user_id"].ToString(),
                            DetailAddress = reader["detail_address"].ToString(),
                            ProvinceID = reader["province_id"].ToString(),
                            DistrictID = reader["district_id"].ToString(),
                            CommuneID = reader["commune_id"].ToString(),
                            //ProvinceID = (int)reader["province_id"],
                            //DistrictID = int.Parse(reader["district_id"].ToString()),
                            //CommuneID = (int)reader["commune_id"],
                            Status = (int)reader["status"],
                        };

                        return Ok(address);
                    }
                    else
                    {
                        return NotFound("Không tìm thấy địa chỉ với ID " + addressID);
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
