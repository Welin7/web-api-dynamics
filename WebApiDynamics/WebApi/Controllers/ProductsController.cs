using Microsoft.AspNetCore.Mvc;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : DynamicController<Product>
{
    public ProductsController(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
    {

    }
}
