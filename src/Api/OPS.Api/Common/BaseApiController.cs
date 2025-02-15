using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OPS.Api.Common;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ApiController : ControllerBase;