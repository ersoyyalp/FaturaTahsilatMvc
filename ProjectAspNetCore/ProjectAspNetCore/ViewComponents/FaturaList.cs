using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectAspNetCore.Entities;
using ProjectAspNetCore.Enums;
using ProjectAspNetCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectAspNetCore.ViewComponents
{
    public class FaturaList : ViewComponent
    {
        private readonly IFaturaBilgiRepository _faturaRepository;
        private readonly UserManager<AppUser> _userManager;
        public FaturaList(IFaturaBilgiRepository urunRepository, UserManager<AppUser> userManager)
        {
            _faturaRepository = urunRepository;
            _userManager = userManager;
        }
        public IViewComponentResult Invoke(int? kategoriId, int? userId)
        {

            List<FaturaBilgi> faturaBilgis = new List<FaturaBilgi>();
            if (kategoriId.HasValue)
            {
                if (kategoriId < 0)
                    faturaBilgis = _faturaRepository.GetirByStatus(kategoriId.Value);

                else faturaBilgis = _faturaRepository.GetirKategoriIdile((int)kategoriId);

                faturaBilgis = faturaBilgis.Where(x => x.CompanyId == userId).ToList();
            }
            faturaBilgis = _faturaRepository.GetirHepsi().Where(x => x.CustomerId == userId).ToList();

            return View(faturaBilgis);
        }
    }

}
