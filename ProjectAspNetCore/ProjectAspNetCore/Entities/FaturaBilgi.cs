using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectAspNetCore.Entities
{
    [Dapper.Contrib.Extensions.Table("FaturaBilgiler")]
    public class FaturaBilgi : IEquatable<FaturaBilgi>
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Ad { get; set; }
        [MaxLength(250)]
        public string Resim { get; set; }
        public decimal Fiyat { get; set; }
        public bool FaturaOdendiMi { get; set; }
        public int FaturaDonemi { get; set; }
        public string Aciklama { get; set; }
        public List<FaturaBilgiKategori> FaturaBilgiKategoriler { get; set; }
        public AppUser AppUserCustomer { get; set; }
        public int? CustomerId { get; set; }
        public AppUser AppUserCompany { get; set; }
        public int? CompanyId { get; set; }



        public bool Equals([AllowNull] FaturaBilgi other)
        {
            return Id == other.Id && Ad == other.Ad && Resim == other.Resim && Fiyat == other.Fiyat;

        }
    }
}
