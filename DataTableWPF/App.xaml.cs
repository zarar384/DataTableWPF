using DataTableWPF.Data;
using System;
using System.Data.Entity;
using System.Windows;

namespace DataTableWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // auto-migration: create and update the database schema based on the model
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<AppDbContext>());

            // force the database to be created and initialized on application startup
            using (var db = new AppDbContext())
            {
                db.Database.Initialize(false);
            }
        }
    }
}
