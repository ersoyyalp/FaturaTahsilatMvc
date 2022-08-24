using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectAspNetCore.Entities;
using ProjectAspNetCore.Interfaces;
using ProjectAspNetCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectAspNetCore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class KategoriController : Controller
    {

        private readonly IFaturaKategoriRepository _faturakategoriRepository;
        public KategoriController(IFaturaKategoriRepository faturakategoriRepository)
        {
            _faturakategoriRepository = faturakategoriRepository;
        }
        public IActionResult Index()
        {
            return View(_faturakategoriRepository.GetirHepsi());
        }
        public IActionResult Ekle()
        {
            return View(new KategoriEkleModel());
        }
        [HttpPost]
        public IActionResult Ekle(KategoriEkleModel model)
        {
            if (ModelState.IsValid)
            {
                _faturakategoriRepository.Ekle(new FaturaKategori
                {
                    Ad = model.Ad
                });
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public IActionResult Guncelle(int id)
        {
            var guncellenecekKategori = _faturakategoriRepository.GetirIdile(id);
            KategoriGuncelleModel model = new KategoriGuncelleModel
            {
                Id = guncellenecekKategori.Id,
                Ad = guncellenecekKategori.Ad

            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Guncelle(KategoriGuncelleModel model)
        {
            if (ModelState.IsValid)
            {
                var guncellenecekKategori = _faturakategoriRepository.GetirIdile(model.Id);
                guncellenecekKategori.Ad = model.Ad;

                _faturakategoriRepository.Guncelle(guncellenecekKategori);

                return RedirectToAction("Index");
            }

            return View(model);
        }
        public IActionResult Sil(int id)
        {
            _faturakategoriRepository.Sil(new FaturaKategori { Id = id });
            return RedirectToAction("Index");
        }

    }
}
