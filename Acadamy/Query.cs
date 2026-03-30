using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acadamy
{
	internal class Query
	{
		public string Fields {  get; set; }
		public string Tables  { get; set; }
		public string Condition { get; set; }
		public string GroupBy {  get; set; }
		public Query(string fields,string tables,string condition = "", string groupBy = "")
		{
			this.Fields = fields;
			this.Tables = tables;
			this.Condition = condition;
			this.GroupBy = groupBy;
		}
		public override string ToString()
		{
			string query = $"SELECT {Fields} FROM {Tables}";
			if (Condition != "") query += $" WHERE {Condition}";
			if (!string.IsNullOrEmpty(GroupBy)) query += $" GROUP BY {GroupBy}";
			return query;
		}
	}
}
