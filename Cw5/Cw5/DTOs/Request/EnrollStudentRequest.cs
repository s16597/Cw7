using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cw5.DTOs.Request
{
    public class EnrollStudentRequest
    {
        [Required(ErrorMessage ="Musisz podać indeks")]
        [MaxLength(100)]
        [RegularExpression("^s[0-9]+$")]
        public string IndexNumber { get; set; }

        [Required(ErrorMessage = "Musisz podać imie")]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Musisz podać nazwisko")]
        [MaxLength(100)]
        public string LastName { get; set; }

        [Required(ErrorMessage ="Musisz podać date urodzenia")]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage ="Musisz podać nazwę studiów")]
        public string Name { get; set; }

        [Required] public string Password { get; set; }
    }
}
