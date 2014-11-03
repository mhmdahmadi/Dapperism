using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            var st1 = new Stopwatch();
            st1.Start();
            var lst = rep.GetAll(2, 15);
            st1.Stop();

            var st2 = new Stopwatch();
            st2.Start();
            var lst2 = rep.GetAll(3, 15);
            st2.Stop();

            var st3 = new Stopwatch();
            st3.Start();
            var lst3 = rep.GetAll(4, 15);
            st3.Stop();

            var t1 = st1.ElapsedMilliseconds;
            var t2 = st2.ElapsedMilliseconds;
            var t3 = st3.ElapsedMilliseconds;
        }
    }
}
