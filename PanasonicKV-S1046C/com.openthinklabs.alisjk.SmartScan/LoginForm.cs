/*
 * Created by SharpDevelop.
 * User: Wildan Maulana
 * Date: 4/14/2015
 * Time: 3:33 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;
using System.Text;

namespace com.openthinklabs.alisjk.SmartScan
{
	/// <summary>
	/// Description of LoginForm.
	/// </summary>
	public partial class LoginForm : Form
	{
		public bool logOnSuccessFul  = false;
		public static string username = "";
		
		public LoginForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
				
		void Btn_loginClick(object sender, EventArgs e)
		{
			try
			{
                this.logOnSuccessFul = true;
                LoginForm.username = this.txt_username.Text;
                this.Close();
                /**
			    string myConnection = "datasource=192.168.1.3;port=3306;username=alisjk;password=rahasia";
			    MySqlConnection myConn = new MySqlConnection(myConnection);
			 
			    MySqlCommand SelectCommand = new MySqlCommand("SELECT username, salt, password FROM 20150412pascaganjil.sf_guard_user WHERE username='" + this.txt_username.Text +"' ;", myConn);
			 
			    MySqlDataReader myReader;
			    myConn.Open();
			    myReader = SelectCommand.ExecuteReader();
			    int count = 0;
			    while (myReader.Read()) {
			    	string password = this.CreateSHA1(myReader.GetString(1)+this.txt_password.Text).ToLower();
			    	if(password == myReader.GetString(2)) {
			          count = count + 1;
			    	}
			    }
			 
			    if (count == 1){
			    	this.logOnSuccessFul = true;
                    LoginForm.username = this.txt_username.Text ;			    	
			        this.Close();
			    } else if (count > 1)
			        MessageBox.Show("Duplicate username and password. Access is denied.");
			    else
			        MessageBox.Show("Username and password is incorrect. Please try again.");
			 
			    myConn.Close();
                **/
            }
			catch (Exception ex)
			{
			    MessageBox.Show(ex.Message);
			}			
		}
		
	   /**
	    * @todo : pindahkan ke pustaka tersendiri
	    */ 
       public  string CreateSHA1(string input)
        {
            // Use input string to calculate MD5 hash
            SHA1 sha1 = System.Security.Cryptography.SHA1.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = sha1.ComputeHash(inputBytes);

            // Convert the byte array to hexadecimal string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }
            return sb.ToString();
        }		
	}
}
