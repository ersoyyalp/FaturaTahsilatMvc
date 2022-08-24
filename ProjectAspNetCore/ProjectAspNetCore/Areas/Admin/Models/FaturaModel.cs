using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectAspNetCore.Areas.Admin.Models
{
    public class FaturaModel
    {
        public int FaturaId { get; set; }
        public string FaturaDonemi { get; set; }
        public decimal FaturaTutar { get; set; }
        public bool FaturaOdendiMi { get; set; }
        public string FaturaAciklama { get; set; }



    }
}
