using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _6.NovaPoshta.Data.Entities;

[Table("tbl_areas")]
public class AreaEntity
{
    [Key]
    public int Id { get; set; } // Первинний ключ

    [Required]
    [StringLength(50)]
    public string Ref { get; set; } = String.Empty; // Унікальне поле "Ref"

    [Required]
    [StringLength(50)]
    public string AreasCenter { get; set; } = String.Empty; // Відображає "AreasCenter"

    [Required]
    [StringLength(255)]
    public string Description { get; set; } = String.Empty; // Відображає "Description"
}
