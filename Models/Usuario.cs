using System.ComponentModel.DataAnnotations.Schema;

namespace SPCOIN.Models
{
    public class Usuario
    {
      
            public int CodigoUsuario { get; set; }
            public string? User { get; set; }
            public string? Contraseña { get; set; }

            [NotMapped]
            public bool MantenerActivo { get; set; }
        
    }
}
