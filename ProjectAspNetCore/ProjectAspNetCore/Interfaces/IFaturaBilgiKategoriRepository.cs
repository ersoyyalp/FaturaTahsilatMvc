using ProjectAspNetCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ProjectAspNetCore.Interfaces
{
    public interface IFaturaBilgiKategoriRepository: IGenericRepository<FaturaBilgiKategori>
    {
        FaturaBilgiKategori GetirFiltreile(Expression<Func<FaturaBilgiKategori, bool>> filter);
        //void Ekle(FaturaBilgiKategoriler urunKategori);
    }
}
