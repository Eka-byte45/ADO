using Acadamy.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Acadamy
{
	public partial class StudentForm : HumanForm
	{
		Models.Student student;
		public StudentForm()
		{
			InitializeComponent();

			tbLastName.Text = "Жук";
			tbFirstName.Text = "Василий";
			tbMiddleName.Text = "Петрович";
			dtpBirthDate.Text = "1977.10.24";
			tbEmail.Text = "bazilik_spb@mail.ru";
			tbPhone.Text = "+7(911)024-56-78";
			//tbLastName.Text = "+7(911)024-56-78";
			DataTable groups = DataBase.Connector.Select("SELECT * FROM Groups");
			cbGroup.DataSource = groups;
			cbGroup.DisplayMember = "group_name";
			cbGroup.ValueMember = "group_id";
		}
		public StudentForm(int id):this()
		{
			DataTable data = DataBase.Connector.Select("*", "Students", $"stud_id ={id}");
			//object[] arr = data.Rows[0].ItemArray;
			student = new Models.Student(data.Rows[0].ItemArray);
			human = student;
			Extract();
			cbGroup.SelectedValue = student.group;
			pbPhoto.Image = DataBase.Connector.DownladPhoto("Students","photo",student.id);
			
		}
		protected override void buttonOk_Click(object sender, EventArgs e)
		{
			base.buttonOk_Click(sender, e);
			student = new Models.Student(human,Convert.ToInt32(cbGroup.SelectedValue));
			//object id = (int)DataBase.Connector.Scalar($"SELECT stud_id FROM Students WHERE {student.GetCondition()}");
			if(student.id==0)DataBase.Connector.Insert("Students", $"{student.GetNames()}",$"{student.GetValues()}");
			else DataBase.Connector.Update($"UPDATE Students SET {student.GetUpdateString()} WHERE stud_id={student.id}");
			if(student.photo!= null)DataBase.Connector.UploadPhoto(student.SerializePhoto(),student.id,"photo","Students");
			//try
			//{
			//	DataBase.Connector.Insert
			//			(
			//			"Students",
			//			"last_name,first_name,middle_name,birth_date,email,phone,[group]",
			//			$"{tbLastName.Text},{tbFirstName.Text},{tbMiddleName.Text},{dtpBirthDate.Value.ToString("yyyy-MM-dd")},{tbEmail.Text},{tbPhone.Text},{cbGroup.SelectedValue}"
			//			);
			//}
			//catch (Exception ex)
			//{
			//	MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK);
			//}	
		}
	}
}
