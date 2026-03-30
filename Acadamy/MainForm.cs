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
		Query[] queries =
		{
			new Query
				(
				"last_name,first_name,middle_name, group_name,direction_name",
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
		string[] status_messages = 
			{
			"Количество студентов", 
			"Количество групп",
			"Количество направлений",
			"Количество дисциплин", 
			"Количество преподавателей"
		};
		DataGridView[] tables;
		DBtools.Connector connector;
		public MainForm()
		{
			InitializeComponent();
			tables = new DataGridView[] { dgvStudents, dgvGroups, dgvDirections, dgvDisciplines, dgvTeachers };
			connector = new DBtools.Connector(ConfigurationManager.ConnectionStrings["PV_521_Import"].ConnectionString);
			//dgvDirections.DataSource = connector.Select("*","Directions");
			//toolStripStatusLabel.Text = $"Количество направлений обучения: {dgvDirections.Rows.Count-1}";
			//toolStripStatusLabel.Text = $"Количество направлений обучения: {connector.Scalar("SELECT COUNT(*) FROM Directions")}";
			tabControl_SelectedIndexChanged(tabControl, null);

			LoadDirectionsToComboBox();
			LoadDirectionsForDisciplineFilter();
			LoadDisciplinesToComboBox();
			LoadDirectionsForStudentFilter();
		}

		private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
		{
			int i = tabControl.SelectedIndex;
			//tables[i].DataSource = connector.Select($"SELECT * FROM {tabControl.SelectedTab.Text}");
			tables[i].DataSource = connector.Select(queries[i].ToString());
			//toolStripStatusLabel.Text = $"Количество записей: {tables[i].RowCount - 1}";
			toolStripStatusLabel.Text = $"{status_messages[i]}: {tables[i].RowCount - 1}";
		}
		private void LoadDirectionsToComboBox()
		{
			DataTable directions = connector.Select("direction_id, direction_name", "Directions");//объект DataTable  directions
			DataRow allRow = directions.NewRow();//создаем новую строку со структурой что и directions
			allRow["direction_id"] = 0;//id в строке новой будет 0
			allRow["direction_name"] = "(Все направления)";//название первой строки будет "Все направления"

			directions.Rows.InsertAt(allRow, 0);//вставка новой строки в таблицу directions на нулевой индекс

			cbDisciplines.DisplayMember = "direction_name";//в элементе умправления combo box будет отображаться название направления
			cbDisciplines.ValueMember = "direction_id";//для внутреннего значения, выбранного элемента combo box будет использоваться direction_id

			cbDisciplines.DataSource = directions;//для combo box источником данных будет таблица directions

			cbDisciplines.SelectedIndex = 0;
		}

		private void btnFilterGroups_Click(object sender, EventArgs e)
		{
			DataRowView selectedRow = (DataRowView)cbDisciplines.SelectedItem;//выбранный элемент из combo box имеет тип данных DataRowView
			int directionId = Convert.ToInt32(selectedRow["direction_id"]);//из столбца direction_id извлекается значение и преобразуется к int

			if (directionId == 0)//если выбрали "Все направления",то  отразятся все группы без фильтрации
			{
				dgvGroups.DataSource = connector.Select(queries[1].ToString());
			}
			else
			{
				dgvGroups.DataSource = connector.Select("*", "Groups", $"direction = {directionId}");
				
			}

			toolStripStatusLabel.Text = $"{status_messages[1]}: {dgvGroups.RowCount - 1}";
		}

		private void LoadDirectionsForDisciplineFilter()
		{
			DataTable directions = connector.Select("direction_id, direction_name", "Directions");

			DataRow allRow = directions.NewRow();
			allRow["direction_id"] = 0;
			allRow["direction_name"] = "(Все направления)";
			directions.Rows.InsertAt(allRow, 0);

			cbDirections.DisplayMember = "direction_name";
			cbDirections.ValueMember = "direction_id";
			cbDirections.DataSource = directions;
			cbDirections.SelectedIndex = 0;

		}

		private void btnFilterDirections_Click(object sender, EventArgs e)
		{
			DataRowView selectedRow = (DataRowView)cbDirections.SelectedItem;
			int directionId = Convert.ToInt32(selectedRow["direction_id"]);

			if (directionId == 0)
			{
				dgvDisciplines.DataSource = connector.Select("*", "Disciplines");
			}
			else
			{
			
				dgvDisciplines.DataSource = connector.Select("*", "Disciplines", $"discipline_id IN (SELECT discipline FROM DisciplinesDirectionsRelation WHERE direction = {directionId})");
			}

			toolStripStatusLabel.Text = $"{status_messages[3]}: {dgvDisciplines.RowCount - 1}";
		}

		private void LoadDisciplinesToComboBox()
		{
			DataTable disciplines = connector.Select("discipline_id, discipline_name", "Disciplines");
			DataRow allRow = disciplines.NewRow();
			allRow["discipline_id"] = 0;
			allRow["discipline_name"] = "(Все дисциплины)";
			disciplines.Rows.InsertAt(allRow, 0);
			cbDiscipline.DisplayMember = "discipline_name";
			cbDiscipline.ValueMember = "discipline_id";
			cbDiscipline.DataSource = disciplines;
			cbDiscipline.SelectedIndex = 0;
		}

		private void btnFilterTeachers_Click(object sender, EventArgs e)
		{
			DataRowView selectedRow = (DataRowView)cbDiscipline.SelectedItem;
			int disciplineId = Convert.ToInt32(selectedRow["discipline_id"]);

			if (disciplineId == 0)
			{
				dgvTeachers.DataSource = connector.Select("*","Teachers");
			}
			else
			{
				dgvTeachers.DataSource = connector.Select("*", "Teachers", $"teacher_id IN (SELECT teacher FROM TeachersDisciplinesRelation WHERE discipline = {disciplineId})");
			}

			toolStripStatusLabel.Text = $"{status_messages[4]}: {dgvTeachers.RowCount - 1}";
		}
		private void LoadDirectionsForStudentFilter()
		{
			DataTable directions = connector.Select("direction_id, direction_name", "Directions");

			DataRow allRow = directions.NewRow();
			allRow["direction_id"] = 0;
			allRow["direction_name"] = "(Все направления)";
			directions.Rows.InsertAt(allRow, 0);

			cbFilterDirection.DisplayMember = "direction_name";
			cbFilterDirection.ValueMember = "direction_id";
			cbFilterDirection.DataSource = directions;
			cbFilterDirection.SelectedIndex = 0;
		}

		private void btnFilterStudents_Click(object sender, EventArgs e)
		{
			int directionId = Convert.ToInt32(cbFilterDirection.SelectedValue);
			int groupId = Convert.ToInt32(cbFilterGroup.SelectedValue);

			if (directionId == 0 && groupId == 0)
			{
				dgvStudents.DataSource = connector.Select("*", "Students");
			}
			else if (groupId != 0)
			{
				dgvStudents.DataSource = connector.Select("*", "Students", $"[group] = {groupId}");
			}
			else
			{
				dgvStudents.DataSource = connector.Select
					("*", "Students",
					$"[group] IN (SELECT group_id FROM Groups WHERE direction = {directionId})"
					);
			}

			toolStripStatusLabel.Text = $"{status_messages[0]}: {dgvStudents.RowCount - 1}";
		}

		private void cbFilterDirection_SelectedIndexChanged(object sender, EventArgs e)
		{
			int directionId = Convert.ToInt32(cbFilterDirection.SelectedValue);
			DataTable groups = new DataTable();

			if (directionId == 0)
			{
				groups = connector.Select("group_id, group_name", "Groups");
			}
			else
			{
				groups = connector.Select("group_id, group_name", "Groups", $"direction = {directionId}");
			}

			DataRow allRow = groups.NewRow();
			allRow["group_id"] = 0;
			allRow["group_name"] = "(Все группы)";
			groups.Rows.InsertAt(allRow, 0);

			cbFilterGroup.DisplayMember = "group_name";
			cbFilterGroup.ValueMember = "group_id";
			cbFilterGroup.DataSource = groups;
			cbFilterGroup.SelectedIndex = 0;
		}
	}
}
