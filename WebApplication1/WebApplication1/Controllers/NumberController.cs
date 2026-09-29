using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NumberController : ControllerBase
    {
        [HttpGet("{numero}")]
        public IActionResult Sumar(int numero)
        {
            int resultado = numero + numero;

            return Ok(resultado);
        }
    }
}
