using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace ADO
{
	internal class Program
	{
		static void Main(string[] args)
		{
			//Строка подключения к локальной базе данных
			string connection_string = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Movies_PV_521;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
           
			Connector connector = new Connector(connection_string);
			connector.Insert("INSERT Directors (director_id,first_name,last_name) VALUES (12,N'Guy',N'Richie');");
			Console.WriteLine($"PK Max:\t{connector.GetMaxPrimaryKey("Directors")}");
			//string cmd = "SELECT* FROM Movies";
			//string cmd = "SELECT movie_id,title,release_date,first_name,last_name FROM Movies,Directors WHERE director=director_id";//Формируем запрос на получение данных из таблиц Directors, Movies и сохраняем данные в строку
																																	//connector.Select(cmd);
			connector.Select("*", "Directors");
			Console.WriteLine($"Количество записей: {connector.Scalar("SELECT COUNT(*) FROM Directors")}");

			//connector.Select("SELECT* FROM Directors");
			connector.Select
				(
				"title,release_date,first_name,last_name",
				"Movies,Directors",
				"director = director_id"
				);
			Console.WriteLine($"Количество записей: {connector.Scalar("SELECT COUNT(*) FROM Directors")}");

			// Меняем команду на выбор количества записей в таблице Movies
			//command.CommandText = "SELECT COUNT(*) FROM Movies";
			// Выполняем новый запрос и получаем единственное скалярное значение — количество записей
			//Console.WriteLine($"Количество записей: \t{command.ExecuteScalar()}");
			// Закрываем соединение с базой данных
			//connection.Close();
			


			//string pk = connector.GetPrimaryKeyName("Directors");
			//string pk2 = connector.GetPrimaryKeyName("Movies");
			//Console.WriteLine($"Имя первичного ключа таблицы {pk}");
			//Console.WriteLine($"Имя первичного ключа таблицы {pk2}");

			//int number_pk = connector.GetNextPrimaryKey("Directors");
			//Console.WriteLine($"Следующий ПК {number_pk}");

			//var director = new Dictionary<string, object>
			//{
			//	{"first_name","James" },
			//	{"last_name","Cameron" }
			//};

			//bool exists = connector.RecordExists("Directors",director);
			//if (exists)
			//{
			//	Console.WriteLine("Запись найдена в базе данных");
			//}
			//else
			//{
			//	Console.WriteLine("Запись не найдена в базе данных");
			//}

			//Dictionary<string,object> unique = new Dictionary<string,object>();
			//unique.Add("first_name","James");
			//unique.Add("last_name","Cameron");
			//Dictionary<string,object> all = new Dictionary<string,object>();
			//all.Add("first_name","");
			//all.Add("last_name","");

			//Dictionary<string, object> unique = new Dictionary<string, object>();
			//unique.Add("first_name", "Леонид");
			//unique.Add("last_name", "Гайдай");
			//Dictionary<string, object> all = new Dictionary<string, object>();
			//all.Add("first_name", "Леонид");
			//all.Add("last_name", "Гайдай");
			//connector.Insert("Directors",unique,all);

			connector.Select("*", "Directors");

			//Dictionary<string, object> unique = new Dictionary<string, object>();
			//unique.Add("title", "Иван Васильевич меняет профессию");
			//unique.Add("release_date", "1973-09-17");
			//unique.Add("director","13");
			//Dictionary<string, object> all = new Dictionary<string, object>();
			//all.Add("title", "Иван Васильевич меняет профессию");
			//all.Add("release_date", "1973-09-17");
			//all.Add("director", "13");
			//connector.Insert("Movies", unique, all);

			connector.Select("*", "Movies");

		}
		
	}
}
