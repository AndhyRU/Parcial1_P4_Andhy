using Microsoft.AspNetCore.Mvc;

namespace Parcial1_P4_Andhy.Controllers
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
