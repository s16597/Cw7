using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cw5.DTOs.Request
{
    public class PromoteStudentRequest
    {
        [Required(ErrorMessage = "Podaj nazwe studiów")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Podaj numer semestru")]
        public int Semester { get; set; }
    }
}
