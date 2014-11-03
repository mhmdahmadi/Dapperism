using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapperism.DataAccess;

namespace Dapperism.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var rep = new Repository<Order>();
            var lst = rep.GetAll(2, 15);

        }
    }
}
