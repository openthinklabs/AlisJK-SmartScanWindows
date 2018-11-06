/*
 * Created by SharpDevelop.
 * User: Wildan Maulana
 * Date: 4/14/2015
 * Time: 2:44 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace com.openthinklabs.alisjk.SmartScan
{
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
		  LoginForm fLogin = new LoginForm();
		  fLogin.ShowDialog();
		  if(fLogin.logOnSuccessFul)
            {
             //Application.EnableVisualStyles();		  	
             //Application.SetCompatibleTextRenderingDefault(false);
                SmartScan ss = new SmartScan();
              ss.Text   = "AlisJK : SmartScan - "+ LoginForm.username;  
		      Application.Run(ss);
		   }
		  else {
		    Application.Exit();
		  }        	
        }
    }	
}
