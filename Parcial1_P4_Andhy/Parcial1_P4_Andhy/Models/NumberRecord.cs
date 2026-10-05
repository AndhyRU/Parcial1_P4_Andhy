using System.ComponentModel.DataAnnotations;

namespace Parcial1_P4_Andhy.Models;

public record NumberRecordGet(long Id, string Fecha, long Numero, long Resultado);

public record NumberRecordSet(int Numero, int Resultado);
