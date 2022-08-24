using ProjectAspNetCore.Contexts;
using ProjectAspNetCore.Entities;
using ProjectAspNetCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ProjectAspNetCore.Repositories
{
    public class FaturaBilgiKategoriRepository : GenericRepository<FaturaBilgiKategori>, IFaturaBilgiKategoriRepository
    {
        public FaturaBilgiKategori GetirFiltreile(Expression<Func<FaturaBilgiKategori, bool>> filter)
        {
            using var context = new ProjectContext();

            return context.FaturaBilgiKategoriler.FirstOrDefault(filter);
        }

        
    }
}
