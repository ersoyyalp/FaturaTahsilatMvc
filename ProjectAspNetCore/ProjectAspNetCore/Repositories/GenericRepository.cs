using ProjectAspNetCore.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectAspNetCore.Repositories
{
    public class GenericRepository<Tablo> where Tablo:class,new()
    {

        public IQueryable<Tablo> GetTablo()
        {
            var context = new ProjectContext();
            return context.Set<Tablo>();
        }

        public void Ekle(Tablo tablo)
        {
            using var context = new ProjectContext();

            context.Set<Tablo>().Add(tablo);
            context.SaveChanges();
        }
        public void Guncelle(Tablo tablo)
        {
            using var context = new ProjectContext();

            context.Set<Tablo>().Update(tablo);
            context.SaveChanges();
        }
        public void Sil(Tablo tablo)
        {
            using var context = new ProjectContext();

            context.Set<Tablo>().Remove(tablo);
            context.SaveChanges();
        }
        public List<Tablo> GetirHepsi()
        {
            using var context = new ProjectContext();

            return context.Set<Tablo>().OrderByDescending(I => I).ToList();
        }
        public Tablo GetirIdile(int id)
        {
            using var context = new ProjectContext();
            return context.Set<Tablo>().Find(id);
        }
    }
}
