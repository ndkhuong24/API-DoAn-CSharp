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

        [HttpPut("update")]
        public async Task<IActionResult> UpdateUser([FromBody] Users updatedUser)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("SQLServer-Connection")))
                {
                    await connection.OpenAsync();
                    var command = new SqlCommand("UpdateUser", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    // Add parameters for the stored procedure
                    command.Parameters.AddWithValue("@Id", updatedUser.Id);
                    command.Parameters.AddWithValue("@FullName", updatedUser.FullName);
                    command.Parameters.AddWithValue("@Email", updatedUser.Email);
                    command.Parameters.AddWithValue("@Gender", updatedUser.Gender);
                    command.Parameters.AddWithValue("@PhoneNumber", updatedUser.PhoneNumber);

                    // Execute the update stored procedure
                    var rowsAffected = await command.ExecuteNonQueryAsync();

                    if (rowsAffected > 0)
                    {
                        return Ok($"User with ID {updatedUser.Id} updated successfully");
                    }
                    else
                    {
                        return NotFound($"User with ID {updatedUser.Id} not found");
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"SQL Exception: {sqlEx.Message}");
                return BadRequest("Failed to update user. Please try again.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return BadRequest("Failed to update user. Please try again.");
            }
        }
    }
}