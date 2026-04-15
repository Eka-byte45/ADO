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
	public partial class HumanForm : Form
	{
		internal Models.Human human;//Здесь объявляется поле. Это главный объект, с которым работает форма. Форма «знает» об одном конкретном объекте класса Human и отображает его данные на экране.
									//static protected DBtools.Connector connector;
		protected HumanForm()
		{
			InitializeComponent();
			//connector = new DBtools.Connector(ConfigurationManager.ConnectionStrings["PV_521_Import"].ConnectionString);
			
		}
		protected void Extract()// Этот метод берёт данные из объекта human и «вписывает» их в соответствующие поля на форме.
		{
			if (human != null)
			{
				if (human.id != 0) labelID.Text = $"ID:{human.id}";
				tbLastName.Text = human.last_name;
				tbFirstName.Text = human.first_name;
				tbMiddleName.Text = human.middle_name;
				dtpBirthDate.Value = Convert.ToDateTime(human.birth_date);
				tbEmail.Text = human.email;
				tbPhone.Text = human.phone;
			}
		}
		
		protected virtual void buttonOk_Click(object sender, EventArgs e) //Этот метод срабатывает, когда пользователь нажимает кнопку «ОК». Он берёт всё, что пользователь ввёл в поля на форме, и создаёт новый объект Human.
		{ 
			human = new Models.Human
			(
			labelID.Text == "" ? 0: Convert.ToInt32(labelID.Text.Split(':').Last()),
			tbLastName.Text,
			tbFirstName.Text,
			tbMiddleName.Text,
			dtpBirthDate.Value.ToString("yyyy-MM-dd"),
			tbEmail.Text,
			tbPhone.Text,
			pbPhoto.Image
			);
		}

		private void buttonbrowse_Click(object sender, EventArgs e)
		{
			OpenFileDialog dialog = new OpenFileDialog();//Это системный компонент для выбора файла.
			dialog.Filter = "JPG files (*.jpg)|*.jpg|PNG fiels (*.png)|*.png|All image files|*.png;*.jpg|ALL files (*.*)|*.*";//Настраивает фильтр, чтобы в окне отображались только картинки (JPG, PNG).
			if (dialog.ShowDialog() == DialogResult.OK)
				pbPhoto.Image = Image.FromFile(dialog.FileName);//Загружает выбранный файл с диска и превращает его в объект Image, который потом можно сохранить в базу данных.
		}
	}
}
