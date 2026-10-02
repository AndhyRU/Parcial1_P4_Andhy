using Microsoft.AspNetCore.Mvc;
using Parcial1_P4_Andhy.Models;
using Parcial1_P4_Andhy.Services;

namespace Parcial1_P4_Andhy.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NumberController : ControllerBase
    {
        private readonly NumbersService _numbersService;

        public NumberController(NumbersService service)
        {
            _numbersService = service;
        }

        //[HttpGet("{numero:int}")]
        //public IActionResult Sumar(int numero)
        //{
        //    int resultado = numero + numero;

        //    return Ok(resultado);
        //}


        // Sumar y guardar el cálculo.
        [HttpGet("{numero:int}")]
        public async Task<IActionResult> Sumar(int numero)
        {
            int resultado;

            try
            {
                resultado = checked(numero + numero);
            }
            catch (OverflowException)
            {
                return BadRequest("El número es demasiado grande.");
            }

            var record = new NumberRecord
            {
                Numero = numero,
                Resultado = resultado,
                Fecha = DateTime.UtcNow
            };

            await _numbersService.SaveAsync(record);

            return Ok(record);
        }

        // Consultar todo el historial.
        [HttpGet("historial")]
        public async Task<IActionResult> GetHistorial()
        {
            var historial =
                await _numbersService.GetListAsync();

            return Ok(historial);
        }

        // Consultar un registro por Id.
        [HttpGet("historial/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var registro =
                await _numbersService.GetByIdAsync(id);

            if (registro == null)
            {
                return NotFound(
                    "No se encontró el registro solicitado.");
            }

            return Ok(registro);
        }

        // Actualizar un cálculo.
        [HttpPut("historial/{id:int}")]
        public async Task<IActionResult> Actualizar(
            int id,
            [FromBody] NumberRecord record)
        {
            var existente =
                await _numbersService.GetByIdAsync(id);

            if (existente == null)
            {
                return NotFound(
                    "No se encontró el registro solicitado.");
            }

            int resultado;

            try
            {
                resultado = checked(record.Numero + record.Numero);
            }
            catch (OverflowException)
            {
                return BadRequest("El número es demasiado grande.");
            }

            record.Id = id;
            record.Resultado = resultado;
            record.Fecha = DateTime.UtcNow;

            await _numbersService.UpdateAsync(record);

            return Ok(record);
        }
    }
}