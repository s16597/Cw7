using Cw5.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cw5.DAL
{
    public class MockDbService : IDbService
    {
        private static IEnumerable<Student> _students;

        static MockDbService()
        {
            _students = new List<Student>
            {
                new Student{IndexNumber="",FirstName="Kinga",LastName="Malecka"},
                new Student{IndexNumber="",FirstName="Robert",LastName="Rak"},
                new Student{IndexNumber="",FirstName="Antoni",LastName="Pasek"}
            };
        }
        public IEnumerable<Student> getStudents()
        {
            return _students;
        }

    }
}
