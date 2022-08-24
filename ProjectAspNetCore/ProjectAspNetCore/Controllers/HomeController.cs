using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NLog;
using ProjectAspNetCore.Entities;
using ProjectAspNetCore.Enums;
using ProjectAspNetCore.Interfaces;
using ProjectAspNetCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProjectAspNetCore.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {

        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IFaturaBilgiRepository _faturaRepository;
        private readonly ISepetRepository _sepetRepository;
        private readonly IFaturaKategoriRepository _faturaKategoriRepository;
        public HomeController(IFaturaBilgiRepository faturaRepository, SignInManager<AppUser>
            signInManager, ISepetRepository sepetRepository, IFaturaKategoriRepository faturaKategoriRepository, UserManager<AppUser> userManager)
        {
            _signInManager = signInManager;
            _faturaRepository = faturaRepository;
            _sepetRepository = sepetRepository;
            _faturaKategoriRepository = faturaKategoriRepository;
            _userManager = userManager;
        }
        public IActionResult Index(int? kategoriId, string? kategori)
        {
            string title = "";

            if (kategoriId == (int)FaturaStatusEnum.Ödenmis) title = "Ödenmiş Faturalar Kategorisi";
            else if (kategoriId == (int)FaturaStatusEnum.Ödenmis) title = "Ödenmemiş Faturalar Kategorisi";

            else if (string.IsNullOrEmpty(kategori))
                title = "Tüm Kategoriler";
            else title = kategori + " Kategorisi";

            ViewBag.KategoriId = kategoriId;
            ViewBag.KategoriAd = title;

            var user = _userManager.GetUserAsync(HttpContext.User).Result;


            List<FaturaBilgi> faturaBilgis = new List<FaturaBilgi>();

            if(user.Type == (int)UserTypeEnum.Manager)
            {
                faturaBilgis = _faturaRepository.GetirHepsi();
            }
            else
            {
                if (kategoriId.HasValue)
                {
                    if (kategoriId < 0)
                        faturaBilgis = _faturaRepository.GetirByStatus(kategoriId.Value);

                    else faturaBilgis = _faturaRepository.GetirKategoriIdile((int)kategoriId);

                    faturaBilgis = faturaBilgis.Where(x => x.CustomerId == user.Id).ToList();
                }
                else
                {
                    faturaBilgis = _faturaRepository.GetirHepsi().Where(x => x.CustomerId == user.Id).ToList();
                }

            }

            return View(faturaBilgis);
        }
        public IActionResult FaturaDetay(int id)
        {
            return View(_faturaRepository.GetirIdile(id));
        }

        [AllowAnonymous]
        public IActionResult GirisYap()
        {
            return View(new KullanıcıGirisModel());
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> GirisYap(KullanıcıGirisModel model)
        {
            if (ModelState.IsValid)
            {
                var signInResult = _signInManager.PasswordSignInAsync(model.KullaniciAd, model.Sifre,
                    model.BeniHatirla, false).Result;

                if (signInResult.Succeeded)
                {
                    AppUser appUser = await _userManager.FindByNameAsync(model.KullaniciAd);
                    if (appUser.Type == (int)UserTypeEnum.Customer)
                    {

                        return RedirectToAction("Index");
                    }
                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                }
                ModelState.AddModelError("", "Kullanıcı Adı veya Şifre hatalı");
            }
            return View(model);
        }

        [AllowAnonymous]
        public IActionResult Register()
        {
            ViewBag.CategoryList = _faturaKategoriRepository.GetirHepsi();
            return View(new KayitOlModel());
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(KayitOlModel signInModel)
        {
            if (!String.IsNullOrEmpty(signInModel.Surname)) signInModel.Type = 1;
            else signInModel.Type = 2;

            if (signInModel.Type == 1) signInModel.CategoryId = null;

            await _userManager.CreateAsync(new AppUser
            {
                Name = signInModel.Name,
                SurName = signInModel.Surname,
                UserName = signInModel.Username,
                Email = signInModel.Mail,
                Type = signInModel.Type,
                FaturaKategoriId = signInModel.CategoryId

            }, signInModel.Password); ;

            return RedirectToAction("GirisYap");
        }


        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("GirisYap");
        }


        public IActionResult Sepet()
        {
            List<FaturaBilgi> faturaBilgis = _sepetRepository.GetirSepettekiFaturalar();
            return View(faturaBilgis);
        }
        public IActionResult SepettenCikar(int id)
        {
            var cikarilacakUrun = _faturaRepository.GetirIdile(id);
            _sepetRepository.SepettenCikar(cikarilacakUrun);
            return RedirectToAction("Sepet");
        }
        public IActionResult SepetiBosalt(string faturaIds)
        {
            _sepetRepository.SepetiBosalt();

            List<FaturaBilgi> faturaBilgis = _faturaRepository.GetirByIds(faturaIds.Split(",").Select(Int32.Parse).ToList());

            bool updateResult = _faturaRepository.UpdateFaturalarStatus(faturaBilgis);

            if (updateResult == false)
                return Redirect("/Home");

            return RedirectToAction("Tesekkur", new { fiyat = faturaBilgis.Sum(x => x.Fiyat) });
        }

        public IActionResult Tesekkur(decimal fiyat)
        {
            ViewBag.Fiyat = fiyat;
            return View();
        }

        public IActionResult EkleSepet(int id)
        {
            var fatura = _faturaRepository.GetirIdile(id);

            List<FaturaBilgi> faturaBilgis = _sepetRepository.GetirSepettekiFaturalar();
            if (faturaBilgis != null && faturaBilgis.Any(x => x.Id == fatura.Id))
                TempData["bildirim"] = $"{fatura.Ad} adlı fatura zaten sepete eklenmiş";
            else
            {
                _sepetRepository.SepetEkle(fatura);
                TempData["bildirim"] = "Faturam sepete eklendi";
            }
            return RedirectToAction("Index");
        }

        public IActionResult NotFound(int code)
        {
            ViewBag.Code = code;
            return View();

        }
        [Route("/Error")]
        public IActionResult Error()
        {
           
            return View();
        }
    }

}
