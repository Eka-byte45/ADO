namespace Acadamy
{
	partial class AddStudentForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.txtLastName = new System.Windows.Forms.TextBox();
			this.txtFirstName = new System.Windows.Forms.TextBox();
			this.txtMiddleName = new System.Windows.Forms.TextBox();
			this.dtpBirthDate = new System.Windows.Forms.DateTimePicker();
			this.txtEmail = new System.Windows.Forms.TextBox();
			this.txtPhone = new System.Windows.Forms.TextBox();
			this.cbGroup = new System.Windows.Forms.ComboBox();
			this.btnSave = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// txtLastName
			// 
			this.txtLastName.Location = new System.Drawing.Point(45, 36);
			this.txtLastName.Name = "txtLastName";
			this.txtLastName.Size = new System.Drawing.Size(277, 20);
			this.txtLastName.TabIndex = 0;
			// 
			// txtFirstName
			// 
			this.txtFirstName.Location = new System.Drawing.Point(45, 96);
			this.txtFirstName.Name = "txtFirstName";
			this.txtFirstName.Size = new System.Drawing.Size(277, 20);
			this.txtFirstName.TabIndex = 1;
			// 
			// txtMiddleName
			// 
			this.txtMiddleName.Location = new System.Drawing.Point(45, 159);
			this.txtMiddleName.Name = "txtMiddleName";
			this.txtMiddleName.Size = new System.Drawing.Size(277, 20);
			this.txtMiddleName.TabIndex = 2;
			// 
			// dtpBirthDate
			// 
			this.dtpBirthDate.Location = new System.Drawing.Point(45, 226);
			this.dtpBirthDate.Name = "dtpBirthDate";
			this.dtpBirthDate.Size = new System.Drawing.Size(277, 20);
			this.dtpBirthDate.TabIndex = 3;
			// 
			// txtEmail
			// 
			this.txtEmail.Location = new System.Drawing.Point(45, 291);
			this.txtEmail.Name = "txtEmail";
			this.txtEmail.Size = new System.Drawing.Size(277, 20);
			this.txtEmail.TabIndex = 5;
			// 
			// txtPhone
			// 
			this.txtPhone.Location = new System.Drawing.Point(45, 353);
			this.txtPhone.Name = "txtPhone";
			this.txtPhone.Size = new System.Drawing.Size(277, 20);
			this.txtPhone.TabIndex = 6;
			// 
			// cbGroup
			// 
			this.cbGroup.FormattingEnabled = true;
			this.cbGroup.Location = new System.Drawing.Point(45, 434);
			this.cbGroup.Name = "cbGroup";
			this.cbGroup.Size = new System.Drawing.Size(277, 21);
			this.cbGroup.TabIndex = 7;
			// 
			// btnSave
			// 
			this.btnSave.Location = new System.Drawing.Point(141, 521);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(105, 39);
			this.btnSave.TabIndex = 8;
			this.btnSave.Text = "Save";
			this.btnSave.UseVisualStyleBackColor = true;
			// 
			// AddStudentForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(368, 637);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.cbGroup);
			this.Controls.Add(this.txtPhone);
			this.Controls.Add(this.txtEmail);
			this.Controls.Add(this.dtpBirthDate);
			this.Controls.Add(this.txtMiddleName);
			this.Controls.Add(this.txtFirstName);
			this.Controls.Add(this.txtLastName);
			this.Name = "AddStudentForm";
			this.Text = "AddStudentForm";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtLastName;
		private System.Windows.Forms.TextBox txtFirstName;
		private System.Windows.Forms.TextBox txtMiddleName;
		private System.Windows.Forms.DateTimePicker dtpBirthDate;
		private System.Windows.Forms.TextBox txtEmail;
		private System.Windows.Forms.TextBox txtPhone;
		private System.Windows.Forms.ComboBox cbGroup;
		private System.Windows.Forms.Button btnSave;
	}
}