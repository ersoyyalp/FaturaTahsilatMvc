using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectAspNetCore.Entities
{
    public class FaturaKategori
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Ad { get; set; }

        public List<FaturaBilgiKategori> FaturaBilgiKategori { get; set; }
        public List<AppUser> AppUsers { get; set; }

    }
}
