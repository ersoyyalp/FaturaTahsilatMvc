using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectAspNetCore.Models
{
    public class FaturaGuncelleModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Ad alanı doldurulmalıdır.")]
        public string Ad { get; set; }
        [Range(1, double.MaxValue, ErrorMessage = "Fiyat 0'dan büyük olmalıdır.")]
        public decimal Fiyat { get; set; }
        public int FaturaDonemi { get; set; }
        public IFormFile Resim { get; set; }
        public string Aciklama { get; set; }
    }
}
