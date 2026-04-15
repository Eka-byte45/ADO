using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Drawing;
using System.IO;

namespace DBtools
{
    public class Connector
    {
		string connection_string;//приватное поле для хранения строки подключения
		SqlConnection connection;//приватное поле для хранения объекта соединения с базой данных
		public Connector(string connection_string)//конструктор класса, который принимает строку подключения
		{
			Console.WriteLine(connection_string);//Выводим строку подключения к локальной базе данных на консоль
			this.connection_string = connection_string;//полученное значение параметра присваиваем приватному полю класса
			connection = new SqlConnection(connection_string);//создаем новый объект,используя переданную строку подключения
		}

		public DataTable Select(string cmd)//В метод передаем строку cmd это наш запрос 
		{
			DataTable table = new DataTable();//создаем объект DataTable - представляет из себя структуру для хранения данных в виде таблицы
			connection.Open();//Открываем соединение с  базой данных
			SqlCommand command = new SqlCommand(cmd, connection);//создаем объект SqlCommand для того чтобы выполнить SQL-запрос,cmd -сам запрос в виде строки, connection - соединение
			SqlDataReader reader = command.ExecuteReader();//Выполнение запроса и получение результирующего набора данных
														   // Перебираем все колонки результата, чтобы узнать их имена
			for (int i = 0; i < reader.FieldCount; ++i)
			{
				Console.Write(reader.GetName(i) + "\t");//Печатаем имена колонок через табуляцию
				table.Columns.Add(reader.GetName(i));//добавляем колонку с таким именем в наш DataTable
			}
			Console.WriteLine();
			// Читаем данные построчно из полученного набора результатов
			while (reader.Read())//пока есть доступные записи
			{
				DataRow row = table.NewRow();//создаем новую строку для DataTable
				//Console.WriteLine($"{reader[0]}\t{reader[1]}\t{reader[2]}\t{reader[3]}");
				// Перебираем поля текущего ряда и печатаем значения колонок
				for (int i = 0; i < reader.FieldCount; ++i)
				{
					Console.Write($"{reader[i]}\t\t"); // Значения колонок выводятся через двойную табуляцию
					row[i] = reader[i];//записываем значение поля в соответствующую ячейку новой строки
				}
				Console.WriteLine();//Переходим на новую строку в консоли после вывода всех значений текущей строки
				table.Rows.Add(row);//Добавляем заполненную строку в DataTable.
			}
			// Завершаем работу с набором данных и закрываем его
			reader.Close();//Закрываем SqlDataReader — освобождаем ресурсы, связанные с чтением данных.
			connection.Close();//Закрываем соединение с базой данных.
			return table;//Возвращаем заполненный DataTable вызывающему коду.
		}
		public Dictionary<string,int> GetDictionary(string table,string condition ="")//метод для получения из базы данных данны в виде словаря, где ключ это строка - имя, а значение это целое число - id
		{
			Dictionary<string,int> dictionary = new Dictionary<string, int> ();//создаем объект в виде словаря, первое значение это строка, то есть ключ, а значением выступает целое число
			string cmd = $"SELECT {table.Substring(0, table.Length - 1)}_name,{table.Substring(0, table.Length - 1)}_id FROM {table}";//формируем строку запроса
			if (condition != "") cmd += $" WHERE {condition}";//если условие не пустая строка , то к cmd мы добавляем условие
			SqlCommand command = new SqlCommand(cmd, connection);//создаем объект SqkCommand для выполнения запроса, передаем саму строку запроса и соединение
			connection.Open();//открываем соединение с БД
			SqlDataReader reader = command.ExecuteReader();//Выполняем команду и сохраняем результат в объекте SqlDataReader
			while(reader.Read())//читаем данные построчно пока они есть
			{
				dictionary.Add(reader[0].ToString(),Convert.ToInt32(reader[1]));//добавляем данные в словарь,reader[0] - строка (_name),reader[1] - это (_id)
			}
			reader.Close ();//закрываем поток для чтения данных
			connection.Close ();//закрываем соединение с БД
			return dictionary;//возвращаем заполненный словарь
		}
		public DataTable Select(string fields, string tables, string condition = "")//принимает строки: поля, таблицы и условие
		{
			string cmd = $"SELECT {fields} FROM {tables}";//формируем запрос
			if (condition != "") cmd += $" WHERE {condition}";//если есть условие, то добавляем его к запросу
			cmd += ";";//здесь запрос заканчивается 
			return Select(cmd);//вызывает другой метод с именем Select,перегрузка
		}

		public object Scalar(string cmd)
		{
			object result = null;//создаем переменную для хранения результата
			connection.Open();//открываем соединение сБД
			SqlCommand command = new SqlCommand(cmd, connection);//создаем команду для выполнения SQL-запроса
			result = command.ExecuteScalar();//Выполнение скалярного запроса, может вернуть что угодно
			connection.Close();//закрываем соединение
			return result;//возвращаем результат в виде объекта, который может быть в виде числа, текста, null
		}
		public int GetMaxPrimaryKey(string table)//метод узнвет имя первой колонки в таблице, предположительно id, выполняет скалярный запрос, чтобы найти самое большое число в колонке
		{
			string cmd = $"SELECT * FROM {table}";//формируем запрос для получения всех данных из таблицы
			SqlCommand command = new SqlCommand(cmd, connection);//создаем объект SqlCommand, передаем в него запрос и соединенние
			connection.Open();//открываем соединение
			SqlDataReader reader = command.ExecuteReader();//выполняем команду и сохраняем в sqlDataReader, reader будет содержать информацию о структуре и данных
			string pk_name = reader.GetName(0);//получаем имя первой колонки в таблице, предположительно это PK
			reader.Close();//закрываем reader
			connection.Close();//закрываем соединение
			return (int)Scalar($"SELECT MAX({pk_name}) FROM {table}");//здесь используем наш метд Scalar, в который передаем запрос, который ищет максимальное значение id из таблицы,преобразуем к int,так как у нас Scalar возвращает объект 
		}
		public int GetNextPrimaryKey(string table)
		{
			return GetMaxPrimaryKey(table) + 1;// добавляем 1 к найденному максимальному id и получаем максимальный id из таблицы
		}
		public string GetPrimaryKeyColumnName(string table)//метод спрашивает как называется колонка, которая является первичным ключом в этой таблице
		{
			//INFORMATION_SCHEMA: Это специальные «системные таблицы» внутри базы данных. В них хранится информация о самой структуре базы: какие есть таблицы, колонки, ключи.
			//KEY_COLUMN_USAGE: Это конкретная системная таблица, где записано, какие колонки являются ключами.
			//WHERE TABLE_NAME = N'{table}': Мы ищем информацию только для той таблицы, которую передали в метод.
			//AND CONSTRAINT_NAME LIKE N'PK_%': Мы ищем только те ключи (CONSTRAINT), название которых начинается на PK_ (обычно так называют первичные ключи).
			//SELECT COLUMN_NAME: Мы просим базу вернуть нам только имя колонки.
			//string raw = @"RAW string"; // RAW - строка игнорирует переносы
			string cmd = $@"SELECT INFORMATION_SCHEMA.KEY_COLUMN_USAGE.COLUMN_NAME
FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE
WHERE TABLE_NAME = N'{table}'
AND CONSTRAINT_NAME LIKE N'PK_%'";

			return (string)Scalar(cmd);
		}
		public void Insert(string cmd)//Это метод для выполнения любых команд изменения данных (INSERT, UPDATE, DELETE)
		{
			SqlCommand command = new SqlCommand(cmd, connection);//создаем команду
			connection.Open();//открываем соединение
			try
			{
				command.ExecuteNonQuery();//выполняем запрос с помощью ExecuteNonQuery
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.GetType());
				Console.WriteLine(ex.Message);
				if (ex.GetType() == typeof(SqlException) && ex.Message.Contains("_id"))//if (ex.GetType() == typeof(SqlException)): Мы проверяем, что ошибка пришла именно от базы данных SQL Server.&& ex.Message.Contains("_id"): Мы проверяем текст ошибки. 
				{
					Console.WriteLine("Good");//Если в нём встречается подстрока _id, то программа выведет в консоль слово "Good".
				}
			}

			connection.Close();//закрываем соединение
		}
		public void Insert(string table, string fields, string values)//принимает таблицу, поля и значения, цель метода «Если в таблице Users нет записи, где first_name='Иван' И last_name='Петров', то вставь такую запись».
		{
			//Метод разбивает входные строки на массивы, метод уверен, что количество колонок и значений совпадает
			string condition = "";
			string[] s_fields = fields.Split(',');//первая колонка
			string[] s_values = values.Split(',');//первое значение
			//string parsed_values = $"N'{s_values[0]}',";
			string parsed_fields = "";
			string parsed_values = "";
			for (int i = s_fields[0].Contains("_id")? 1 : 0; i < s_fields.Length; ++i)//«Содержит ли первая колонка в списке _id?»,да, тоцикл начинается с индекса 1, если нет , то цикл начинается с индекса 0
			{
				if (s_values[i] == "") continue;
				condition += $" {s_fields[i]}=N'{s_values[i]}' ";//Сбор условия для проверки (Переменная condition)
				parsed_fields += s_fields[i];//Сбор списка колонок для вставки (Переменная parsed_fields)
				if (i != s_fields.Length - 1) parsed_fields += ",";
				parsed_values += s_values[i].Length >1 && s_values[i][0] != 'N' &&  s_values[i][1]!= '\'' ? $"N'{s_values[i]}'" : s_values[i];//Сбор значений для вставки

				if (i != s_fields.Length - 1)
				{
					condition += "AND";
					parsed_values += ",";
				}

			}
			string cmd = $"IF NOT EXISTS (SELECT {GetPrimaryKeyColumnName(table)} FROM {table} WHERE {condition})";
			cmd += $"INSERT {table} ({parsed_fields}) VALUES ({parsed_values})";
			Insert(cmd);
		}
		public void Update(string cmd)
		{
			SqlCommand command = new SqlCommand(cmd,connection);
			connection.Open();
			command.ExecuteNonQuery();
			connection.Close();	
		}
		public void UploadPhoto(byte[] image, int id,string field, string table)//метод для загрузки фото принимает массив байт,id,поле(название колонки, где хранится фото) и название таблицы, находит нужную запись по id и заменяет старое значение в указанной колонке на новое.
		{
			string cmd = $"UPDATE {table} SET {field}=@image WHERE {GetPrimaryKeyColumnName(table)}={id}";//«Изменить таблицу [название_таблицы], задав полю [название_поля] значение [параметр @image], где [имя_первичного_ключа] равно [значение_переменной id]».
			SqlCommand command = new SqlCommand(cmd,connection);
			command.Parameters.Add("@image", SqlDbType.VarBinary).Value = image;
			connection.Open();//открываем соединение
			command.ExecuteNonQuery();//выполняем команду
			connection.Close();//закрываем соединение
		}
		//Находит в базе данных нужную запись по ID и извлекает из неё байты фотографии.
		//Превращает массив байтов (цифры) в поток данных (MemoryStream).
		//Создает из этого потока объект Image (картинку), который понимает графическая система компьютера.
		public Image DownladPhoto(string table, string field,int id)
		{
			Image photo = null;//создаем переменную для картинки, изначально она пустая

			string cmd = $"SELECT {field} FROM {table} WHERE {GetPrimaryKeyColumnName(table)}={id}";//формируем запрос для получения данных
			SqlCommand command = new SqlCommand(cmd,connection);//создаем команду
			connection.Open();//открываем соединение
			SqlDataReader reader = command.ExecuteReader();//выполняем запрос
			if(reader.Read())//нашлись ли данные?
			{
				byte[] data = reader[0] as byte[];//извлечение данных из БД, В базе фото хранится как массив байтов (byte[]),reader[0] берет первое (и единственное) поле из результата.
				if (data!= null)//проверка на было ли что-то 
				{
					MemoryStream ms = new MemoryStream(data);// Создаем поток в памяти, куда загружаем наши байты.
					photo = Image.FromStream(ms);//создаем объект Image, который можно отобразить
				}
			}
			connection.Close();//закрываем соединение
			return photo;//возвращаем фото или null
		}
	}
}
