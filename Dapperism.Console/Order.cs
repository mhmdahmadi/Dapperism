using System;
using Dapperism.Attributes;
using Dapperism.Entities;
using Dapperism.Enums;
using Dapperism.Extensions.Persian;
using Dapperism.Utilities;

namespace Dapperism.Console
{
    [TableName("Orders")]
    public class Order : Entity<Order>
    {
        [PrimaryKey(AutoNumber.Yes)]
        public int OrderId { get; set; }
        public string CustomerId { get; set; }
        public int? EmployeeId { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? RequiredDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public int? ShipVia { get; set; }
        public decimal? Freight { get; set; }
        public string ShipName { get; set; }
        public string ShipAddress { get; set; }
        public string ShipCity { get; set; }
        public string ShipRegion { get; set; }
        public string ShipPostalCode { get; set; }
        public string ShipCountry { get; set; }

        [Separated]
        public PersianDateTime PersianOrderDate
        {
            get { return OrderDate; }
        }
        [Separated]
        public PersianDateTime PersianRequiredDate
        {
            get { return RequiredDate; }

        }
        [Separated]
        public PersianDateTime PersianShippedDate
        {
            get { return ShippedDate; }

        }
    }
}
