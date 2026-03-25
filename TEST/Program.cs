using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADO;

namespace TEST
{
	internal class Program
	{
		static void Main(string[] args)
		{

			string connection_string = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Movies_PV_521;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
			Connector connector = new Connector(connection_string);

			//connector.Select
			//	(
			//	"title,release_date,first_name,last_name",
			//	"Movies,Directors",
			//	"director = director_id"
			//	);
			//Console.WriteLine($"Количество записей: {connector.Scalar("SELECT COUNT(*) FROM Directors")}");
			connector.Select("SELECT* FROM Directors");

			Console.WriteLine("Метод Select выполнен. Нажмите любую клавишу...");
			Console.ReadKey();
		}
	}
}
