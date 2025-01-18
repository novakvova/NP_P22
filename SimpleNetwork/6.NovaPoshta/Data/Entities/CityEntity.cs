using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _6.NovaPoshta.Data.Entities
{
    [Table("tbl_cities")]
    public class CityEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Ref { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string AreaRef { get; set; } = String.Empty;

        [Required]
        [StringLength(255)]
        public string Description { get; set; } = String.Empty;

        [Required]
        [StringLength(50)]
        public string TypeDescription { get; set; } = String.Empty;

        [ForeignKey("Area")]
        public int AreaId { get; set; }
        public AreaEntity? Area { get; set; }

        public ICollection<DepartmentEntity> Departments { get; set; } = new List<DepartmentEntity>();
    }
}
