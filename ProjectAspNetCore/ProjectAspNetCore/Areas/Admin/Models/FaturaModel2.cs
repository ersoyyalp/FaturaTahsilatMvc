using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectAspNetCore.Areas.Admin.Models
{
    public class FaturaModel2
    {
        public int FatId { get; set; }
        public int FatSahibi { get; set; }
        public string FatDonemi { get; set; }
        public decimal FatTutar { get; set; }
        public bool FatOdendiMi { get; set; }
        public string FatAciklama { get; set; }
    }
}
