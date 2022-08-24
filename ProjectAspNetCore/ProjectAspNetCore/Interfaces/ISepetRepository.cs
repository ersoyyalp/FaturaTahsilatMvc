using ProjectAspNetCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectAspNetCore.Interfaces
{
    public interface ISepetRepository
    {
        void SepetEkle(FaturaBilgi urun);
        void SepettenCikar(FaturaBilgi urun);
        List<FaturaBilgi> GetirSepettekiFaturalar();
        void SepetiBosalt();

    }
}
