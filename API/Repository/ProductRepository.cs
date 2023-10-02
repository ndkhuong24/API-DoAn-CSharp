﻿using API.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace API.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductDbcontext _dbcontext;

        public ProductRepository(ProductDbcontext productDbcontext)
        {
            _dbcontext = productDbcontext;
        }

        public async Task<Product> AddProductAsync(Product product)
        {
            try
            {
                var code = new SqlParameter("@Code", product.code);
                var name = new SqlParameter("@Name", product.name);
                var style_id = new SqlParameter("@Style_id", product.style_id);
                var description = new SqlParameter("@Description", product.description);
                var status = new SqlParameter("@Status", product.status);
                await _dbcontext.Database.ExecuteSqlRawAsync("EXEC PostProduct @Code, @Name, @Style_id, @Description, @Status", code, name, style_id, description, status);
                return product;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteProductAsync(int id)
        {
            try
            {
                await _dbcontext.Database.ExecuteSqlRawAsync("EXEC DeleteProduct {0}", id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<Product>> GetAllProductAsync()
        {
            try
            {
                var products = await _dbcontext.Product!.FromSqlRaw("EXEC GetProduct").ToListAsync();
                return products;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Product> GetProductAsync(int id)
        {
            try
            {
                var product = (await _dbcontext.Product!.FromSqlRaw("EXEC GetProduct {0}", id).ToListAsync()).FirstOrDefault();
                return product;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task UpdateProductAsync(int id, Product product)
        {
            try
            {
                var ids = new SqlParameter("@Id", id);
                var code = new SqlParameter("@Code", product.code);
                var name = new SqlParameter("@Name", product.name);
                var style_id = new SqlParameter("@Style_id", product.style_id);
                var description = new SqlParameter("@Description", product.description);
                var status = new SqlParameter("@Status", product.status);
                await _dbcontext.Database.ExecuteSqlRawAsync("EXEC UpdateProduct @Id,@Code,@Name,@Style_id,@Description,@Status", ids, code, name, style_id, description, status);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
