using Cw5.DTOs.Request;
using Cw5.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cw5.Services
{
    public interface IEnrollmentDbService
    {
        public EnrollResponse EnrollStudent (EnrollStudentRequest request);
        public PromoteStudentResponse PromoteStudents(PromoteStudentRequest request);

        public Boolean Validate(String index);



    }
}
