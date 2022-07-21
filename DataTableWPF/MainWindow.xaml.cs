using DataTableWPF.Data;
using DataTableWPF.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DataTableWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        AppDbContext db;
        public MainWindow()
        {
            InitializeComponent();

            db = new AppDbContext();
            db.Persons.Load();
            personGrin.ItemsSource = db.Persons.Local.ToBindingList();

            this.Closing += MainWindow_Closing;
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            db.Dispose();
        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            if (personGrin.IsVisible)
            {
                for (int i = 0; i < personGrin.SelectedItems.Count; i++)
                {
                    Person person = personGrin.SelectedItems[i] as Person;
                    if (person != null)
                    {
                        db.Persons.Add(person);
                    }
                }
            }
            db.SaveChanges();
        }

        private void updBtn_Click(object sender, RoutedEventArgs e)
        {
            db.SaveChanges();
        }

        private void delBtn_Click(object sender, RoutedEventArgs e)
        {
            if (personGrin.SelectedItems.Count > 0)
            {
                for (int i = 0; i < personGrin.SelectedItems.Count; i++)
                {
                    Person person = personGrin.SelectedItems[i] as Person;
                    if (person != null)
                    {
                        db.Persons.Remove(person);
                    }
                }
            }
            db.SaveChanges();
        }
    }
}

