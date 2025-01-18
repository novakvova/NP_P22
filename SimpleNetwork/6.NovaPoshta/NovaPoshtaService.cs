using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;
using _6.NovaPoshta.Data;
using _6.NovaPoshta.Data.Entities;
using _6.NovaPoshta.Models;
using _6.NovaPoshta.Models.Area;
using _6.NovaPoshta.Models.City;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace _6.NovaPoshta
{
    public class NovaPoshtaService
    {
        private readonly HttpClient _httpClient;
        private readonly string _url;
        private readonly BombaDbContext _context;

        public NovaPoshtaService()
        {
            _httpClient = new HttpClient();
            _url = "https://api.novaposhta.ua/v2.0/json/";
            _context = new BombaDbContext();
            _context.Database.Migrate(); //накатує на БД усі міграції, який там немає
        }

        public void SeedAreas()
        {
            if (!_context.Areas.Any()) // Якщо таблиця пуста
            {
                var modelRequest = new AreaPostModel
                {
                    ApiKey = "c44c00290a5023fcc0ff81091471dda1",
                    ModelName = "Address",
                    CalledMethod = "getAreas",
                    MethodProperties = new MethodProperties() { }
                };

                string json = JsonConvert.SerializeObject(modelRequest, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    Formatting = Formatting.Indented // Для кращого вигляду (не обов'язково)
                });
                HttpContent context = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PostAsync(_url, context).Result;
                if(response.IsSuccessStatusCode)
                {
                    string jsonResp = response.Content.ReadAsStringAsync().Result; //Читаємо відповідь від сервера
                    var result = JsonConvert.DeserializeObject<AreaResponse>(jsonResp);
                    if (result!=null && result.Data != null && result.Success)
                    {
                        foreach(var item in result.Data)
                        {
                            var entity = new AreaEntity
                            {
                                Ref = item.Ref,
                                AreasCenter = item.AreasCenter,
                                Description = item.Description,
                            };
                            _context.Areas.Add(entity);
                            _context.SaveChanges();
                        }
                    }
                }
            }
        }

        public void SeedCities()
        {
            if (!_context.Cities.Any()) // Якщо таблиця пуста
            {
                var modelRequest = new CityPostModel
                {
                    ApiKey = "c44c00290a5023fcc0ff81091471dda1",
                    MethodProperties = new MethodCityProperties() { }
                };

                string json = JsonConvert.SerializeObject(modelRequest, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    Formatting = Formatting.Indented // Для кращого вигляду (не обов'язково)
                });
                HttpContent context = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PostAsync(_url, context).Result;
                if (response.IsSuccessStatusCode)
                {
                    string jsonResp = response.Content.ReadAsStringAsync().Result; //Читаємо відповідь від сервера
                    var result = JsonConvert.DeserializeObject<CityResponse>(jsonResp);
                    if (result != null && result.Data != null && result.Success)
                    {
                        Console.WriteLine("List cities count {0}", result.Data.Count);
                        var listAreas = GetListAreas();
                        foreach (var city in result.Data)
                        {
                            var area = listAreas.SingleOrDefault(x => x.Ref == city.Area);
                            if (area != null) {
                                var cityEntity = new CityEntity
                                {
                                    Ref = city.Ref,
                                    Description = city.Description,
                                    TypeDescription = city.SettlementTypeDescription,
                                    AreaRef = city.Area,
                                    AreaId =area.Id
                                };
                                _context.Cities.Add(cityEntity);
                            }
                        }
                        _context.SaveChanges();
                    }
                }
            }
        }


        public List<AreaEntity> GetListAreas()
        {
            return _context.Areas.ToList();
        }
    }
}
