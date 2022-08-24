using ProjectAspNetCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectAspNetCore.Interfaces
{
    public interface IFaturaBilgiRepository : IGenericRepository<FaturaBilgi>
    {
        List<FaturaKategori> GetirKategoriler(int faturaId);
        void EkleKategori(FaturaBilgiKategori faturaBilgiKategori);
        void SilKategori(FaturaBilgiKategori faturaBilgiKategori);
        List<FaturaBilgi> GetirKategoriIdile(int kategoriId);

        List<FaturaBilgi> GetirByIds(List<int> ids);

        bool UpdateFaturalarStatus(List<FaturaBilgi> faturaBilgis);

        List<FaturaBilgi> GetirByStatus(int status);

    }
}
