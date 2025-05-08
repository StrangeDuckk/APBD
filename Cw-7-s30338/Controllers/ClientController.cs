using Cw_7_s30338.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cw_7_s30338.Controllers;

[ApiController]
[Route("[controller]")]
public class ClientController(IDbService service):ControllerBase
{
    
}