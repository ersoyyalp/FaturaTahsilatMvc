using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectAspNetCore.Models
{
    public class FaturaEkleModel
    {
        [Required(ErrorMessage="Ad alanı doldurulmalıdır.")]
        public string Ad { get; set; }
        [Range(3,double.MinValue,ErrorMessage ="Fiyat 0'dan büyük olmalıdır.")]
        public int Fiyat { get; set; }
        public int FaturaDonemi { get; set; }
        public int UserId { get; set; }
        public int CompanyId { get; set; }
        public IFormFile Resim { get; set; }
        public int CategoryId { get; set; }
        public string Aciklama { get; set; }

    }
}
