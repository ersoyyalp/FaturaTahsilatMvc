using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectAspNetCore.Contexts;
using ProjectAspNetCore.Entities;
using ProjectAspNetCore.Enums;
using ProjectAspNetCore.Interfaces;
using ProjectAspNetCore.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectAspNetCore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IFaturaBilgiRepository _faturabilgiRepository;
        private readonly IFaturaKategoriRepository _faturakategoriRepository;
        private readonly UserManager<AppUser> _userManager;
        public HomeController(IFaturaBilgiRepository faturabilgiRepository, IFaturaKategoriRepository faturakategoriRepository, UserManager<AppUser> userManager)
        {
            _faturabilgiRepository = faturabilgiRepository;
            _faturakategoriRepository = faturakategoriRepository;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index(int? status)
        {
            var user = _userManager.GetUserAsync(HttpContext.User).Result;
            if (user.Type == (int)UserTypeEnum.Customer)
                return Redirect("/");


            IQueryable<FaturaBilgi> faturaBilgis = _faturabilgiRepository.GetTablo();

            if (status.HasValue)
            {
                if (status == (int)FaturaStatusEnum.Ödenmis)
                    faturaBilgis = faturaBilgis.Where(x => x.FaturaOdendiMi == true);

                else if (status == (int)FaturaStatusEnum.Ödenmemis)
                    faturaBilgis = faturaBilgis.Where(x => x.FaturaOdendiMi == false);
            }

            if (user.Type == (int)UserTypeEnum.Company)
            {
                faturaBilgis = faturaBilgis.Where(x => x.AppUserCompany.Id == user.Id);
            }

            ViewBag.Status = status;

            var data = faturaBilgis.ToList();
            foreach (var item in data)
            {
                item.AppUserCustomer = await _userManager.FindByIdAsync(item.CustomerId.ToString());
            }
            return View(data);
        }
        //Eklemenin asarımını yaptık
        public async Task<IActionResult> Ekle()
        { 
            FaturaEkleModel model = new FaturaEkleModel();
            var user = await _userManager.GetUserAsync(HttpContext.User);
            int userType = user.Type;
            //Kullanıcıları eklemek istedıgımızde listeleyen metod
            ViewBag.UserType = userType;
            if (userType == (int)UserTypeEnum.Company)
            {
                using ProjectContext project = new ProjectContext();
                ViewBag.Customers = project.Users.Where(x => x.Type == (int)UserTypeEnum.Customer).ToList();
                model.CompanyId = user.Id;
                model.CategoryId = (int)user.FaturaKategoriId;
            }
            else if (userType == (int)UserTypeEnum.Manager)
            {
                using ProjectContext project = new ProjectContext();
                ViewBag.Customers2 = project.Users.Where(x => x.Type == (int)UserTypeEnum.Customer).ToList();
                ViewBag.Companies = project.Users.Where(x => x.Type == (int)UserTypeEnum.Company).ToList();
            }
            return View(model);
        }
        //Eklemeyi gerçekleştir
        [HttpPost]
        public async Task<IActionResult> Ekle(FaturaEkleModel model)//FaturaEkleModelden bir model alır
        {   //istenilen şekilde girilmiş mi kontrol eder
            if (ModelState.IsValid)
            {

                if (model.CompanyId <= 0 || model.CompanyId == null)
                {
                    var user = await _userManager.GetUserAsync(HttpContext.User);
                    model.CompanyId = user.Id;
                }

                FaturaBilgi fatura = new FaturaBilgi();
                if (model.Resim != null)
                {
                    var uzanti = Path.GetExtension(model.Resim.FileName);
                    var yeniResimAd = Guid.NewGuid() + uzanti;
                    var yuklenecekYer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/" + yeniResimAd);



                    var stream = new FileStream(yuklenecekYer, FileMode.Create);
                    model.Resim.CopyTo(stream);

                    fatura.Resim = yeniResimAd;
                }

                fatura.Ad = model.Ad;
                fatura.Fiyat = model.Fiyat;
                fatura.CompanyId = model.CompanyId;
                fatura.CustomerId = model.UserId;
                fatura.Aciklama = model.Aciklama;

                _faturabilgiRepository.Ekle(fatura);

                int? categoryId = model.CategoryId;

                using ProjectContext project = new ProjectContext();


                if (categoryId <= 0)                                            //İlk değeri döndür. eğer veri yoksa default döndür
                    categoryId = project.Users.Where(x => x.Id == model.CompanyId).FirstOrDefault().FaturaKategoriId;
                project.FaturaBilgiKategoriler.Add(new FaturaBilgiKategori { FaturaId = fatura.Id, KategoriId = (int)categoryId });
                project.SaveChanges();

                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }
            return View(model);
        }
        public IActionResult Guncelle(int id)
        {
            var gelenFatura = _faturabilgiRepository.GetirIdile(id);
            FaturaGuncelleModel model = new FaturaGuncelleModel
            {
                Ad = gelenFatura.Ad,
                Fiyat = gelenFatura.Fiyat,
                Id = gelenFatura.Id,
                Aciklama = gelenFatura.Aciklama

            };
            return View(model);
        }
        [HttpPost]
        public IActionResult Guncelle(FaturaGuncelleModel model)
        {
            if (ModelState.IsValid)
            {
                var guncellenecekFatura = _faturabilgiRepository.GetirIdile(model.Id);
                if (model.Resim != null)
                {
                    var uzanti = Path.GetExtension(model.Resim.FileName);
                    var yeniResimAd = Guid.NewGuid() + uzanti;
                    var yuklenecekYer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/" + yeniResimAd);



                    var stream = new FileStream(yuklenecekYer, FileMode.Create);
                    model.Resim.CopyTo(stream);

                    guncellenecekFatura.Resim = yeniResimAd;
                }
                guncellenecekFatura.Ad = model.Ad;
                guncellenecekFatura.Fiyat = model.Fiyat;
                guncellenecekFatura.Aciklama = model.Aciklama;
                _faturabilgiRepository.Guncelle(guncellenecekFatura);

                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }
            return View(model);
        }

        public IActionResult Sil(int id)
        {
            _faturabilgiRepository.Sil(new FaturaBilgi { Id = id });
            return RedirectToAction("Index");
        }
        public IActionResult AtaKategori(int id)
        {
            var faturayaAitKategoriler = _faturabilgiRepository.GetirKategoriler(id).Select(I => I.Ad);
            var faturaKategoriler = _faturakategoriRepository.GetirHepsi();

            TempData["FaturaId"] = id;

            List<KategoriAtaModel> list = new List<KategoriAtaModel>();

            foreach (var item in faturaKategoriler)
            {
                KategoriAtaModel model = new KategoriAtaModel();
                model.KategoriId = item.Id;
                model.KategoriAd = item.Ad;
                model.Varmi = faturayaAitKategoriler.Contains(item.Ad);

                list.Add(model);

            }
            return View(list);
        }
        [HttpPost]
        public IActionResult AtaKategori(List<KategoriAtaModel> list)
        {
            int faturaId = (int)TempData["FaturaId"];
            foreach (var item in list)
            {
                if (item.Varmi)
                {
                    _faturabilgiRepository.EkleKategori(new FaturaBilgiKategori
                    {
                        KategoriId = item.KategoriId,
                        FaturaId = faturaId
                    });
                }
                else
                {
                    _faturabilgiRepository.SilKategori(new FaturaBilgiKategori
                    {
                        KategoriId = item.KategoriId,
                        FaturaId = faturaId
                    });
                }
            }
            return RedirectToAction("Index");
        }
    }
}
