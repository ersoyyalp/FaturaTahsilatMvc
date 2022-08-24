using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectAspNetCore.Entities
{
    public class FaturaBilgiKategori
    {
        public int Id { get; set; }

        public int FaturaId { get; set; }
        public FaturaBilgi FaturaBilgi { get; set; }
        public int KategoriId { get; set; }
        public FaturaKategori FaturaKategori { get; set; }
    }
}
