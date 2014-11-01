using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Dapperism.DataAccess;
using Dapperism.Enums;
using Dapperism.Query;
using Dapperism.Utilities;


namespace Dapperism.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var sw = new Stopwatch();

            sw.Start();
            var rep = new Repository<PersonClass>();
            var id = rep.Insert(new PersonClass()
               {
                   BirthDate = new PersianDateTime(1396, 5, 2),
                   Family = "Fathi",
                   Id = 70,
                   Name = "Hamed"
               });
            sw.Stop();


            var sw2 = new Stopwatch();

            sw2.Start();
            var rep2 = new Repository<PersonClass>();
            var id2 = rep2.Insert(new PersonClass()
            {
                BirthDate = new PersianDateTime(1356, 5, 21),
                Family = "Faaaaathi",
                Id = 71,
                Name = "Hamesasdd"
            });
            sw2.Stop();

            var r = rep2.ValidationResults;

            var sw22 = new Stopwatch();

            sw22.Start();
            var rep22 = new Repository<PersonClass>();
            var id22 = rep22.Insert(new PersonClass()
            {
                BirthDate = new PersianDateTime(1251, 3, 11),
                Family = "Faafsdfgdsfg111aaathi",
                Id = 72,
                Name = "Ha111messdfgdfgasdd"
            });
            sw22.Stop();


            var s5 = new Stopwatch();

            s5.Start();
            var rep4 = new Repository<PersonClass>();


            rep4.Insert(new List<PersonClass>
            {
                new PersonClass()
                {
                    BirthDate = new PersianDateTime(1251, 3, 11),
                    Family = "asrd",
                    Id = 72,
                    Name = "dfg"
                },
                new PersonClass()
                {
                    BirthDate = new PersianDateTime(1251, 3, 11),
                    Family = "jhkr",
                    Id = 72,
                    Name = "cxcv"
                },
                new PersonClass()
                {
                    BirthDate = new PersianDateTime(1251, 3, 11),
                    Family = "ioprr",
                    Id = 72,
                    Name = "qvbgfh"
                },
                new PersonClass()
                {
                    BirthDate = new PersianDateTime(1251, 3, 11),
                    Family = "mkrh",
                    Id = 72,
                    Name = "afunmp"
                },
                new PersonClass()
                {
                    BirthDate = new PersianDateTime(1251, 3, 11),
                    Family = "rrfd",
                    Id = 72,
                    Name = "cvbhgjk"
                }

            });
            s5.Stop();


            var s55 = new Stopwatch();

            s55.Start();

            var rep224 = new Repository<PersonClass>();
            var id224 = rep224.InsertBySp(new PersonClass
            {
                BirthDate = new PersianDateTime(1251, 3, 11),
                Family = "cxvb",
                Id = 72,
                Name = "hj"
            });

            s55.Stop();


            var s551 = new Stopwatch();

            s551.Start();

            var rep2241 = new Repository<PersonClass>();
            var id2241 = rep2241.InsertBySp(new PersonClass
            {
                BirthDate = new PersianDateTime(1351, 6, 6),
                Family = "ctyxcv",
                Id = 72,
                Name = "ytutyutu"
            });
            var validationResults = rep2241.ValidationResults;

            s551.Stop();

            var s0 = new Stopwatch();
            s0.Start();
            var rep0 = new Repository<PersonClass>();
            rep0.UpdateBySp(new PersonClass()
                {
                    BirthDate = new PersianDateTime(1251, 3, 11),
                    Family = "xuuccv",
                    Id = 1,
                    Name = "x",
                    Age = 88

                });
            s0.Stop();


            var s01 = new Stopwatch();
            s01.Start();
            var rep01 = new Repository<PersonClass>();
            rep01.UpdateBySp(new PersonClass()
            {
                BirthDate = new PersianDateTime(1251, 3, 11),
                Family = "vfrrr",
                Id = 2,
                Name = "rrrr",
                Age = 44

            });
            s01.Stop();

            rep.DeleteBySp(new PersonClass()
            {
                BirthDate = new PersianDateTime(1251, 3, 11),
                Family = "vfrrr",
                Id = 22,
                Name = "rrrr",
                Age = 44

            });

            var s011 = new Stopwatch();
            s011.Start();
            var rep011 = new Repository<PersonClass>();
            rep011.Delete(id: 34);
            s011.Stop();

            var s0111 = new Stopwatch();
            s0111.Start();
            var rep0111 = new Repository<PersonClass>();
            rep0111.DeleteBySp(id: 54);
            s0111.Stop();

            var s66 = new Stopwatch();
            s66.Start();
            var l = rep.GetAll(selectClause: "family");
            s66.Stop();

            var s666 = new Stopwatch();
            s666.Start();
            var kl = rep.GetByIdWithSp(id: 25).Name;
            s666.Stop();


            var ttt = rep.ExecuteSqlFunction("dbo.getShamsiDate", FunctionType.Scalar, argumens: "20030319").FirstOrDefault();
            if (ttt != null)
            {
                var aaa = ttt.Result as string;
            }



            var a = new Predicate()
                .Schema("dbo")
                .TableOrView("T1")
                .Where<PersonClass>(x => x.Id, FilterOperation.Equal, 1);

            var time1 = sw.ElapsedMilliseconds;
            var time2 = sw2.ElapsedMilliseconds;
            var time3 = sw22.ElapsedMilliseconds;
            var time5 = s5.ElapsedMilliseconds;
            var time6 = s55.ElapsedMilliseconds;
            var time61 = s551.ElapsedMilliseconds;
            var time7 = s0.ElapsedMilliseconds;
            var time8 = s01.ElapsedMilliseconds;
            var time9 = s011.ElapsedMilliseconds;
            var time81 = s0111.ElapsedMilliseconds;
            var time82 = s66.ElapsedMilliseconds;
            var time84 = s666.ElapsedMilliseconds;

            var add = time1 + time2 + time3 + time5 + time6 + time61 + time7 + time8 + time9 + time81 + time82 + time84;
        }
    }
}
