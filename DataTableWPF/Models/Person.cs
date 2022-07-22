using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTableWPF.Models
{
    //I don't know why EF migration changes Person model name to People. 
    public class Person
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Jmeno")]
        public string Name { get; set; }
        [Display(Name = "Příjmení")]
        public string Surname { get; set; }
        //without 420
        [Display(Name = "Telefonní číslo")]
        public int Phone { get; set; }
        [Display(Name = "E-Mail")]
        public string Email { get; set; }
    }
}
