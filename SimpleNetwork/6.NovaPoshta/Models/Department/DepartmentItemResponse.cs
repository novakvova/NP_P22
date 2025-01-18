using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _6.NovaPoshta.Models.Department
{
    public class DepartmentItemResponse
    {
        public string Ref { get; set; } = string.Empty;
        public string CityRef { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string ShortAddress { get; set; } = string.Empty;
    }
}
