using Cw5.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cw5.DAL
{
    public interface IDbService
    {
        public IEnumerable<Student> getStudents();

}
}
