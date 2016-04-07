/*
 * Created by SharpDevelop.
 * User: Wildan Maulana
 * Date: 4/14/2015
 * Time: 3:33 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace com.openthinklabs.alisjk.SmartScan
{
	partial class LoginForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
			this.label_username = new System.Windows.Forms.Label();
			this.label_password = new System.Windows.Forms.Label();
			this.txt_username = new System.Windows.Forms.TextBox();
			this.txt_password = new System.Windows.Forms.TextBox();
			this.btn_login = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label_username
			// 
			this.label_username.Location = new System.Drawing.Point(49, 43);
			this.label_username.Name = "label_username";
			this.label_username.Size = new System.Drawing.Size(63, 23);
			this.label_username.TabIndex = 0;
			this.label_username.Text = "Username";
			// 
			// label_password
			// 
			this.label_password.Location = new System.Drawing.Point(49, 79);
			this.label_password.Name = "label_password";
			this.label_password.Size = new System.Drawing.Size(63, 23);
			this.label_password.TabIndex = 1;
			this.label_password.Text = "Password";
			// 
			// txt_username
			// 
			this.txt_username.Location = new System.Drawing.Point(130, 43);
			this.txt_username.Name = "txt_username";
			this.txt_username.Size = new System.Drawing.Size(176, 20);
			this.txt_username.TabIndex = 2;
			// 
			// txt_password
			// 
			this.txt_password.Location = new System.Drawing.Point(130, 79);
			this.txt_password.Name = "txt_password";
			this.txt_password.Size = new System.Drawing.Size(176, 20);
			this.txt_password.TabIndex = 3;
			this.txt_password.UseSystemPasswordChar = true;
			// 
			// btn_login
			// 
			this.btn_login.Location = new System.Drawing.Point(230, 122);
			this.btn_login.Name = "btn_login";
			this.btn_login.Size = new System.Drawing.Size(75, 23);
			this.btn_login.TabIndex = 4;
			this.btn_login.Text = "Login";
			this.btn_login.UseVisualStyleBackColor = true;
			this.btn_login.Click += new System.EventHandler(this.Btn_loginClick);
			// 
			// LoginForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(414, 185);
			this.Controls.Add(this.btn_login);
			this.Controls.Add(this.txt_password);
			this.Controls.Add(this.txt_username);
			this.Controls.Add(this.label_password);
			this.Controls.Add(this.label_username);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "LoginForm";
			this.Text = "AlisJK:SmartScan - Login";
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.Button btn_login;
		private System.Windows.Forms.TextBox txt_password;
		private System.Windows.Forms.TextBox txt_username;
		private System.Windows.Forms.Label label_password;
		private System.Windows.Forms.Label label_username;
	}
}
