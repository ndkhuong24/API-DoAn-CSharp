﻿using API.ViewModel;
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
        public async Task<IActionResult> GetUserAddressByUserID(int userID)
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
                            AddressID = reader["AddressID"] as int? ?? 0,
                            ProvinceID = reader["ProvinceID"]?.ToString() ?? string.Empty,
                            ProvinceName = reader["ProvinceName"]?.ToString() ?? string.Empty,
                            DistrictID = reader["DistrictID"]?.ToString() ?? string.Empty,
                            DistrictName = reader["DistrictName"]?.ToString() ?? string.Empty,
                            CommuneID = reader["CommuneID"]?.ToString() ?? string.Empty,
                            CommuneName = reader["CommuneName"]?.ToString() ?? string.Empty,
                            DetailAddress = reader["detail_address"]?.ToString() ?? string.Empty,
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
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"SQL Exception: {sqlEx.Message}");
                return BadRequest("Failed to retrieve user addresses. Please try again.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return BadRequest("Failed to retrieve user addresses. Please try again.");
            }
        }

        [HttpPost("{userID}")]
        public async Task<IActionResult> UpdateAddressDefault(int addressID, int userID)
        {
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("SQLServer-Connection")))
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand("UpdateAddressDefault", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@AddressID", addressID);
                        command.Parameters.AddWithValue("@UserID", userID);

                        await command.ExecuteNonQueryAsync();

                        return Ok("Address updated successfully");
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                return BadRequest($"SQL Exception: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                return BadRequest($"Exception: {ex.Message}");
            }
        }

        [HttpPost("add/{userID}")]
        public async Task<IActionResult> InsertAddress(UserAddress userAddress, int userID)
        {
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("SQLServer-Connection")))
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand("InsertAddress", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@ProvinceID", userAddress.ProvinceID);
                        command.Parameters.AddWithValue("@ProvinceName", userAddress.ProvinceName);
                        command.Parameters.AddWithValue("@DistrictID", userAddress.DistrictID);
                        command.Parameters.AddWithValue("@DistrictName", userAddress.DistrictName);
                        command.Parameters.AddWithValue("@CommuneID", userAddress.CommuneID);
                        command.Parameters.AddWithValue("@CommuneName", userAddress.CommuneName);
                        command.Parameters.AddWithValue("@DetailAddress", userAddress.DetailAddress);
                        command.Parameters.AddWithValue("@Status", userAddress.Status);
                        command.Parameters.AddWithValue("@UserID", userID);

                        await command.ExecuteNonQueryAsync();

                        return Ok("Insert address successfully");
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                return BadRequest($"SQL Exception: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                return BadRequest($"Exception: {ex.Message}");
            }
        }


        [HttpDelete("delete/{userID}/{addressId}")]
        public async Task<IActionResult> DeleteAddress(int userID, int addressId)
        {
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("SQLServer-Connection")))
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand("DeleteAddress", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@AddressID", addressId);
                        command.Parameters.AddWithValue("@UserID", userID);

                        await command.ExecuteNonQueryAsync();

                        return Ok("Delete address successfully");
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                return BadRequest($"SQL Exception: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                return BadRequest($"Exception: {ex.Message}");
            }
        }




    }
}
