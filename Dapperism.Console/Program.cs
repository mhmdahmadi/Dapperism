using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Dapperism.DataAccess;
using Dapperism.Entities;
using Dapperism.Utilities;

namespace Dapperism.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            FrameworkSettings.WarmingUp();

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

            var st6 = new Stopwatch();
            st6.Start();
            var lst6 = rep.GetAll();
            st6.Stop();


            var st7 = new Stopwatch();
            st7.Start();
            var lst7 = rep.Insert(new Order()
                {
                    OrderId = 11080,
                    CustomerId = "BONAP",
                    EmployeeId = 8,
                    ShippedDate = DateTime.Now,
                    Freight = 12,
                    OrderDate = DateTime.Now,
                    RequiredDate = DateTime.Now,
                    ShipAddress = "zzz",
                    ShipCity = "zzzz",
                    ShipCountry = "zzzz",
                    ShipName = "zzzzz",
                    ShipPostalCode = "2222",
                    ShipRegion = "zzzzz",
                    ShipVia = 2
                });
            st7.Stop();

            var st8 = new Stopwatch();
            st8.Start();
            var lst8 = rep.InsertOrUpdate(new Order()
            {
                OrderId = 11190,
                CustomerId = "BONAP",
                EmployeeId = 8,
                ShippedDate = DateTime.Now,
                Freight = 12,
                OrderDate = DateTime.Now,
                RequiredDate = DateTime.Now,
                ShipAddress = "x",
                ShipCity = "x",
                ShipCountry = "x",
                ShipName = "x",
                ShipPostalCode = "7",
                ShipRegion = "x",
                ShipVia = 2
            });
            st8.Stop();

            var st9 = new Stopwatch();
            st9.Start();
            var lst9 = rep.Insert(new Order()
            {
                OrderId = 11080,
                CustomerId = "BONAP",
                EmployeeId = 8,
                ShippedDate = DateTime.Now,
                Freight = 12,
                OrderDate = DateTime.Now,
                RequiredDate = DateTime.Now,
                ShipAddress = "zzz",
                ShipCity = "zzzz",
                ShipCountry = "zzzz",
                ShipName = "zzzzz",
                ShipPostalCode = "2222",
                ShipRegion = "zzzzz",
                ShipVia = 2
            });
            st9.Stop();

            var st10 = new Stopwatch();
            st10.Start();
            var lst10 = rep.Insert(new Order()
            {
                OrderId = 11190,
                CustomerId = "BONAP",
                EmployeeId = 8,
                ShippedDate = DateTime.Now,
                Freight = 12,
                OrderDate = DateTime.Now,
                RequiredDate = DateTime.Now,
                ShipAddress = "zzz",
                ShipCity = "zzzz",
                ShipCountry = "zzzz",
                ShipName = "zzzzz",
                ShipPostalCode = "2222",
                ShipRegion = "zzzzz",
                ShipVia = 2
            });
            st10.Stop();

            var lstt = Assembly.GetExecutingAssembly().GetTypes().Where(x=>x.GetInterfaces().Contains(typeof(IEntity))).ToList();

            var t1 = st1.ElapsedMilliseconds;
            var t2 = st2.ElapsedMilliseconds;
            var t3 = st3.ElapsedMilliseconds;
            var t4 = st4.ElapsedMilliseconds;
            var t5 = st5.ElapsedMilliseconds;
            var t6 = st6.ElapsedMilliseconds;
            var t7 = st7.ElapsedMilliseconds;
            var t8 = st8.ElapsedMilliseconds;
            var t9 = st9.ElapsedMilliseconds;
            var t10 = st10.ElapsedMilliseconds;

        }
    }
}
