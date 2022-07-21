using DataTableWPF.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTableWPF.Data
{
    internal class AppDbContext: DbContext
    {
        public AppDbContext() : base("ConnectionStrings:AppDbContext")
        {

        }

        public DbSet<Person> Persons { get; set; }
    }
}
