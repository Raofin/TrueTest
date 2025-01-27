using Microsoft.AspNetCore.Mvc;

namespace OPS.Api.Controllers;

public class HelloWorldController : ApiController
{
    [HttpGet()]
    public string Get()
    {
        return "Hello World!";
    }
}
