/*
 * Created by SharpDevelop.
 * User: Wildan Maulana
 * Date: 4/21/2015
 * Time: 12:55 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;


namespace com.openthinklabs.alisjk.SmartScan
{
	/// <summary>
	/// Description of Class1.
	/// </summary>
	public class InputBox
	{
		public static DialogResult Show(string title, string promptText, ref string value)
		{
			return Show(title, promptText, ref value, null);
		}

		public static DialogResult Show(string title, string promptText, ref string value, InputBoxValidation validation)
		{
			Form form = new Form();
			Label label = new Label();
			Label label_template = new Label();
			TextBox textBox = new TextBox();
			ComboBox combobox = new ComboBox();
			combobox.DropDownStyle = ComboBoxStyle.DropDownList;
			Button buttonOk = new Button();
			Button buttonCancel = new Button();

			form.Text = title;
			label.Text = promptText;
            label_template.Text = "Template LJK";			
			textBox.Text = value;
			combobox.Items.Add("Berbahasa Indonesia");
			combobox.Items.Add("Berbahasa Inggris");
			combobox.SelectedItem = "Berbahasa Indonesia";
			

			buttonOk.Text = "OK";
			buttonCancel.Text = "Batal";
			buttonOk.DialogResult = DialogResult.OK;
			buttonCancel.DialogResult = DialogResult.Cancel;

			label.SetBounds(9, 20, 372, 13);
			label_template.SetBounds(9, 65, 372, 13);
			textBox.SetBounds(12, 36, 300, 20);
			combobox.SetBounds(12, 90, 210, 20);
			buttonOk.SetBounds(228, 125, 75, 23);
			buttonCancel.SetBounds(309, 125, 75, 23);

			label.AutoSize = true;
			label_template.AutoSize = true;
			textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
			buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

			form.ClientSize = new Size(396, 167);
			form.Controls.AddRange(new Control[] { label, textBox, label_template, combobox, buttonOk, buttonCancel });
			form.ClientSize = new Size(Math.Max(300,label.Right+10), form.ClientSize.Height);
			form.FormBorderStyle = FormBorderStyle.FixedDialog;
			form.StartPosition = FormStartPosition.CenterScreen;
			form.MinimizeBox = false;
			form.MaximizeBox = false;
			form.AcceptButton = buttonOk;
			form.CancelButton = buttonCancel;
			if (validation != null) {
				form.FormClosing += delegate(object sender, FormClosingEventArgs e) {
					if (form.DialogResult == DialogResult.OK) {
						string errorText = validation(textBox.Text);
						if (e.Cancel = (errorText != "")) {
							MessageBox.Show(form, errorText, "Validation Error",
							                MessageBoxButtons.OK, MessageBoxIcon.Error);
							textBox.Focus();
						}
					}
				};
			}
			DialogResult dialogResult = form.ShowDialog();
			string batch = textBox.Text.Replace(":","");
			value = batch+":"+combobox.Text;
			return dialogResult;
		}
	}
	
   public delegate string InputBoxValidation(string errorMessage);	
}
