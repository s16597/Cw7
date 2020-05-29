using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cw5.DTOs.Responses
{
    public class EnrollStudentResponse
    {
   
        public int IdEnrollment { get; set; }
        public int IdStudy { get; set; }
   

        public string LastName { get; set; }
        public int Semester { get; set; }
        public DateTime StartDate { get; set; }
        public string Name { get; set; }

    }
}
