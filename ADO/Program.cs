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
            Console.WriteLine(connection_string);//Выводим строку подключения к локальной базе данных на консоль

			//Создание объекта SqlConnection connection,подключение к базе данных
			SqlConnection connection = new SqlConnection(connection_string);
			connection.Open();//Открываем соединение с  базой данных
			//string cmd = "SELECT* FROM Movies";
			string cmd = "SELECT movie_id,title,release_date,first_name,last_name FROM Movies,Directors WHERE director=director_id";//Формируем запрос на получение данных из таблиц Directors, Movies и сохраняем данные в строку
			SqlCommand command = new SqlCommand(cmd, connection);//Создание комманды SQL-запроса, привязываем к открытому соединению
			SqlDataReader reader = command.ExecuteReader();//Выполнение запроса и получение результирующего набора данных
			// Перебор и вывод имен колонок в первой строке вывода
			for (int i=0; i<reader.FieldCount;++i)
			{
                Console.Write(reader.GetName(i)+"\t");//Печатаем имена колонок через табуляцию
			}
            Console.WriteLine();
			// Читаем данные построчно из полученного набора результатов
			while (reader.Read())//пока есть доступные записи
			{
				//Console.WriteLine($"{reader[0]}\t{reader[1]}\t{reader[2]}\t{reader[3]}");
				// Перебираем поля текущего ряда и печатаем значения колонок
				for (int i=0; i<reader.FieldCount;++i)
				{
					Console.Write($"{reader[i]}\t\t"); // Значения колонок выводятся через двойную табуляцию
				}
				Console.WriteLine();
			}
			// Завершаем работу с набором данных и закрываем его
			reader.Close();
			// Меняем команду на выбор количества записей в таблице Movies
			command.CommandText = "SELECT COUNT(*) FROM Movies";
			// Выполняем новый запрос и получаем единственное скалярное значение — количество записей
			Console.WriteLine($"Количество записей: \t{command.ExecuteScalar()}");
			// Закрываем соединение с базой данных
			connection.Close();
		}
	}
}
