using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cw5.Connection
{
    public static class SetConnection
    {
        private const string ConString = "Data Source=db-mssql;Initial Catalog=s16597;Integrated Security=True;MultipleActiveResultSets=true";

        public static string GetConnection()
        {

            return ConString;

        }

    }
}
