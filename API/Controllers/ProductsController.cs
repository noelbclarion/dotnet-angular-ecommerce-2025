using Microsoft.AspNetCore.Mvc;
using Infrastructure.Data;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.CodeAnalysis;
using API.RequestHelpers;

namespace API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ProductsController(IGenericRepository<Product> _productRepository) : ControllerBase
{

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts([FromQuery]ProductSpecParams specParams)
    {
        var spec = new ProductFilterSortPaginationSpecification(specParams);
        var products = await _productRepository.ListAsync(spec);
        var count = await _productRepository.CountAsync(spec);

        var pagination =  new Pagination<Product>(specParams.PageIndex, specParams.PageSize, count, products);

        return Ok(pagination);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null) return NotFound();
        return Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        _productRepository.Add(product);
        if(await _productRepository.SaveAllAsync())
        {
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }
        return BadRequest("Failed to create product");
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<Product>> UpdateProduct(int id, Product product)
    {
        if (id != product.Id || !ProductExists(id)) return BadRequest();
        _productRepository.Update(product);
        if(await _productRepository.SaveAllAsync())
        {
            return NoContent();
        }
        return BadRequest("Failed to update product");
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<Product>> DeleteProduct(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null) return NotFound();
        _productRepository.Delete(product);
        if(await _productRepository.SaveAllAsync())
        {
            return NoContent();
        }
        return BadRequest("Failed to delete product");
    }

    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetProductBrands()
    {
        var spec = new BrandListSpecification();
        return Ok(await _productRepository.ListAsync(spec));
    }

    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetProductTypes()
    {
        var spec = new TypeListSpecification();
        return Ok(await _productRepository.ListAsync(spec));
    }

    private bool ProductExists(int id)
    {
        return _productRepository.Exists(id);
    }
}

