using Microsoft.AspNetCore.Mvc;
using ProjectAspNetCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectAspNetCore.ViewComponents
{
    public class KategoriList : ViewComponent
    {
        private readonly IFaturaKategoriRepository _kategoriRepository;
        public KategoriList(IFaturaKategoriRepository kategoriRepository)
        {
            _kategoriRepository = kategoriRepository;
        }
         public IViewComponentResult Invoke()
        {
            return View(_kategoriRepository.GetirHepsi());
        }
    }
}
