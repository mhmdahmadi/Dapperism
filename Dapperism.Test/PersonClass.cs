using System;
using System.Collections.Generic;
using System.Data;
using Dapperism.Attributes;
using Dapperism.Entities;
using Dapperism.Enums;
using Dapperism.Utilities;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;


namespace Dapperism.Test
{
    [TableName("Info")]
    [SchemaName("dbo")]
    [ViewName("V_Person")]
    [InsertSpName("PR_Info_Insert")]
    [UpdateSpName("PR_Info_UpdateByPK")]
    [DeleteSpName("PR_Info_DeleteByPK")]
    [SelectAllSpName("PR_Info_SelectAll")]
    [SelectByIdSpName("PR_Info_SelectByPK")]
    public class PersonClass : Entity<PersonClass>
    {
        [PrimaryKey(AutoNumber.Yes)]
        public long Id { get; set; }

        [ColumnName("Name")]
        public string Name { get; set; }

        [StringLengthValidator(3, RangeBoundaryType.Inclusive, 7, RangeBoundaryType.Inclusive,MessageTemplate = "خطا")]
        [NotNullValidator]
        [SpParameterName("@F")]
        public string Family { get; set; }

        [ViewColumn]
        [NotSpParameter]
        [Separated]
        public DateTime BirthDate { get; set; }

        [Separated]
        public PersianDateTime PersianDateTime
        {
            get
            {
                PersianDateTime dt = BirthDate;
                return dt;
            }
        }


        public int Age { get; set; }
    }

    public class CarClass
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public string Pelak { get; set; }
        public int ProductYear { get; set; }
    }

    [ViewName("PC")]
    public class PersonCarClass
    {
        public long Id { get; set; }
        public long PersonId { get; set; }

        [ForeignKey("CarClass")]
        public long CarId { get; set; }

        public DateTime FromDate { get; set; }

        [ParameterDirection(ParameterDirection.Output)]
        public long Price { get; set; }

        public IList<CarClass> CarClass { get; set; }
    }
}
