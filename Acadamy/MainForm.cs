using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

namespace Acadamy
{
	public partial class MainForm : Form
	{

		internal Query[] queries =
		{
			new Query
				(
				"stud_id,last_name,first_name,middle_name,group_name,direction_name",
				"Students,Groups,Directions",
				"[group]=group_id AND direction = direction_id"
				),
			new Query
				(
				"*",
				"Groups,Directions",
				"direction = direction_id"
				),
			new Query("*","Directions"),
			new Query("*","Disciplines"),
			new Query("*","Teachers"),
		};
		internal string[] status_messages =
			{
			"Количество студентов",
			"Количество групп",
			"Количество направлений",
			"Количество дисциплин",
			"Количество преподавателей"
		};
		internal DataGridView[] tables;
		//DBtools.Connector connector;
		internal DBtools.Connector connector;
		internal StudentForm studentForm;

		/// ///////////////////////////////////////
		Dictionary<string, int> d_directions;
		Dictionary<string, int> d_groups;
		public MainForm()
		{
			InitializeComponent();
			tables = new DataGridView[] { dgvStudents, dgvGroups, dgvDirections, dgvDisciplines, dgvTeachers };
			connector = new DBtools.Connector(ConfigurationManager.ConnectionStrings["PV_521_Import"].ConnectionString);
			//dgvDirections.DataSource = connector.Select("*","Directions");
			//toolStripStatusLabel.Text = $"Количество направлений обучения: {dgvDirections.Rows.Count-1}";
			//toolStripStatusLabel.Text = $"Количество направлений обучения: {connector.Scalar("SELECT COUNT(*) FROM Directions")}";
			tabControl_SelectedIndexChanged(tabControl, null);
			d_directions = connector.GetDictionary("Directions");
			d_groups = connector.GetDictionary("Groups");

			cbStudentsGroup.Items.AddRange(d_groups.Keys.ToArray());
			cbGroupsDirection.Items.AddRange(d_directions.Keys.ToArray());
			cbStudentsDirection.Items.AddRange(d_directions.Keys.ToArray());

		}

		private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
		{
			int i = tabControl.SelectedIndex;
			tables[i].DataSource = connector.Select(queries[i].ToString());
			toolStripStatusLabel.Text = $"{status_messages[i]}: {tables[i].RowCount - 1}";
		}

		private void cbGroupsDirection_SelectedIndexChanged(object sender, EventArgs e)
		{

			dgvGroups.DataSource = connector.Select(queries[1].ToString() + $" AND direction={d_directions[cbGroupsDirection.SelectedItem.ToString()]}");
			toolStripStatusLabel.Text = $"{status_messages[1]}: {dgvGroups.RowCount - 1}";
		}

		private void cbStudentsDirection_SelectedIndexChanged(object sender, EventArgs e)
		{
			d_groups = connector.GetDictionary
				("Groups",
				$"direction={d_directions[cbStudentsDirection.SelectedItem.ToString()]}"
				);
			cbStudentsGroup.Items.Clear();
			cbStudentsGroup.Items.AddRange(d_groups.Keys.ToArray());
			dgvStudents.DataSource = connector.Select
				(
				queries[0].ToString() +
				$" AND direction={d_directions[cbStudentsDirection.SelectedItem.ToString()]}");
			toolStripStatusLabel.Text = $"{status_messages[0]}: {dgvStudents.RowCount - 1}";
		}

		private void buttonAddStudent_Click(object sender, EventArgs e)
		{
			studentForm = new StudentForm(this.connector);
			studentForm.ShowDialog();
			dgvStudents.DataSource = connector.Select(queries[0].ToString());

			//if (cbStudentsDirection.SelectedItem != null)
			//	cbStudentsDirection_SelectedIndexChanged(null, null);
		}

		private void dgvStudents_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{

			if (e.RowIndex >= 0)
			{
				DataGridViewRow row = dgvStudents.Rows[e.RowIndex];
				int studentId = Convert.ToInt32(row.Cells["stud_id"].Value);

				DataTable studentDataTable = connector.Select($"SELECT * FROM Students WHERE stud_id = {studentId}");
				if (studentDataTable.Rows.Count > 0)
				{
					DataRow dataRow = studentDataTable.Rows[0];
					StudentData student = new StudentData
					{
						LastName = dataRow["last_name"].ToString(),
						FirstName = dataRow["first_name"].ToString(),
						MiddleName = dataRow["middle_name"].ToString(),
						BirthDate = Convert.ToDateTime(dataRow["birth_date"]),
						Email = dataRow["email"].ToString(),
						Phone = dataRow["phone"].ToString(),
						GroupId = Convert.ToInt32(dataRow["group"])
					};

					studentForm = new StudentForm(this.connector);
					studentForm.LoadStudentData(student);
					studentForm.StudentId = studentId;
					studentForm.ShowDialog();

					dgvStudents.DataSource = connector.Select(queries[0].ToString());
					
				}
			}

		}
	}
}
