using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace ADO
{
	internal class Connector
	{
		string connection_string;
		SqlConnection connection;
		public Connector(string connection_string)
		{
			Console.WriteLine(connection_string);//Выводим строку подключения к локальной базе данных на консоль
			this.connection_string = connection_string;
			connection = new SqlConnection(connection_string);
		}

		public void Select(string cmd)
		{
			connection.Open();//Открываем соединение с  базой данных
			SqlCommand command = new SqlCommand(cmd, connection);//Создание комманды SQL-запроса, привязываем к открытому соединению
			SqlDataReader reader = command.ExecuteReader();//Выполнение запроса и получение результирующего набора данных
														   // Перебор и вывод имен колонок в первой строке вывода
			for (int i = 0; i < reader.FieldCount; ++i)
			{
				Console.Write(reader.GetName(i) + "\t");//Печатаем имена колонок через табуляцию
			}
			Console.WriteLine();
			// Читаем данные построчно из полученного набора результатов
			while (reader.Read())//пока есть доступные записи
			{
				//Console.WriteLine($"{reader[0]}\t{reader[1]}\t{reader[2]}\t{reader[3]}");
				// Перебираем поля текущего ряда и печатаем значения колонок
				for (int i = 0; i < reader.FieldCount; ++i)
				{
					Console.Write($"{reader[i]}\t\t"); // Значения колонок выводятся через двойную табуляцию
				}
				Console.WriteLine();
			}
			// Завершаем работу с набором данных и закрываем его
			reader.Close();
			connection.Close();
		}
		public void Select(string fields,string tables,string condition ="")
		{
			string cmd = $"SELECT {fields} FROM {tables}";
			if (condition != "") cmd += $" WHERE {condition}";
			cmd += ";";
			Select(cmd);
		}

		public object Scalar(string cmd)
		{
			object result = null;
			connection.Open();
			SqlCommand command = new SqlCommand(cmd, connection);
			result = command.ExecuteScalar();//Выполнение скалярного запроса
			connection.Close();
			return result;
		}
		public int GetMaxPrimaryKey(string table)
		{
			string cmd = $"SELECT * FROM {table}";
			SqlCommand command = new SqlCommand(cmd, connection);
			connection.Open();
			SqlDataReader reader = command.ExecuteReader();
			string pk_name = reader.GetName(0);
			reader.Close();
			connection.Close() ;
			return (int)Scalar($"SELECT MAX({pk_name}) FROM {table}");
		}
		public int GetNextPrimaryKey(string table)
		{
			return GetMaxPrimaryKey(table) +1;
		}
		public void Insert(string cmd)
		{
			SqlCommand command = new SqlCommand (cmd, connection);
			connection.Open();
			try
			{
				command.ExecuteNonQuery();
			}
			catch (Exception ex)
			{
                Console.WriteLine(ex.GetType());
                Console.WriteLine(ex.Message);
				if(ex.GetType() == typeof(SqlException) && ex.Message.Contains("_id"))
				{
					Console.WriteLine("Good");
				}
			}
			
			connection.Close ();
		}
		
	}
}
