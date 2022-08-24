using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectAspNetCore.CustomExtensions
{
    public static class CustomSessionExtension
    {
        public static void SetObject<T>(this ISession session, string key, T value) where T : class, new()
        {   //SerializeObject: nesneyi veritabanına depolamak için bir nesneyi bayt akışına dönüştürme işlemidir
            var stringData = JsonConvert.SerializeObject(value);
            session.SetString(key, stringData);
        }
        public static T GetObject<T>(this ISession session, string key) where T : class, new()
        {
            var JsonData = session.GetString(key);
            if (!string.IsNullOrWhiteSpace(JsonData))
            {
                return JsonConvert.DeserializeObject<T>(JsonData);
            }
            return null;
        }
    }
}
