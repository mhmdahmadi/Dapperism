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
            var lst = rep.GetAll(4, 25);
            st1.Stop();

            var st2 = new Stopwatch();
            st2.Start();
            var lst2 = rep.GetAll(9, 45);
            st2.Stop();

            var st3 = new Stopwatch();
            st3.Start();
            var lst3 = rep.GetAll(18, 30);
            st3.Stop();

            var st4 = new Stopwatch();
            st4.Start();
            var lst4 = rep.GetAll(13, 60);
            st4.Stop();

            var st5 = new Stopwatch();
            st5.Start();
            var lst5 = rep.GetAll(100, 7);
            st5.Stop();

            var t1 = st1.ElapsedMilliseconds;
            var t2 = st2.ElapsedMilliseconds;
            var t3 = st3.ElapsedMilliseconds;
            var t4 = st2.ElapsedMilliseconds;
            var t5 = st3.ElapsedMilliseconds;
        }
    }
}
