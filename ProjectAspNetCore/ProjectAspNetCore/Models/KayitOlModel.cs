using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectAspNetCore.Models
{
    public class KayitOlModel
    {
        [Required(ErrorMessage = "Ad alanı  Boş Bırakılamaz")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Soyad alanı  Boş Bırakılamaz")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "Boş Bırakılamaz")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Mail alanı  Boş Bırakılamaz")]
        public string Mail { get; set; }
        [Required(ErrorMessage = "Ad alanı  Boş Bırakılamaz")]
        public string Password { get; set; }
        public string PasswordAgain { get; set; }
        public int Type { get; set; }
        public int? CategoryId { get; set; }
    }
}
