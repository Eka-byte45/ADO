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
	public partial class TeacherForm : HumanForm
	{
		internal Models.Teacher teacher;
		public TeacherForm()
		{
			InitializeComponent();
		}
		public TeacherForm(int id):this()
		{
			DataTable table = DataBase.Connector.Select($"SELECT *FROM Teachers WHERE teacher_id = {id}");
			teacher = new Models.Teacher(table.Rows[0].ItemArray);
			human = teacher;
			Extract();
			this.dtpWorkSince.Value = Convert.ToDateTime(teacher.work_since);
			this.tbRate.Text = teacher.rate.ToString();
			pbPhoto.Image = DataBase.Connector.DownladPhoto("Teachers","photo",id);

		}

		protected override void buttonOk_Click(object sender, EventArgs e)
		{
			base.buttonOk_Click(sender, e);
			teacher = new Models.Teacher
				(
				human, dtpWorkSince.Value.ToString("yyyy-MM-dd"),
				Convert.ToDecimal(tbRate.Text)
				);
			if(teacher.id == 0) teacher.id = Convert.ToInt32
					(
				    DataBase.Connector.Scalar($"INSERT Teachers({teacher.GetNames()}) VALUES({ teacher.GetValues()}); SELECT SCOPE_IDENTITY()")
					);
			else DataBase.Connector.Update($"UPDATE Teachers SET {teacher.GetUpdateString()} WHERE teacher_id={teacher.id}");

			if(teacher.photo != null)
			{
				DataBase.Connector.UploadPhoto(teacher.SerializePhoto(), teacher.id, "photo", "Teachers");
			}
		}

	}
}
