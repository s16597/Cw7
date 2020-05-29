using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Cw5.DAL;
using Cw5.Models;
using Cw5.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Cw5.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {
        private readonly IDbService _dbService;
        private const string ConString = "Data Source=db-mssql;Initial Catalog=s16597;Integrated Security=True";
        private IJWTAuthorizationService _JWTauthService;

        public StudentsController(IDbService dbService, IConfiguration configuration, IJWTAuthorizationService JWTauthService)
        {
            _dbService = dbService;
            _JWTauthService = JWTauthService;
        }

        [HttpGet("getStudents")]

        public IActionResult GetListStudent(string orderBy)
        {

            return Ok(_dbService.getStudents());

        }

        [HttpGet]
        public IActionResult GetStudents(string orderBy)
        {

            var list = new List<Student>();
            using (System.Data.SqlClient.SqlConnection con = new SqlConnection(ConString))
            using (System.Data.SqlClient.SqlCommand com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "SELECT IndexNumber, FirstName, LastName, BirthDate, st.IdEnrollment, Name, Semester FROM Student st JOIN ENROLLMENT enr ON st.IdEnrollment = enr.IdEnrollment JOIN Studies stud on enr.IdStudy = stud.IdStudy";

                con.Open();
                SqlDataReader dr = com.ExecuteReader();
                while (dr.Read())
                {

                    var st = new Student();
                    st.IndexNumber = dr["IndexNumber"].ToString();
                    st.FirstName = dr["FirstName"].ToString();
                    st.LastName = dr["LastName"].ToString();
                    st.BirthDate = Convert.ToDateTime(dr["BirthDate"].ToString());
                    st.IdEnrollment = Convert.ToInt32(dr["IdEnrollment"].ToString());
                    st.Name = dr["Name"].ToString();
                    st.Semester = Convert.ToInt32(dr["Semester"].ToString());



                    list.Add(st);

                }

            }

            return Ok(list);
        }

        [HttpGet("{indexNumber}")]
        public IActionResult GetStudent(string indexNumber)
        {
            using (SqlConnection con = new SqlConnection(ConString))
            using (SqlCommand com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "select * from Student where indexnumber=@index";
                com.Parameters.AddWithValue("index", indexNumber);

                con.Open();
                SqlDataReader dr = com.ExecuteReader();
                if (dr.Read())
                {
                    var st = new Student();
                    st.IndexNumber = dr["indexNumber"].ToString();
                    st.FirstName = dr["FirstName"].ToString();
                    st.LastName = dr["LastName"].ToString();
                    st.BirthDate = Convert.ToDateTime(dr["BirthDate"].ToString());
                    st.IdEnrollment = Convert.ToInt32(dr["IdEnrollment"].ToString());


                    return Ok(st);

                }


            }

            return NotFound();
        }

        [HttpGet("joinTables")]
        public IActionResult GetStudentsJoinTables()
        {

            var list2 = new List<Combo>();
            using (SqlConnection con = new SqlConnection(ConString))
            using (SqlCommand com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "select FirstName, LastName, BirthDate, Name, Semester From Student st LEFT JOIN Enrollment enr ON st.IdEnrollment = enr.IdEnrollment LEFT JOIN Studies stu ON enr.IdStudy = stu.IdStudy";

                con.Open();
                SqlDataReader dr = com.ExecuteReader();
                while (dr.Read())
                {
                    var combo = new Combo();

                    combo.FirstName = dr["FirstName"].ToString();
                    combo.LastName = dr["LastName"].ToString();
                    combo.BirthDate = Convert.ToDateTime(dr["BirthDate"].ToString());
                    combo.Name = dr["Name"].ToString();
                    combo.Semester = Convert.ToInt32(dr["Semester"].ToString());

                    list2.Add(combo);

                }


            }

            return Ok(list2);
        }

        [HttpPost]
        public IActionResult CreateStudent(Student student)
        {
            student.IndexNumber = $"s{new Random().Next(1, 99999)}";
            return Ok(student);
        }

        [HttpPut("{id}")]
        public IActionResult PutStudent(int id, Student student)
        {
            return Ok("Aktualizacja studenta nr " + id + " dokończona.");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(int id)
        {
            return Ok("Usuwanie studenta nr " + id + " ukończone.");
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        public IActionResult Login(LoginRequest request)
        {
            var token = _JWTauthService.Login(request);
            if (token != null)
            {
                return Ok(token);
            }
            else
            {
                return Unauthorized("Wrong login or password entered");
            }
        }
        [HttpPost]
        [AllowAnonymous]
        [Route("refreshToken")]
        public IActionResult RefreshToken(RefreshRequest request)
        {
            var token = _JWTauthService.RefreshToken(request);
            if (token != null)
            {
                return Ok(token);
            }
            else
            {
                return NotFound("No token like that found in database");
            }
        }

    }
}