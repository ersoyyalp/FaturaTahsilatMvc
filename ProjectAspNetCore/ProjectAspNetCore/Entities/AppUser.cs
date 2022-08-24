using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectAspNetCore.Entities
{
    public class AppUser : IdentityUser<int>
    {
        public string Name { get; set; }
        public string SurName { get; set; }
        public string AccountNumber { get; set; }
        public int Type { get; set; }
        public int? FaturaKategoriId { get; set; }
        public FaturaKategori FaturaKategori { get; set; }
        public List<FaturaBilgi> CustomerFatura { get; set; }
        public List<FaturaBilgi> CompanyFatura { get; set; }
    }
}
