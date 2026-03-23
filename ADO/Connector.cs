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
		//Переделываем метод GetMaxPrimaryKey, используя GetPrimaryKeyName
		public int GetMaxPrimaryKey(string table)
		{
			string pk_column_name = GetPrimaryKeyName(table);//получаем имя столбца, который является первичным ключом
			if (pk_column_name == null)
			{
				throw new Exception($"Первичный ключ для таблицы {table} не найден");
			}
			string cmd = $"SELECT MAX([{pk_column_name}]) FROM {table}";//формируем запрос на получение максимального значения из столбца, который мы нашли с помощью функции GetPrimaryKeyName
			//[] - служат для экранирования имен, если они содержат пробелы или зарезервированные слова
			object result = Scalar(new SqlCommand(cmd));
			return result == null ? 0 : Convert.ToInt32(result);
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
		//Переписанный метод Scalar, он принимает готовую команду с параметрами
		public object Scalar(SqlCommand command)
		{
			command.Connection = connection;
			object result = null;
			connection.Open();
			result = command.ExecuteScalar();
			connection.Close();
			return result;
		}
		//Метод полчения названия первичного ключа
		public string GetPrimaryKeyName(string table)
		{
			string pk_name = null;//строка для хранения названия первичного ключа

			//данный запрос получает имя столбца, который является первичным ключом
			//в системной таблице INFORMATION_SCHEMA.KEY_COLUMN_USAGE запрос ищет, где имя таблицы совпадает с переданным параметром
			//а имя ограничения начинается с PK
			string cmd = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE WHERE TABLE_NAME = @table AND CONSTRAINT_NAME LIKE 'PK_%'";
			//создание объекта комманды для выполнения SQL-запроса
			SqlCommand command = new SqlCommand (cmd);
			//здесь добавляем параметр в команду,защита от SQL-инъекций
			command.Parameters.AddWithValue("@table",table);
			object result = this.Scalar(command);//с помощью функции Scalar выполняем команду
			pk_name = result.ToString();
			return pk_name;
		}
		//Метод, проверяющий существование записи в базе данных
		public bool RecordExists(string table, Dictionary <string,object> columns)
		{
			string conditions = "";//строка для того, чтобы хранить значения WHERE
			foreach(KeyValuePair<string,object> col in columns)
			{
				conditions += $"[{col.Key}] = @{col.Key} AND ";
			}

			conditions = conditions.Substring(0,conditions.Length-5);//здесь убираем повторение в виде AND

			string cmd = $"SELECT COUNT(*) FROM {table} WHERE {conditions}";//считаем количество строк, где выполняются условия conditions
			SqlCommand command = new SqlCommand (cmd);//создаем объект комманды для выполнения запроса
			foreach(KeyValuePair<string,object> col in columns)//проходим по словарю и добавляем значения параметров в команду
			{
				command.Parameters.AddWithValue($"@{col.Key}", col.Value);

			}
			int count = (int)Scalar(command);//преобразуем object в int
			return count > 0;//если такая запись в таблице есть, то мы возвращаем true count>0, а если нет, то false count=0
		}

		//Метод вставки значений в таблицу базы данных
		public void Insert(string table, Dictionary<string,object>unique_columns,Dictionary<string,object>all_columns)
		{
			string pk = GetPrimaryKeyName(table);//получаем название перивчного ключа
			if (pk != null && !all_columns.ContainsKey(pk))//проверяем существует ли он вообще и был ли передан в словаре
			{
				int next_id = GetNextPrimaryKey(table);//если этого не было, то мы ищем следующий id с помощью метода GetNextPrimaryKey
				all_columns.Add(pk, next_id);//добавляем в словарь ключ/значение
			}

			if(RecordExists(table,unique_columns))//проверяем существует ли запись в таблице, если есть, то ничего не добавляем
			{
                Console.WriteLine("Запись с такими параметрами уже существует!");
				return;
			}

			string fields = "";
			string values = "";

			bool is_first = true;	//флаг для постановки запятой, если первая итерация то запятая не ставится
			foreach(KeyValuePair<string,object> col in all_columns)
			{
				if(!is_first)
				{
					fields += ", ";
					values += ", ";
				}
				fields += "[" + col.Key + "]";
				values += "@" + col.Key;
				is_first = false;
			}

			string cmd = "INSERT INTO " + table + " (" + fields + ") VALUES (" +values +");";
			SqlCommand command = new SqlCommand (cmd,connection);

			foreach(KeyValuePair<string,object> col in all_columns)
			{
				command.Parameters.AddWithValue("@" + col.Key, col.Value);
			}
			connection.Open ();
			try
			{
				command.ExecuteNonQuery ();
				Console.WriteLine("Запись успешно добавлена");
			}
			catch (Exception ex)
			{
                Console.WriteLine(ex.Message);
			}
			connection.Close ();
		}
	}
}
