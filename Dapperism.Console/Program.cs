using System;
using System.Data.SqlClient;
using System.Diagnostics;
using Dapper;
using Dapperism.DataAccess;
using Dapperism.Enums;
using Dapperism.Query;
using Dapperism.Settings;

namespace Dapperism.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            DapperismSettings.WarmingUp();
            DapperismSettings.PreventArabicLetters();
            DapperismSettings.PreventPersianNumbers();
            var rep = new Repository<Order>();



            var a = rep.Count();
            var b =
                rep.Count(
                    new QueryExpression<Order>()
                    .Where(x => x.ShipVia, ConditionType.GreaterThan, 1)
                    .And
                        .Where(x => x.ShipVia, ConditionType.LessThanEqual, 3)
                        .Select());

            /*
            var sqlCnn = new SqlConnection("Data Source=.;Initial Catalog=person;Integrated Security=True");

            var st11 = new Stopwatch();
            st11.Start();
            var query = new QueryExpression<Order>()
                .Select();
            var aaa = rep.GetByFilter(query);
            st11.Stop();

            var st2 = new Stopwatch();
            st2.Start();
            var lst2 = rep.GetAll(9, 45);
            st2.Stop();

            sqlCnn.Open();
            var b = sqlCnn.Query<bool>("SELECT TOP 1 1 FROM Info WHERE id = '24'");
            sqlCnn.Close();

            var st1 = new Stopwatch();
            st1.Start();
            var lst = rep.GetAll(4, 41, selectClause: new[]
            {
                "OrderID", "ShipName"
            });
            st1.Stop();



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
            var t11 = st11.ElapsedMilliseconds;

            var all = t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9 + t10 + t11;*/
        }
    }
}
