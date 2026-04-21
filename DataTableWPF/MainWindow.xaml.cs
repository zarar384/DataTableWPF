using DataTableWPF.Data;
using DataTableWPF.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DataTableWPF
{
    public partial class MainWindow : Window
    {
        private readonly AppDbContext db;
        private Person selectedPerson;

        public MainWindow()
        {
            InitializeComponent();

            db = new AppDbContext();

            db.Persons.Load();
            personDataGrid.ItemsSource = db.Persons.Local;

            this.Closing += MainWindow_Closing;
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            db.Dispose();
        }

        //Table Tab
        private void AddBtnTbl_Click(object sender, RoutedEventArgs e)
        {
            selectedPerson = new Person(); 
            personGrid.DataContext = selectedPerson;

            personTabControl.SelectedItem = personTab;
        }

        private void UpdBtnTbl_Click(object sender, RoutedEventArgs e)
        {
            if (personDataGrid.SelectedItem is not Person person)
                return;

            selectedPerson = person;                    
            personGrid.DataContext = selectedPerson;     

            personTabControl.SelectedItem = personTab;   
        }

        private void DelBtnTbl_Click(object sender, RoutedEventArgs e)
        {
            var selected = personDataGrid.SelectedItems
                .OfType<Person>()
                .ToList();

            if (selected.Count == 0)
            {
                MessageBox.Show("No items selected");
                return;
            }

            var result = MessageBox.Show("Delete selected records?", "Confirm",
                MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes)
                return;

            db.Persons.RemoveRange(selected);
            db.SaveChanges();

            personDataGrid.SelectedItems.Clear();
        }

        //Person Tab
        private void AddBtnPrsn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (selectedPerson == null)
                {
                    MessageBox.Show("No data to save");
                    return;
                }

                if (!IsValide())
                    return;

                if (selectedPerson.Id == 0)
                {
                    db.Persons.Add(selectedPerson);
                }

                db.SaveChanges();

                ClearForm();

                personTabControl.SelectedItem = dbTableTab;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }    

        #region helpers
        private void ClearForm()
        {
            personGrid.DataContext = null;
            selectedPerson = null;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private bool IsValide()
        {
            if (selectedPerson == null)
            {
                MessageBox.Show("No data");
                return false;
            }

            var errors = typeof(Person)
                .GetProperties()
                .Select(p => selectedPerson[p.Name])
                .Where(e => !string.IsNullOrEmpty(e))
                .ToList();

            if (errors.Any())
            {
                MessageBox.Show(errors.Any() ? string.Join("\n", errors) : null);
                return false;
            }

            return true;
        }
        #endregion      
    }
}