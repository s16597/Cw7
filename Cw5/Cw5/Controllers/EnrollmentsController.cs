using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Cw5.Connection;
using Cw5.DTOs.Request;
using Cw5.DTOs.Responses;
using Cw5.Models;
using Cw5.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cw5.Controllers
{

    [ApiController]
    [Route("api/enrollments")]
    [Authorize(Roles = "employee")]
    public class EnrollmentsController : ControllerBase
    {

        private readonly IEnrollmentDbService _enrollmentDbService;

        public EnrollmentsController(IEnrollmentDbService enrollmentDbService)
        {
            _enrollmentDbService = enrollmentDbService;
        }

        [HttpPost]
        [Route("enrollmentStudent")]
        [Authorize(Roles = "employee")]
        public IActionResult EnrollStudent(EnrollStudentRequest enrollStudentRequest)
        {


            return Ok(_enrollmentDbService.EnrollStudent(enrollStudentRequest));


        }

        [HttpPost]
        [Route("promotions")]
        [Authorize(Roles = "employee")]
        public IActionResult PromoteStudents(PromoteStudentRequest request)
        {
            return Ok(_enrollmentDbService.PromoteStudents(request));


        }

     

    }
}