using Microsoft.AspNetCore.Http;
using ProjectAspNetCore.CustomExtensions;
using ProjectAspNetCore.Entities;
using ProjectAspNetCore.Interfaces;
using System.Collections.Generic;

namespace ProjectAspNetCore.Repositories
{
    public class SepetRepository : ISepetRepository
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public SepetRepository(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public void SepetEkle(FaturaBilgi faturaBilgi)
        {
            var gelenListe = _httpContextAccessor.HttpContext.Session.GetObject<List<FaturaBilgi>>("sepet");

            if (gelenListe == null)
            {
                gelenListe = new List<FaturaBilgi>();
                gelenListe.Add(faturaBilgi);
            }
            else
            {
                gelenListe.Add(faturaBilgi);
            }
            _httpContextAccessor.HttpContext.Session.SetObject("sepet", gelenListe);
        }
        public void SepettenCikar(FaturaBilgi urun)
        {
            var gelenListe = 
                _httpContextAccessor.HttpContext.Session.GetObject<List<FaturaBilgi>>("sepet");
            gelenListe.Remove(urun);
            _httpContextAccessor.HttpContext.Session.SetObject("sepet", gelenListe);
        }
        public List<FaturaBilgi> GetirSepettekiFaturalar()
        {
            return _httpContextAccessor.HttpContext.Session.GetObject<List<FaturaBilgi>>("sepet");
          
        }
        public void SepetiBosalt()
        {
            _httpContextAccessor.HttpContext.Session.Remove("sepet");
        }
        
    }
}
