using AngularProject.DTO;
using AngularProject.Models;
using AngularProject.UnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AngularProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        IUnitOfWorks unit;
        public ProductController(IUnitOfWorks unit)
        {
            this.unit = unit;
        }

        [HttpGet]
        public ActionResult getall([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 4)
        {
            var (products, totalCount) = unit.ProductRepository.GetAllPaged(pageNumber, pageSize, includeProperties: "");
            var response = new
            {
                TotalCount = totalCount,
                PageSize = pageSize,
                PageNumber = pageNumber,
                Items = products.Select(p => new ProductDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Description = p.Description,
                    ImageUrl = p.ImageUrl,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category?.Name,
                    Brand = p.Brand,
                    Stock = p.Stock,
                    ModelYear = p.ModelYear
                })
            };
            return Ok(response);
        }

        [HttpGet]
        [Route("getbycategory/{id}")]
        public ActionResult getbycategory(int id, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 4)
        {
            var (products, totalCount) = unit.ProductRepository
                .GetAllPaged(pageNumber, pageSize, includeProperties: "Category");

            // Filter products by category
            products = products.Where(p => p.CategoryId == id).ToList();

            totalCount = products.Count;

            // Apply pagination after filtering
            var paginatedProducts = products.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            var response = new
            {
                TotalCount = totalCount,
                PageSize = pageSize,
                PageNumber = pageNumber,
                Items = paginatedProducts.Select(p => new ProductDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Description = p.Description,
                    ImageUrl = p.ImageUrl,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category?.Name,
                    Brand = p.Brand,
                    Stock = p.Stock,
                    ModelYear = p.ModelYear
                })
            };

            return Ok(response);
        }





        [HttpGet("{id}")]
        public ActionResult getbyid(int id)
        {
            var product = unit.ProductRepository.GetById((p) => p.Id == id, "Category");
            if (product == null)
            {
                return NotFound();
            }
            var productDTO = new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                ImageUrl = product.ImageUrl,
                CategoryId = product.CategoryId,
                CategoryName = product.Category?.Name,
                Brand = product.Brand,
                Stock = product.Stock,
                ModelYear = product.ModelYear
            };
            return Ok(productDTO);
        }

        [HttpPost]
        [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult add([FromBody] ProductDTO productDTO)
        {
            var product = new Product
            {
                Name = productDTO.Name,
                Price = productDTO.Price,
                Description = productDTO.Description,
                ImageUrl = productDTO.ImageUrl,
                CategoryId = productDTO.CategoryId,
                Brand = productDTO.Brand,
                Stock = productDTO.Stock,
                ModelYear = productDTO.ModelYear
            };
            unit.ProductRepository.Add(product);
            unit.Save();
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult update(int id, [FromBody] ProductDTO productDTO)
        {
            if (id != productDTO.Id)
            {
                return BadRequest();
            }
            var productToUpdate = unit.ProductRepository.GetById((p) => p.Id == id, "Category");
            if (productToUpdate == null)
            {
                return NotFound();
            }
            productToUpdate.Name = productDTO.Name;
            productToUpdate.Price = productDTO.Price;
            productToUpdate.Description = productDTO.Description;
            productToUpdate.ImageUrl = productDTO.ImageUrl;
            productToUpdate.CategoryId = productDTO.CategoryId;
            productToUpdate.Brand = productDTO.Brand;
            productToUpdate.Stock = productDTO.Stock;
            productToUpdate.ModelYear = productDTO.ModelYear;

            unit.ProductRepository.Update(productToUpdate);
            unit.Save();
            return StatusCode(StatusCodes.Status204NoContent);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult delete(int id)
        {
            if (unit.ProductRepository.GetById((p) => p.Id == id)==null)
            {
                return NotFound();
            }
            unit.ProductRepository.Delete(id);
            unit.Save();
            return StatusCode(StatusCodes.Status204NoContent);
        }
    }
}
