using eCommerceLibrary.Logs;
using eCommerceLibrary.Response;
using Microsoft.EntityFrameworkCore;
using ProductApi.Application.Interfaces;
using ProductApi.Domain.Entities;
using ProductApi.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProductApi.Infrastructure.Repositories
{
    public class ProductRepo(ProductContext context) : IProduct
    {
        public async Task<Responses> CreateAsync(Product entity)
        {
            try
            {
                var getProduct = await GetByAsync(_ => _.NameProduct.Equals(entity.NameProduct));
                if (getProduct != null || !string.IsNullOrEmpty(getProduct.NameProduct)) 
                {
                    return new Responses(false, $"{getProduct.NameProduct} already exist");
                }
                var nowEntity = context.Products.Add(entity).Entity;
                await context.SaveChangesAsync();
                if (nowEntity != null || nowEntity.Id > 0) 
                {
                    return new Responses(true, $"{entity.NameProduct} added successfully");
                }
                else
                {
                    return new Responses(false, $"Error save changes {entity.NameProduct}");
                }
            }catch (Exception ex)
            {
                //log exception
                LogExceptions.LogException(ex);
                //display scary-free message to the client
                return new Responses(false, "Error occured while adding");
            }
        }

        public async Task<Responses> DeleteAsync(Product entity)
        {
            try
            {
               var product = await FindByIdAsync(entity.Id);
                if(product is null)
                     return new Responses(false, "Not found");   
                
                context.Products.Remove(product);
                await context.SaveChangesAsync();
                return new Responses(true, "Deleted successfully");
                
            }
            catch (Exception ex)
            {
                //log exception
                LogExceptions.LogException(ex);
                //display scary-free message to the client
                return new Responses(false, "Error occured while deleted");
            }
        }

        public async Task<Product> FindByIdAsync(int id)
        {
            try
            {
                var product = await context.Products.FindAsync(id);
                return product is not null ? product : null!;
            }
            catch (Exception ex)
            {
                //log exception
                LogExceptions.LogException(ex);
                //display scary-free message to the client
                throw new Exception("Error occurred retrieve product");
            }
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            try
            {
                var products = await context.Products.AsNoTracking().ToListAsync();
                return products is not null ? products : null!;
            }
            catch (Exception ex)
            {
                //log exception
                LogExceptions.LogException(ex);
                //display scary-free message to the client
                throw new Exception("Error occurred retrieve list product");
            }
        }

        public async Task<Product> GetByAsync(Expression<Func<Product, bool>> predicate)
        {
            try
            {
                var product = await context.Products.Where(predicate).FirstOrDefaultAsync()!;
                return product is not null ? product : null!;
            }
            catch (Exception ex)
            {
                //log exception
                LogExceptions.LogException(ex);
                //display scary-free message to the client
                throw new Exception("Error occurred retrieve list product");
            }
        }

        public async Task<Responses> UpdateAsync(Product entity)
        {
            try
            {
                var product = await FindByIdAsync(entity.Id);
                if (product is null)
                {
                    return new Responses(false, $"{product.NameProduct} not found");
                }
                context.Entry(product).State = EntityState.Detached;
                context.Products.Update(entity);
                await context.SaveChangesAsync();
                return new Responses(true, $"{entity.NameProduct} is updated successfully");
            }
            catch (Exception ex)
            {
                //log exception
                LogExceptions.LogException(ex);
                //display scary-free message to the client
                return new Responses(false, "Error occurred while update product");
            }
        }
    }
}
