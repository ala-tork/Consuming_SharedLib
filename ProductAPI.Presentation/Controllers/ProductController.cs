using eCommerce.SharedLibrary.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.Application.DTOs;
using ProductAPI.Application.DTOs.Convertions;
using ProductAPI.Application.Interfaces;

namespace ProductAPI.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(IProduct _productIterface) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult> GetProduts()
        {
            var products = await _productIterface.GetAllAsync();
            if (!products.Any())
            {
                return NotFound("No Product Found");
            }
            var (_, list) = ProductConvertion.FomEntity(null, products);
            return Ok(list);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetProduts(int id)
        {
            var product = await _productIterface.FindByIdAsync(id);
            if (product is null)
            {
                return NotFound("Product not Found");
            }
            var (p, _) = ProductConvertion.FomEntity(product, null);
            return p is not null ? Ok(p) : NotFound("Product Not Found");
        }

        [HttpPost(Name ="Create_product")]
        public async Task<ActionResult<Response>> CreatePrduct(ProductDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var getEntiy = ProductConvertion.ToEntity(dto);
            var response = await _productIterface.CreateAsync(getEntiy);
            return response.Flag is true? Ok(response) : BadRequest(response);
        }

        [HttpPut]
        public async Task<ActionResult<Response>> updatePrduct(ProductDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var getEntiy = ProductConvertion.ToEntity(dto);
            var response = await _productIterface.UpdateAsync(getEntiy);
            return response.Flag is true ? Ok(response) : BadRequest(response);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteProduct(ProductDto dto)
        {
            var getEntiy = ProductConvertion.ToEntity(dto);
            var response = await _productIterface.DeleteAsync(getEntiy);
            return response.Flag is true ? Ok(response) : BadRequest(response);
        }
    }
}
