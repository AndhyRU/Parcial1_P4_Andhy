using Microsoft.AspNetCore.Mvc;
using Parcial1_P4_Andhy.Models;
using Parcial1_P4_Andhy.Services;

namespace Parcial1_P4_Andhy.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NumberController(NumbersService numbersService)
        : ControllerBase
    {

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

            var record = new NumberRecordSet(
                numero,
                resultado
            );

            await numbersService.SaveAsync(record);

            return Ok(record);
        }


        [HttpGet("historial")]
        public async Task<IActionResult> GetHistorial()
        {
            var historial =
                await numbersService.GetListAsync();

            return Ok(historial);
        }


        [HttpGet("historial/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var registro =
                await numbersService.GetByIdAsync(id);

            if (registro == null)
            {
                return NotFound(
                    "No se encontró el registro solicitado.");
            }

            return Ok(registro);
        }


        [HttpPut("historial/{id:int}")]
        public async Task<IActionResult> Actualizar(
            int id,
            [FromBody] NumberRecordSet record)
        {
            var existente =
                await numbersService.GetByIdAsync(id);

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

            var actualizado = new NumberRecordSet(
                record.Numero,
                resultado
            );

            await numbersService.UpdateAsync(id, actualizado);

            return Ok(actualizado);
        }
    }
}