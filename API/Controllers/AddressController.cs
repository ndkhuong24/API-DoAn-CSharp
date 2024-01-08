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
                            DetailAddress = reader["detail_address"].ToString(),
                            ProvinceID = reader["province_id"].ToString(),
                            DistrictID = reader["district_id"].ToString(),
                            CommuneID = reader["commune_id"].ToString(),
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

        [HttpPut]
        public async Task<IActionResult> UpdateAddress([FromBody] UserAddress userAddress)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("SQLServer-Connection")))
                {
                    await connection.OpenAsync();
                    var command = new SqlCommand("UpdateDetailAddress", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@AddressID", userAddress.AddressID);
                    command.Parameters.AddWithValue("@NewProvinceID", userAddress.ProvinceID);
                    command.Parameters.AddWithValue("@NewProvinceName", userAddress.ProvinceName);
                    command.Parameters.AddWithValue("@NewDistrictID", userAddress.DistrictID);
                    command.Parameters.AddWithValue("@NewDistrictName", userAddress.DistrictName);
                    command.Parameters.AddWithValue("@NewCommuneID", userAddress.CommuneID);
                    command.Parameters.AddWithValue("@NewCommuneName", userAddress.CommuneName);
                    command.Parameters.AddWithValue("@NewDetailAddress", userAddress.DetailAddress);

                    await command.ExecuteNonQueryAsync();

                    return Ok("Đã cập nhật địa chỉ thành công.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Lỗi: " + ex.Message);
            }
        }

    }
}
