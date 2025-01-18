using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _6.NovaPoshta.Models.Department
{
    public class DepartmentResponse
    {
        public bool Success { get; set; }
        public List<DepartmentItemResponse> Data { get; set; }
        public List<object>? Errors { get; set; }
        public List<object>? Warnings { get; set; }
        public object? Info { get; set; }
        public List<object>? MessageCodes { get; set; }
        public List<object>? ErrorCodes { get; set; }
        public List<object>? WarningCodes { get; set; }
        public List<object>? InfoCodes { get; set; }
    }
}
