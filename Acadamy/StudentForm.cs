using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace Acadamy
{

	public partial class StudentForm : HumanForm
	{
		public int StudentId { get; set; }
		private DBtools.Connector _connector;

		public StudentForm(DBtools.Connector connector)
		{
			InitializeComponent();
			this._connector = connector;
			StudentId = 0;
			//tbLastName.Text = "Жук";
			//tbFirstName.Text = "Василий";
			//tbMiddleName.Text = "Петрович";
			//dtpBirthDate.Text = "1977.10.24";
			//tbEmail.Text = "bazilik_spb@mail.ru";
			//tbPhone.Text = "+7(911)024-56-78";
			////tbLastName.Text = "+7(911)024-56-78";
			//DataTable groups = DataBase.Connector.Select("SELECT * FROM Groups");
			//cbGroup.DataSource = groups;
			//cbGroup.DisplayMember = "group_name";
			//cbGroup.ValueMember = "group_id";
			DataTable groups = _connector.Select("SELECT * FROM Groups");

			cbGroup.DataSource = groups;
			cbGroup.DisplayMember = "group_name"; 
			cbGroup.ValueMember = "group_id";
		}

		public void LoadStudentData(StudentData data)
		{
			if (data == null) return;

			tbLastName.Text = data.LastName;
			tbFirstName.Text = data.FirstName;
			tbMiddleName.Text = data.MiddleName;
			dtpBirthDate.Value = data.BirthDate;
			tbEmail.Text = data.Email;
			tbPhone.Text = data.Phone;

			cbGroup.SelectedValue = data.GroupId;
		}
		protected override void buttonOk_Click(object sender, EventArgs e)
		{
			
			string lastName = tbLastName.Text.Trim();
			string firstName = tbFirstName.Text.Trim();
			string middleName = tbMiddleName.Text.Trim();
			DateTime birthDate = dtpBirthDate.Value;
			string email = tbEmail.Text.Trim();
			string phone = tbPhone.Text.Trim();

			if (string.IsNullOrWhiteSpace(lastName) || string.IsNullOrWhiteSpace(firstName))
			{
				MessageBox.Show("Пожалуйста, укажите фамилию и имя.", "Ошибка", MessageBoxButtons.OK);
				return;
			}

			if (StudentId > 0)//редактировать
			{
				
				string setValues = $"last_name='{lastName}', first_name='{firstName}', middle_name='{middleName}', " +
								   $"birth_date='{birthDate:yyyy-MM-dd}', email='{email}', phone='{phone}', [group]={cbGroup.SelectedValue}";

				string whereCondition = $"stud_id = {StudentId}";

				_connector.Update("Students", setValues, whereCondition);
			}
			else//добавлять
			{
				_connector.Insert
					(
				"Students",
				"last_name,first_name,middle_name,birth_date,email,phone,[group]",
				$"{tbLastName.Text},{tbFirstName.Text},{tbMiddleName.Text},{dtpBirthDate.Value.ToString("yyyy-MM-dd")},{tbEmail.Text},{tbPhone.Text},{cbGroup.SelectedValue}"
				);
			}

			this.Close();
		}
	}	
	
}
