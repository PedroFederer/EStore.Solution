using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MyShopping.Utilities
{
	public class SqlParameterBuilder
	{
		private List<SqlParameter> _parameters = new List<SqlParameter>();

		public SqlParameterBuilder AddInt(string parameterName, int value)
		{
			SqlParameter p = new SqlParameter(parameterName, System.Data.SqlDbType.Int) { Value = value };
			_parameters.Add(p);

			return this;
		}
		public SqlParameterBuilder AddNVarchar(string parameterName, int length, string value)
		{
			SqlParameter p = new SqlParameter(parameterName, System.Data.SqlDbType.NVarChar, length) { Value = value };
			_parameters.Add(p);

			return this;
		}
		public SqlParameterBuilder AddBit(string parameterName, bool value)
		{
			SqlParameter p = new SqlParameter(parameterName, System.Data.SqlDbType.Bit) { Value = value };
			_parameters.Add(p);

			return this;
		}
		public SqlParameterBuilder AddDateTime(string parameterName, DateTime value)
		{
			SqlParameter p = new SqlParameter(parameterName, System.Data.SqlDbType.DateTime) { Value = value };
			_parameters.Add(p);

			return this;
		}
		public SqlParameter[] Build()
		{
			return _parameters.ToArray();
		}
	}
}