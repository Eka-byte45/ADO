using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace Acadamy
{
	public partial class AddStudentForm : Form
	{
		DBtools.Connector connector;
		Dictionary<string, int> d_groups;
		public AddStudentForm()
		{
			InitializeComponent();
			string connectionString =
			System.Configuration.ConfigurationManager.ConnectionStrings["PV_521_Import"].ConnectionString;
			connector = new DBtools.Connector(connectionString);
			//this.connector = connector;
			//this.d_groups = groupsDict;
			d_groups = connector.GetDictionary("Groups");
			cbGroup.Items.AddRange(d_groups.Keys.ToArray());
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			string lastName = txtLastName.Text.Trim();
			string firstName = txtFirstName.Text.Trim();
			string middleName = txtMiddleName.Text.Trim();
			string email = txtEmail.Text.Trim();
			string phone = txtPhone.Text.Trim();
			DateTime birthDate = dtpBirthDate.Value;
			string selectedGroup = cbGroup.SelectedItem?.ToString() ?? "";

			if (string.IsNullOrWhiteSpace(lastName) || string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(selectedGroup))
			{
				MessageBox.Show("Пожалуйста, заполните фамилию, имя и выберите группу.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			int group_id = d_groups[selectedGroup];
			string birthDateForSql = birthDate.ToString("yyyy-MM-dd");

			string sqlQuery = $"INSERT INTO Students (last_name, first_name, middle_name, birth_date, email, phone, [group]) " +
							  $"VALUES ('{lastName}', N'{firstName}', N'{middleName}', '{birthDateForSql}', '{email}', '{phone}', {group_id})";
			try
			{
				connector.Insert(sqlQuery);

				MessageBox.Show("Студент успешно добавлен!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
				this.DialogResult = DialogResult.OK;
				this.Close();
			}
			catch (Exception ex)
			{
				
				MessageBox.Show("Произошла ошибка при сохранении:\n\n" + ex.Message, "Ошибка SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		
		//private void btnSave_Click(object sender, EventArgs e)
		//{
		//	string lastName = txtLastName.Text.Trim();
		//	string firstName = txtFirstName.Text.Trim();
		//	string middleName = txtMiddleName.Text.Trim();
		//	string email = txtEmail.Text.Trim();
		//	string phone = txtPhone.Text.Trim();

		//	DateTime birthDate = dtpBirthDate.Value;
		//	string selectedGroup = cbGroup.SelectedItem?.ToString() ?? "";

		//	if (string.IsNullOrWhiteSpace(lastName) || string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(selectedGroup))
		//	{
		//		MessageBox.Show("Пожалуйста, заполните фамилию, имя и выберите группу.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
		//		return;
		//	}

		//	int group_id = d_groups[selectedGroup];
		//	string birthDateForSql = birthDate.ToString("yyyy-MM-dd");

		//	string sqlQuery = $"INSERT INTO Students (last_name, first_name, middle_name, birth_date, email, phone, [group]) " +
		//					  $"VALUES (N'{lastName}', N'{firstName}', N'{middleName}', '{birthDateForSql}', '{email}', '{phone}', {group_id})";
		//	try
		//	{
		//		string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["PV_521_Import"].ConnectionString;

		//		using (SqlConnection myConnection = new SqlConnection(connectionString))
		//		{
		//			myConnection.Open(); 
		//			SqlCommand command = new SqlCommand(sqlQuery, myConnection);
		//			command.ExecuteNonQuery();
		//		}
		//		MessageBox.Show("Студент успешно добавлен!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
		//		this.DialogResult = DialogResult.OK;
		//		this.Close();

		//	}
		//	catch (Exception ex)
		//	{
		//		MessageBox.Show("Произошла ошибка при сохранении:\n\n" + ex.Message, "Ошибка SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
		//	}
		//}

	}
}
