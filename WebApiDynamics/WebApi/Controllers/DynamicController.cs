using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DynamicController<T> : ControllerBase where T : class
{
    private readonly ApplicationDbContext _applicationDbContext;

    public DynamicController(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<T>>> GetAll()
    {
        return await _applicationDbContext.Set<T>().ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<T>> GetById(int id)
    {
        var entity = await _applicationDbContext.Set<T>().FindAsync(id);

        if (entity == null) 
        {
            return NotFound();
        }

        return entity;        
    }

    [HttpPost]
    public async Task<ActionResult<T>> Post(T entity)
    {
        _applicationDbContext.Set<T>().Add(entity);
        await _applicationDbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = entity.GetType().GetProperty("Id").GetValue(entity)}, entity);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<T>> Put(int id, T entity)
    {
        if (id != (int)entity.GetType().GetProperty("Id").GetValue(entity))
        {
            return BadRequest();
        }
        
        _applicationDbContext.Entry(entity).State = EntityState.Modified;
        await _applicationDbContext.SaveChangesAsync();
        return NoContent();           
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<T>> Delete(int id)
    {
        var entity = await _applicationDbContext.Set<T>().FindAsync(id);

        if (entity == null)
        {
            return NotFound();
        }
        
        _applicationDbContext.Set<T>().Remove(entity);
        await _applicationDbContext.SaveChangesAsync();
        return NoContent();
    }
}
