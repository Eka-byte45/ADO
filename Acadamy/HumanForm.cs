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
	public abstract partial class HumanForm : Form
	{
		//static protected DBtools.Connector connector;
		public HumanForm()
		{
			InitializeComponent();
			//connector = new DBtools.Connector(ConfigurationManager.ConnectionStrings["PV_521_Import"].ConnectionString);
		}

		protected abstract void buttonOk_Click(object sender, EventArgs e);
		
	}
}
