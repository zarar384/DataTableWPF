using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataTableWPF.Models
{
    [Table("Person")]
    public class Person : INotifyPropertyChanged, IDataErrorInfo
    {
        [Key]
        public int Id { get; set; }

        private string _name;
        [Required]
        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(nameof(Name)); }
        }

        private string _surname;
        public string Surname
        {
            get => _surname;
            set { _surname = value; OnPropertyChanged(nameof(Surname)); }
        }

        private string _phone;
        public string Phone
        {
            get => _phone;
            set { _phone = value; OnPropertyChanged(nameof(Phone)); }
        }

        private string _email;
        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(nameof(Email)); }
        }

        // Validation
        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                if (columnName == nameof(Name))
                {
                    if (string.IsNullOrWhiteSpace(Name))
                        return "Name is required";
                }

                if (columnName == nameof(Surname))
                {
                    if (string.IsNullOrWhiteSpace(Surname))
                        return "Surname is required";
                }

                if (columnName == nameof(Phone))
                {
                    if (string.IsNullOrWhiteSpace(Phone))
                        return "Phone is required";

                    if (int.TryParse(Phone, out var phoneNumber) && phoneNumber != 0)
                    {
                        if (phoneNumber < 0)
                            return "Phone must be a positive number";
                        else if (Phone.Length < 7 || Phone.Length > 15)
                            return "Phone must be between 7 and 15 digits";
                    }
                    else
                    {
                        return "Phone must be a number";
                    }
                }

                return null;
            }
        }

        // Notify property changes
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
