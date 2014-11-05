using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace Dapperism.DataAccess
{
    public class DynamicParameters : IEnumerable<DynamicParameter>
    {
        private readonly List<DynamicParameter> _dpList;

        public DynamicParameters()
        {
            _dpList = new List<DynamicParameter>();
        }

        public IEnumerator<DynamicParameter> GetEnumerator()
        {
            return _dpList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public void Add(string name, object value = null, DbType? dbType = null, ParameterDirection? direction = null,
            int? size = null)
        {
            _dpList.Add(new DynamicParameter()
            {
                DbType = dbType,
                Direction = direction,
                Name = name,
                Size = size,
                Value = value
            });
        }

        public void Add(DynamicParameter dp)
        {
            _dpList.Add(new DynamicParameter()
            {
                DbType = dp.DbType,
                Direction = dp.Direction,
                Name = dp.Name,
                Size = dp.Size,
                Value = dp.Value
            });
        }

        internal Dapper.DynamicParameters ToDapperParams()
        {
            var ddp = new Dapper.DynamicParameters();
            foreach (var dp in _dpList)
                ddp.Add(dp.Name, dp.Value, dp.DbType, dp.Direction, dp.Size);
            return ddp;
        }

        public T Get<T>(string paramName)
        {
            var dp = ToDapperParams();
            return dp.Get<T>(paramName);
        }


        public static implicit operator Dapper.DynamicParameters(DynamicParameters dps)
        {
            var r = dps.ToDapperParams();
            return r;
        }
    }
}