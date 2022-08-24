using Dapper.Contrib.Extensions;
using Microsoft.Data.SqlClient;
using ProjectAspNetCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectAspNetCore.Repositories
{
    public class DpFaturaRepository
    {
        public List<FaturaBilgi> GetirHepsi()
        {
            using var connection = new SqlConnection("server=LAPTOP-UI4AV1F3;database=FaturaTahsilatDb; integrated security=true;");

            return connection.GetAll<FaturaBilgi>().ToList();

        }
    }
}
