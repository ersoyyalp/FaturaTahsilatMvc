using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using ProjectAspNetCore.Contexts;
using ProjectAspNetCore.Entities;
using ProjectAspNetCore.Enums;
using ProjectAspNetCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectAspNetCore.Repositories
{
    public class FaturaBilgiRepository : GenericRepository<FaturaBilgi>, IFaturaBilgiRepository
    {
        private readonly IFaturaBilgiKategoriRepository _faturabilgikategoriRepository;
        public FaturaBilgiRepository(IFaturaBilgiKategoriRepository faturabilgikategoriRepository)
        {
            _faturabilgikategoriRepository = faturabilgikategoriRepository;
        }

        public List<FaturaKategori> GetirKategoriler(int faturaId)
        {
            using var context = new ProjectContext();
            return context.FaturaBilgiler.Join(context.FaturaBilgiKategoriler, fatura => fatura.Id, faturaKategori => faturaKategori.FaturaId,
                (f, fk) => new
                {
                    fatura = f,
                    faturaKategori = fk

                }).Join(context.FaturaKategori, iki => iki.faturaKategori.KategoriId, kategori => kategori.Id, (fk, k) => new
                {
                    fatura = fk.fatura,
                    kategori = k,
                    FaturaKategori = fk.faturaKategori

                }).Where(I => I.fatura.Id == faturaId).Select(I => new FaturaKategori
                {
                    Ad = I.kategori.Ad,
                    Id = I.kategori.Id
                }).ToList();
        }

        public void SilKategori(FaturaBilgiKategori faturaKategori)
        {
            var kontrolKayit = _faturabilgikategoriRepository.GetirFiltreile(I => I.KategoriId == faturaKategori.KategoriId && I.FaturaId == faturaKategori.FaturaId);
            if (kontrolKayit != null)
            {
                _faturabilgikategoriRepository.Sil(kontrolKayit);
            }

        }
        public void EkleKategori(FaturaBilgiKategori faturaKategori)
        {
            var kontrolKayit = _faturabilgikategoriRepository.GetirFiltreile(I => I.KategoriId == faturaKategori.KategoriId && I.FaturaId == faturaKategori.FaturaId);
            if (kontrolKayit == null)
            {
                _faturabilgikategoriRepository.Ekle(faturaKategori);
            }
        }

        public List<FaturaBilgi> GetirKategoriIdile(int kategoriId)
        {
            using var context = new ProjectContext();

            return context.FaturaBilgiler.Join(context.FaturaBilgiKategoriler, f => f.Id, fk => fk.FaturaId,
                (fatura, faturaKategori) => new
                {
                    Fatura = fatura,
                    FaturaKategori = faturaKategori


                }).Where(I => I.FaturaKategori.KategoriId == kategoriId).Select(I => new FaturaBilgi
                {
                    Id = I.Fatura.Id,
                    Ad = I.Fatura.Ad,
                    Fiyat = I.Fatura.Fiyat,
                    Resim = I.Fatura.Resim,
                    FaturaOdendiMi = I.Fatura.FaturaOdendiMi,
                    CompanyId = I.Fatura.CompanyId,
                    CustomerId = I.Fatura.CustomerId

                }).ToList();
        }


        public List<FaturaBilgi> GetirByIds(List<int> ids)
        {
            using ProjectContext context = new ProjectContext();

            List<FaturaBilgi> datas = context.FaturaBilgiler.Where(x => ids.Contains(x.Id)).ToList();

            return datas;
        }

        public bool UpdateFaturalarStatus(List<FaturaBilgi> faturaBilgis)
        {

            using ProjectContext context = new ProjectContext();

            try
            {
                faturaBilgis.ForEach(f => f.FaturaOdendiMi = true);

                context.FaturaBilgiler.UpdateRange(faturaBilgis);

                context.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public List<FaturaBilgi> GetirByStatus(int status)
        {
            using ProjectContext context = new ProjectContext();

            List<FaturaBilgi> faturaBilgis = new List<FaturaBilgi>();
            if (status == (int)FaturaStatusEnum.Ödenmis)
                faturaBilgis = context.FaturaBilgiler.Where(x => x.FaturaOdendiMi == true).ToList();

            else if (status == (int)FaturaStatusEnum.Ödenmemis)
                faturaBilgis = context.FaturaBilgiler.Where(x => x.FaturaOdendiMi == false).ToList();

            return faturaBilgis;

        }
    }
}