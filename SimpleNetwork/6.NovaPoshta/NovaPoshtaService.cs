using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;
using _6.NovaPoshta.Data;
using _6.NovaPoshta.Data.Entities;
using _6.NovaPoshta.Mapping;
using _6.NovaPoshta.Models;
using _6.NovaPoshta.Models.Area;
using _6.NovaPoshta.Models.City;
using _6.NovaPoshta.Models.Department;
using AutoMapper;
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
        private readonly int _procesLimit;
        private readonly IMapper _mapper;

        public NovaPoshtaService()
        {
            _httpClient = new HttpClient();
            _url = "https://api.novaposhta.ua/v2.0/json/";
            _context = new BombaDbContext();
            _context.Database.Migrate(); //накатує на БД усі міграції, який там немає

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = config.CreateMapper();

            _procesLimit = Environment.ProcessorCount-2;//10;
        }

        public async Task SeedAreas()
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
                if (response.IsSuccessStatusCode)
                {
                    string jsonResp = response.Content.ReadAsStringAsync().Result; //Читаємо відповідь від сервера
                    var result = JsonConvert.DeserializeObject<AreaResponse>(jsonResp);
                    if (result != null && result.Data != null && result.Success)
                    {
                        using var semaphore = new SemaphoreSlim(_procesLimit);

                        await Parallel.ForEachAsync(result.Data, async (item, x) =>
                        {
                            try
                            {
                                var entity = _mapper.Map<AreaEntity>(item);
                                using var localContext = new BombaDbContext();
                                await localContext.Areas.AddAsync(entity);
                                await localContext.SaveChangesAsync();
                            }
                            finally
                            {
                                semaphore.Release();
                            }
                        });
                    }
                }
            }
        }

        public async Task SeedCities()
        {
            if (!_context.Cities.Any()) // Якщо таблиця пуста
            {
                var listAreas = GetListAreas();
                using var semaphore = new SemaphoreSlim(_procesLimit);

                await Parallel.ForEachAsync(listAreas, async (area, x) =>
                {
                    await semaphore.WaitAsync();
                    try
                    {
                        Console.WriteLine("Seed area {0}...", area.Description);
                        var modelRequest = new CityPostModel
                        {
                            ApiKey = "c44c00290a5023fcc0ff81091471dda1",
                            MethodProperties = new MethodCityProperties()
                            {
                                AreaRef = area.Ref
                            }
                        };

                        string json = JsonConvert.SerializeObject(modelRequest, new JsonSerializerSettings
                        {
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

                                var cityEntities = result.Data.Select(city =>
                                {
                                    var entity = _mapper.Map<CityEntity>(city);
                                    entity.AreaId = area.Id;
                                    return entity;
                                });

                                using var localContext = new BombaDbContext();
                                await localContext.Cities.AddRangeAsync(cityEntities);
                                await localContext.SaveChangesAsync();
                            }
                        }
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                });

            }
        }

        public async Task SeedDepartments()
        {
            if (!_context.Departments.Any()) // Якщо таблиця пуста
            {
                var listCities = _context.Cities.ToList();
                using var semaphore = new SemaphoreSlim(_procesLimit);
                await Parallel.ForEachAsync(listCities, async (city, _) =>
                {
                    await semaphore.WaitAsync();
                    try
                    {
                        Console.WriteLine("Seed city {0}...", city.Description);
                        var modelRequest = new DepartmentPostModel
                        {
                            ApiKey = "c44c00290a5023fcc0ff81091471dda1",
                            MethodProperties = new MethodDepatmentProperties()
                            {
                                CityRef = city.Ref
                            }
                        };

                        string json = JsonConvert.SerializeObject(modelRequest, new JsonSerializerSettings
                        {
                            Formatting = Formatting.Indented // Для кращого вигляду (не обов'язково)
                        });
                        HttpContent context = new StringContent(json, Encoding.UTF8, "application/json");
                        HttpResponseMessage response = _httpClient.PostAsync(_url, context).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            string jsonResp = response.Content.ReadAsStringAsync().Result; //Читаємо відповідь від сервера
                            var result = JsonConvert.DeserializeObject<DepartmentResponse>(jsonResp);
                            if (result != null && result.Data != null && result.Success)
                            {
                                var departmentEntities = result.Data.Select(dep =>
                                {
                                    var entity = _mapper.Map<DepartmentEntity>(dep);
                                    entity.CityId = city.Id;
                                    return entity;
                                });

                                using var localContext = new BombaDbContext();
                                await localContext.Departments.AddRangeAsync(departmentEntities);
                                await localContext.SaveChangesAsync();
                            }
                        }
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                });

            }
        }

        public List<AreaEntity> GetListAreas()
        {
            return _context.Areas.ToList();
        }
    }
}
