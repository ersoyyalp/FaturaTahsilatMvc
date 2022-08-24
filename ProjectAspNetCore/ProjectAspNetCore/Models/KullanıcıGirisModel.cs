using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectAspNetCore.Models
{
    public class KullanıcıGirisModel
    {
        [Required(ErrorMessage ="Kullanıcı Adı Boş Bırakılamaz")]
        public string KullaniciAd { get; set; }
        [Required(ErrorMessage = "Şifre Boş Bırakılamaz")]
        public string Sifre { get; set; }
        public bool BeniHatirla { get; set; }

    }
}
