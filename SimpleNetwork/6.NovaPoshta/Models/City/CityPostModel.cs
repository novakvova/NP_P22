using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _6.NovaPoshta.Models.City
{
    public class CityPostModel
    {
        public string ApiKey { get; set; } = string.Empty;
        public string ModelName { get; set; } = "Address";
        public string CalledMethod { get; set; } = "getCities";
        public MethodCityProperties? MethodProperties { get; set; }
    }

    public class MethodCityProperties
    {
        public string AreaRef { get; set; } = String.Empty;
    }
}
