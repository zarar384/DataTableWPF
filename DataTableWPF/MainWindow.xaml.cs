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
using System.Text.RegularExpressions;
using System.Data.Entity.Validation;

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
            personDataGrid.ItemsSource = db.Persons.Local.ToBindingList();

            this.Closing += MainWindow_Closing;
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            db.Dispose();
        }
        //Table Tab
        private void addBtnTbl_Click(object sender, RoutedEventArgs e)
        {
            if (personDataGrid.IsVisible)
            {
                for (int i = 0; i < personDataGrid.SelectedItems.Count; i++)
                {
                    Person person = personDataGrid.SelectedItems[i] as Person;
                    if (person != null)
                    {
                        db.Persons.Add(person);
                    }
                }
            }
            db.SaveChanges();
        }

        private int updPersonId = 0;
        private void updBtnTbl_Click(object sender, RoutedEventArgs e)
        {
            personTabControl.SelectedValue = personTab;

            for (int i = 0; i < personDataGrid.SelectedItems.Count; i++)
            {
                Person person = personDataGrid.SelectedItems[i] as Person;
                if (person != null)
                {
                    add_personNameTextBox.Text = person.Name;
                    add_personSurnameTextBox.Text = person.Surname;
                    add_personMailTextBox.Text = person.Email;
                    add_personPhoneTextBox.Text = person.Phone.ToString();

                    this.updPersonId = person.Id;
                }
            }
        }

        private void delBtnTbl_Click(object sender, RoutedEventArgs e)
        {
            if (personDataGrid.SelectedItems.Count > 0)
            {
                for (int i = 0; i < personDataGrid.SelectedItems.Count; i++)
                {
                    Person person = personDataGrid.SelectedItems[i] as Person;
                    if (person != null)
                    {
                        db.Persons.Remove(person);
                    }
                }
            }
        }
        //Person Tab
        private void addBtnPrsn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (personGrid.IsVisible)
                {
                    Person person = new Person
                    {
                        Id = updPersonId,
                        Name = add_personNameTextBox.Text,
                        Surname = add_personSurnameTextBox.Text,
                        Phone = int.Parse(add_personPhoneTextBox.Text),
                        Email = add_personMailTextBox.Text,
                    };
                    if (person.Id == 0)
                    {
                        db.Persons.Add(person);
                    }
                    else if (person.Id > 0)
                    {
                        var updId = from p in db.Persons
                                    where p.Id == this.updPersonId
                                    select p;
                        person = updId.SingleOrDefault();
                        if (person != null)
                        {
                            person.Name = this.add_personNameTextBox.Text;
                            person.Surname = this.add_personSurnameTextBox.Text;
                            person.Phone = int.Parse(this.add_personPhoneTextBox.Text);
                            person.Email = this.add_personMailTextBox.Text;
                        }
                        db.SaveChanges();
                        personDataGrid.Items.Refresh();
                        this.updPersonId = 0;
                    }
                    else
                    {
                        MessageBox.Show("Person.ID Error");
                    }

                    db.SaveChanges();
                    add_personNameTextBox.Text = "";
                    add_personSurnameTextBox.Text = "";
                    add_personMailTextBox.Text = "";
                    add_personPhoneTextBox.Text = "";
                    personTabControl.SelectedValue = dbTableTab;
                }
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var eve in ex.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw ex;
            }
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}

