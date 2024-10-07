using eCommerce.SharedLibrary.Logs;
using eCommerce.SharedLibrary.Responses;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using ProductAPI.Application.Interfaces;
using ProductAPI.Domain.Entities;
using ProductAPI.Infrastructure.Data;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;

namespace ProductAPI.Infrastructure.Repositories
{
    public class ProductRepository(ProductDbContext _Context) : IProduct
    {
        public async Task<Response> CreateAsync(Product entity)
        {
            try
            {
                var getproduct = await GetByAsync(_=>_.Name!.Equals(entity.Name));
                if (getproduct is not null && !string.IsNullOrEmpty(getproduct.Name))
                {
                    return new Response(false, $"{entity.Name} already addes");
                }
                var currentEntity =  await _Context.Products.AddAsync(entity);
                await _Context.SaveChangesAsync();
                if (currentEntity is not null && currentEntity.Entity.Id >0) 
                {
                    return new Response(true,$"{entity.Name} created successfully");
                }
                else
                {
                    return new Response(false,$"Error while adding this Product ");
                }

            }
            catch (Exception ex)
            {
                // Log the original exception
                LogException.LogExceptions(ex);
                // display scary_free to the client
                return new Response(false, "Error adding new Product");    
            }
        }


        public async  Task<Response> DeleteAsync(Product entity)
        {
            try
            {
                var product = await FindByIdAsync(entity.Id);
                if (product is null)
                    return new Response(false, $"{entity.Name} not Fount");
                _Context.Products.Remove(entity);
                await _Context.SaveChangesAsync();
                return new Response(true, $"{entity.Name} is deleted successfully");
            }
            catch (Exception ex)
            {
                // Log the original exception
                LogException.LogExceptions(ex);
                // display scary_free to the client
                return new Response(false, "Error occured deleting Product");
            }
        }

        public async Task<Product> FindByIdAsync(int id)
        {
            try
            {
                var prod = await _Context.Products.FindAsync(id);
                return prod is not null ? prod : null;
            }
            catch (Exception ex)
            {
                // Log the original exception
                LogException.LogExceptions(ex);
                // display scary_free to the client
                throw new Exception( "Error occured retrieving Product");
            }
        }

        public async Task<List<Product>> GetAllAsync()
        {
            try
            {
                var prod = await _Context.Products.AsNoTracking().ToListAsync();
                return prod is not null ? prod : null;
            }
            catch (Exception ex)
            {
                // Log the original exception
                LogException.LogExceptions(ex);
                // display scary_free to the client
                throw new InvalidOperationException("Error occured retrieving Products");
            }
        }

        public async Task<Product> GetByAsync(Expression<Func<Product, bool>> predicate)
        {
            try
            {
                var product = await _Context.Products.Where(predicate).FirstOrDefaultAsync();
                return product is not null ? product : null;
            }
            catch (Exception ex)
            {
                // Log the original exception
                LogException.LogExceptions(ex);
                // display scary_free to the client
                throw new InvalidOperationException("Error occured retrieving Product");
            }
        }

        public async Task<Response> UpdateAsync(Product entity)
        {
            try
            {
                var prod = await FindByIdAsync(entity.Id);
                if (prod is not null)
                    return new Response(false,$"{entity.Name} not fount");
                _Context.Entry(prod).State = EntityState.Modified;
                _Context.Products.Update(entity);
                await _Context.SaveChangesAsync();
                return new Response(true,$"{entity.Name} is updated successfully");
            }
            catch (Exception ex)
            {
                // Log the original exception
                LogException.LogExceptions(ex);
                // display scary_free to the client
                return new Response(false,"Error occured Updating existen Product");
            }
        }
    }
}
