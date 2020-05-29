using Cw5.Connection;
using Cw5.DTOs.Request;
using Cw5.DTOs.Responses;
using Cw5.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;


namespace Cw5.Services
{
    public class EnrollmentDbService : IEnrollmentDbService
    {
    
        public EnrollResponse EnrollStudent(EnrollStudentRequest request)
        {
            using var con = new SqlConnection(SetConnection.GetConnection());
            using var com = new SqlCommand();
           
            con.Open();
            com.Connection = con;
            var transaction = con.BeginTransaction();
            com.Transaction = transaction;

            com.CommandText = "SELECT s.IdStudy FROM Studies s WHERE s.Name = @Name";
            com.Parameters.AddWithValue("Name", request.Name);
            var dr = com.ExecuteReader();
            if (!dr.Read())
            {
                throw new Exception("Podane studia nie istnieją "); ;
            }
            var idStudy = int.Parse(dr["IdStudy"].ToString());

            dr.Close();
            com.Parameters.Clear();
            com.CommandText =
                "SELECT * FROM Enrollment enr WHERE enr.Semester = 1 AND enr.IdStudy = @IdStudy";
            com.Parameters.AddWithValue("IdStudy", idStudy);
            dr = com.ExecuteReader();

            var enrollResponse = new EnrollResponse();
            if (!dr.Read())
            {
                dr.Close();
                com.Parameters.Clear();

                com.CommandText =
                @"INSERT INTO Enrollment(IdEnrollment, Semester, StartDate, IdStudy) OUTPUT INSERTED.IdEnrollment, INSERTED.Semester, INSERTED.StartDate, INSERTED.IdStudy VALUES((SELECT MAX(E.IdEnrollment) FROM Enrollment E) + 1, @Semester, @StartDate, @IdStudy);";
                com.Parameters.AddWithValue("Semester", 1);
                com.Parameters.AddWithValue("StartDate", DateTime.Now);
                com.Parameters.AddWithValue("IdStudy", idStudy);

                enrollResponse.IdEnrollment = int.Parse(com.ExecuteScalar().ToString());
                enrollResponse.Semester =int. Parse(com.Parameters["Semester"].Value.ToString());
                enrollResponse.IdStudy = int.Parse(com.Parameters["IdStudy"].Value.ToString());
                enrollResponse.StartDate =
                    DateTime.Parse(com.Parameters["StartDate"].Value.ToString()).ToString("yyyy-MM-dd");
            }
            else
            {
                enrollResponse.IdEnrollment =int. Parse(dr["IdEnrollment"].ToString());
                enrollResponse.Semester = int.Parse(dr["Semester"].ToString());
                enrollResponse.IdStudy = int.Parse(dr["IdStudy"].ToString());
                enrollResponse.StartDate =
                    DateTime.Parse(dr["StartDate"].ToString()).ToString("yyyy-MM-dd");
            }

            dr.Close();
            com.Parameters.Clear();
            com.CommandText = "SELECT S.IndexNumber FROM Student S WHERE IndexNumber = @indexNumber";
            com.Parameters.AddWithValue("indexNumber", request.IndexNumber);
            dr = com.ExecuteReader();
            if (dr.Read())
            {
                throw new Exception("Numer indeksu nie jest unikalny"); ;
            }

            dr.Close();
            var studentsalt = GetSalt(32);
            com.Parameters.Clear();
            com.CommandText =
                @"INSERT INTO Student(IndexNumber, FirstName, LastName, BirthDate, IdEnrollment, Password, Salt) VALUES (@IndexNumber, @FirstName, @LastName, @BirthDate, @IdEnrollment, @Password, @Salt)";
            com.Parameters.AddWithValue("IndexNumber", request.IndexNumber);
            com.Parameters.AddWithValue("FirstName", request.FirstName);
            com.Parameters.AddWithValue("LastName", request.LastName);
            com.Parameters.AddWithValue("BirthDate", request.BirthDate);
            com.Parameters.AddWithValue("IdEnrollment", enrollResponse.IdEnrollment);
            com.Parameters.AddWithValue("Password", PasswordHasherService.GenerateSaltedHash(request.Password, studentsalt));
            com.Parameters.AddWithValue("Salt", studentsalt);
            com.ExecuteNonQuery();

            transaction.Commit();
            return enrollResponse;
        }

        public PromoteStudentResponse PromoteStudents(PromoteStudentRequest request)
        {
            var response = new PromoteStudentResponse();
            using var con = new SqlConnection(SetConnection.GetConnection());
            using var com = new SqlCommand();

            con.Open();
            com.Connection = con;
            var transaction = con.BeginTransaction();
            com.Transaction = transaction;

            com.CommandText = "SELECT enr.IdEnrollment from Enrollment enr INNER JOIN Studies st on enr.IdStudy = st.IdStudy WHERE st.Name = @Name and enr.Semester = @Semester";
            com.Parameters.AddWithValue("Name", request.Name);
            com.Parameters.AddWithValue("Semester", request.Semester);

            var dr = com.ExecuteReader();
            if (!dr.Read())
            {
                dr.Close();
                throw new  Exception ("Nie znalezono semestru i/lub studiów");
               
            }
            else
            {
                dr.Close();


                using (SqlConnection conn = new SqlConnection(SetConnection.GetConnection()))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("PromoteStudents", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@Name", request.Name));
                    cmd.Parameters.Add(new SqlParameter("@Semester", request.Semester));
                    cmd.ExecuteReader();
                    conn.Close();
                }
                response.Semester = request.Semester + 1;
                response.StartDate = DateTime.Now.ToString("dd.MM.yyyy");
                response.StudiesName = request.Name;

                transaction.Commit();
                
            }
        
            return response;
        }

        public bool Validate(string index)
        {
            using (var client = new SqlConnection("Data Source = db-mssql.pjwstk.edu.pl; Initial Catalog = s16597; Integrated Security = True"))
            {
                using (var command = new SqlCommand())
                {
                    command.Connection = client;
                    client.Open();
                    command.CommandText = "SELECT * FROM Student st WHERE st.IndexNumber Like @Index";
                    command.Parameters.AddWithValue("Index", index);
                    var dr = command.ExecuteReader();
                    if (dr.Read())
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        private static byte[] GetSalt(int maximumSaltLength)
        {
            var salt = new byte[maximumSaltLength];
            using (var random = new RNGCryptoServiceProvider())
            {
                random.GetNonZeroBytes(salt);
            }

            return salt;
        }

    }
}
