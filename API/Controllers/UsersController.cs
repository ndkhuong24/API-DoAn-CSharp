using API.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace API.Controllers
{
    [Route("api/Users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public UsersController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("getById/{id}")]
        public async Task<IActionResult> GetUsersById(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("SQLServer-Connection")))
                {
                    await connection.OpenAsync();
                    var command = new SqlCommand("GetCustomerById", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", id);

                    var reader = await command.ExecuteReaderAsync();

                    if (await reader.ReadAsync())
                    {
                        var users = new Users
                        {
                            Id = (int)reader["Id"],
                            FullName = reader["fullname"].ToString(),
                            Email = reader["email"].ToString(),
                            Gender = (int)reader["gender"],
                            PhoneNumber = reader["phone_number"].ToString()
                        };

                        return Ok(users);
                    }
                    else
                    {
                        return NotFound($"User with ID {id} not found");
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"SQL Exception: {sqlEx.Message}");
                return BadRequest("Failed to retrieve user by ID. Please try again.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return BadRequest("Failed to retrieve user by ID. Please try again.");
            }
        }
    }
}


//using API.ViewModel;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Data.SqlClient;
//using System.Data;

//namespace API.Controllers
//{
//    [Route("api/Users")]
//    [ApiController]
//    public class UsersController : ControllerBase
//    {
//        private readonly IConfiguration _configuration;

//        public UsersController(IConfiguration configuration)
//        {
//            _configuration = configuration;
//        }

//        [HttpGet("getById/{id}")]
//        public async Task<IActionResult> GetUsersById(int id)
//        {
//            try
//            {
//                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("SQLServer-Connection")))
//                {
//                    await connection.OpenAsync();
//                    var command = new SqlCommand("GetCustomerById", connection);
//                    command.CommandType = CommandType.StoredProcedure;
//                    command.Parameters.AddWithValue("@Id", id);

//                    var reader = await command.ExecuteReaderAsync();

//                    if (await reader.ReadAsync())
//                    {
//                        var users = new Users
//                        {
//                            Id = (int)reader["Id"],
//                            FullName = reader["fullname"].ToString(),
//                            Email = reader["email"].ToString(),
//                            Gender = (int)reader["gender"],
//                            PhoneNumber = reader["phone_number"].ToString()
//                        };

//                        return Ok(users);
//                    }
//                    else
//                    {
//                        return NotFound("Không tìm thấy user với ID " + id);
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(ex.Message);
//            }
//        }

//    }
//}
