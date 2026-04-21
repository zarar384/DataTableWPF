using DataTableWPF.Models;
using System;
using System.Configuration;
using System.Data.Entity;

namespace DataTableWPF.Data
{
    internal class AppDbContext : DbContext
    {
        public DbSet<Person> Persons { get; set; } = null!;

        public AppDbContext() : base(GetConnectionString())
        {
        }

        private static string GetConnectionString()
        {
            var mode = ConfigurationManager.AppSettings["DbMode"] ?? "LocalDb";

            var conn = ConfigurationManager.ConnectionStrings[mode];

            if (conn == null)
                throw new Exception($"Connection string '{mode}' not found.");

            return conn.ConnectionString;
        }
    }
}