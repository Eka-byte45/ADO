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
		
		protected override void buttonOk_Click(object sender, EventArgs e)
		{
			DataBase.Connector.Insert
				(
				"Students",
				"last_name,first_name,middle_name,birth_date,email,phone,[group]",
				$"{tbLastName.Text},{tbFirstName.Text},{tbMiddleName.Text},{dtpBirthDate.Value.ToString("yyyy-MM-dd")},{tbEmail.Text},{tbPhone.Text},{cbGroup.SelectedValue}"
				);
		}
	}
}
