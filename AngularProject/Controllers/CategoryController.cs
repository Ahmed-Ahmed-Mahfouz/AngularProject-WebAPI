using AngularProject.Models;
using AngularProject.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AngularProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private IUnitOfWorks unit;
        public CategoryController(IUnitOfWorks unit)
        {
            this.unit = unit;
        }

        [HttpGet]
        public ActionResult GetAll()
        {
            var categories = unit.CategoryRepository.GetAll(includeProperties:"");
            return Ok(categories.Select(c=> new {c.Id, c.Name}));
        }

        [HttpGet("{id}")]
        public ActionResult GetById(int id)
        {
            var category = unit.CategoryRepository.GetById((p) => p.Id == id, "Category");
            if (category == null)
            {
                return NotFound();
            }
            return Ok(new { category.Id, category.Name});
        }

        [HttpPost]
        public ActionResult Add([FromBody] Category category)
        {
            unit.CategoryRepository.Add(category);
            unit.CategoryRepository.Save();
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody] Category category)
        {
            if (id != category.Id)
            {
                return BadRequest();
            }
            var existingCategory = unit.CategoryRepository.GetById((p) => p.Id == id, "Category");
            if (existingCategory == null)
            {
                return NotFound();
            }
            existingCategory.Name = category.Name;
            existingCategory.Id = category.Id;
            unit.CategoryRepository.Update(existingCategory);
            unit.CategoryRepository.Save();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var category = unit.CategoryRepository.GetById((p) => p.Id == id, "Category");
            if (category == null)
            {
                return NotFound();
            }
            unit.CategoryRepository.Delete(id);
            unit.CategoryRepository.Save();
            return NoContent();
        }
    }
}
