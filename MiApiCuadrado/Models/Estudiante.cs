using System.ComponentModel.DataAnnotations;

namespace MiApiCuadrado.Models
{
    public class Estudiante
    {

        [Required]
        public int Id { get; set; } 
          
        [Required]
        public string Nombre { get; set; } = string.Empty;    
        [Required]
        public string Apellido { get; set; } = string.Empty;
        [Required]
        public string Matricula { get; set; } = string.Empty;
        [Required]  
        public string Carrera { get; set; } = string.Empty;

        [Required]
        public int CantidadMaterias { get; set; }
        [Required]  
        public int Edad { get; set; }


    }
}