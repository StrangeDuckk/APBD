using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers;

[ApiController]
[Route("[controller]")] // tests
public class TestsController : ControllerBase
{
    [HttpGet]
    public IActionResult GetHelloWorld()
    {
        return Ok("Hello World"); // => 200
    }
}