/* Этот файл является частью примеров использования библиотеки Saraff.Twain.NET
 * © SARAFF SOFTWARE (Кирножицкий Андрей), 2011.
 * Saraff.Twain.NET - свободная программа: вы можете перераспространять ее и/или
 * изменять ее на условиях Меньшей Стандартной общественной лицензии GNU в том виде,
 * в каком она была опубликована Фондом свободного программного обеспечения;
 * либо версии 3 лицензии, либо (по вашему выбору) любой более поздней
 * версии.
 * Saraff.Twain.NET распространяется в надежде, что она будет полезной,
 * но БЕЗО ВСЯКИХ ГАРАНТИЙ; даже без неявной гарантии ТОВАРНОГО ВИДА
 * или ПРИГОДНОСТИ ДЛЯ ОПРЕДЕЛЕННЫХ ЦЕЛЕЙ. Подробнее см. в Меньшей Стандартной
 * общественной лицензии GNU.
 * Вы должны были получить копию Меньшей Стандартной общественной лицензии GNU
 * вместе с этой программой. Если это не так, см.
 * <http://www.gnu.org/licenses/>.)
 * 
 * This file is part of samples of Saraff.Twain.NET.
 * © SARAFF SOFTWARE (Kirnazhytski Andrei), 2011.
 * Saraff.Twain.NET is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * Saraff.Twain.NET is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 * You should have received a copy of the GNU Lesser General Public License
 * along with Saraff.Twain.NET. If not, see <http://www.gnu.org/licenses/>.
 * 
 * PLEASE SEND EMAIL TO:  twain@saraff.ru.
 */
namespace com.openthinklabs.alisjk.SmartScan {
    partial class SmartScan {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components=null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if(disposing&&(components!=null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SmartScan));
            this.toolStripPanel1 = new System.Windows.Forms.ToolStripPanel();
            this._toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.newToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripFolderBrowserDialogButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.dataSourcesToolStripComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.resolutionToolStripDropDownButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.pixelTypesToolStripDropDownButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.xferModeToolStripDropDownButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.fileFormatToolStripDropDownButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.printDialog1 = new System.Windows.Forms.PrintDialog();
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this._twain32 = new Saraff.Twain.Twain32(this.components);
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.toolStripPanel1.SuspendLayout();
            this._toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripPanel1
            // 
            this.toolStripPanel1.Controls.Add(this._toolStrip1);
            this.toolStripPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.toolStripPanel1.Location = new System.Drawing.Point(0, 0);
            this.toolStripPanel1.Name = "toolStripPanel1";
            this.toolStripPanel1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.toolStripPanel1.RowMargin = new System.Windows.Forms.Padding(0);
            this.toolStripPanel1.Size = new System.Drawing.Size(1664, 49);
            this.toolStripPanel1.Click += new System.EventHandler(this.toolStripPanel1_Click);
            // 
            // _toolStrip1
            // 
            this._toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this._toolStrip1.ImageScalingSize = new System.Drawing.Size(40, 40);
            this._toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripButton,
            this.toolStripFolderBrowserDialogButton,
            this.toolStripSeparator1,
            this.dataSourcesToolStripComboBox,
            this.resolutionToolStripDropDownButton,
            this.pixelTypesToolStripDropDownButton,
            this.xferModeToolStripDropDownButton,
            this.fileFormatToolStripDropDownButton});
            this._toolStrip1.Location = new System.Drawing.Point(3, 0);
            this._toolStrip1.Name = "_toolStrip1";
            this._toolStrip1.Size = new System.Drawing.Size(859, 49);
            this._toolStrip1.TabIndex = 0;
            this._toolStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this._toolStrip1ItemClicked);
            // 
            // newToolStripButton
            // 
            this.newToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.newToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("newToolStripButton.Image")));
            this.newToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.newToolStripButton.Name = "newToolStripButton";
            this.newToolStripButton.Size = new System.Drawing.Size(44, 46);
            this.newToolStripButton.Text = "Mulai Scan LJK";
            this.newToolStripButton.Click += new System.EventHandler(this.newToolStripButton_Click);
            // 
            // toolStripFolderBrowserDialogButton
            // 
            this.toolStripFolderBrowserDialogButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripFolderBrowserDialogButton.Image = ((System.Drawing.Image)(resources.GetObject("toolStripFolderBrowserDialogButton.Image")));
            this.toolStripFolderBrowserDialogButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripFolderBrowserDialogButton.Name = "toolStripFolderBrowserDialogButton";
            this.toolStripFolderBrowserDialogButton.Size = new System.Drawing.Size(44, 46);
            this.toolStripFolderBrowserDialogButton.Text = "Pilih Folder Lokasi Menyimpan Hasil Scan";
            this.toolStripFolderBrowserDialogButton.Click += new System.EventHandler(this.toolStripFolderBrowserDialogButtonClick);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 49);
            // 
            // dataSourcesToolStripComboBox
            // 
            this.dataSourcesToolStripComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dataSourcesToolStripComboBox.Name = "dataSourcesToolStripComboBox";
            this.dataSourcesToolStripComboBox.Size = new System.Drawing.Size(200, 49);
            this.dataSourcesToolStripComboBox.SelectedIndexChanged += new System.EventHandler(this.dataSourcesToolStripComboBox_SelectedIndexChanged);
            // 
            // resolutionToolStripDropDownButton
            // 
            this.resolutionToolStripDropDownButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.resolutionToolStripDropDownButton.Image = ((System.Drawing.Image)(resources.GetObject("resolutionToolStripDropDownButton.Image")));
            this.resolutionToolStripDropDownButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.resolutionToolStripDropDownButton.Name = "resolutionToolStripDropDownButton";
            this.resolutionToolStripDropDownButton.Size = new System.Drawing.Size(137, 46);
            this.resolutionToolStripDropDownButton.Tag = "";
            this.resolutionToolStripDropDownButton.Text = "xxx dpi";
            // 
            // pixelTypesToolStripDropDownButton
            // 
            this.pixelTypesToolStripDropDownButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.pixelTypesToolStripDropDownButton.Image = ((System.Drawing.Image)(resources.GetObject("pixelTypesToolStripDropDownButton.Image")));
            this.pixelTypesToolStripDropDownButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.pixelTypesToolStripDropDownButton.Name = "pixelTypesToolStripDropDownButton";
            this.pixelTypesToolStripDropDownButton.Size = new System.Drawing.Size(100, 46);
            this.pixelTypesToolStripDropDownButton.Text = "RGB";
            // 
            // xferModeToolStripDropDownButton
            // 
            this.xferModeToolStripDropDownButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.xferModeToolStripDropDownButton.Image = ((System.Drawing.Image)(resources.GetObject("xferModeToolStripDropDownButton.Image")));
            this.xferModeToolStripDropDownButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.xferModeToolStripDropDownButton.Name = "xferModeToolStripDropDownButton";
            this.xferModeToolStripDropDownButton.Size = new System.Drawing.Size(176, 46);
            this.xferModeToolStripDropDownButton.Text = "XferMode";
            this.xferModeToolStripDropDownButton.Click += new System.EventHandler(this.XferModeToolStripDropDownButtonClick);
            // 
            // fileFormatToolStripDropDownButton
            // 
            this.fileFormatToolStripDropDownButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.fileFormatToolStripDropDownButton.Enabled = false;
            this.fileFormatToolStripDropDownButton.Image = ((System.Drawing.Image)(resources.GetObject("fileFormatToolStripDropDownButton.Image")));
            this.fileFormatToolStripDropDownButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.fileFormatToolStripDropDownButton.Name = "fileFormatToolStripDropDownButton";
            this.fileFormatToolStripDropDownButton.Size = new System.Drawing.Size(138, 46);
            this.fileFormatToolStripDropDownButton.Text = "Format";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 49);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1364, 656);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "bmp";
            this.saveFileDialog1.Filter = "Gambar|*.bmp";
            this.saveFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.SaveFileDialog1FileOk);
            // 
            // printDialog1
            // 
            this.printDialog1.Document = this.printDocument1;
            this.printDialog1.UseEXDialog = true;
            // 
            // printDocument1
            // 
            this.printDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocument1_PrintPage);
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.Black;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.textBox1.ForeColor = System.Drawing.Color.Lime;
            this.textBox1.Location = new System.Drawing.Point(0, 705);
            this.textBox1.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.textBox1.MinimumSize = new System.Drawing.Size(1664, 303);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox1.Size = new System.Drawing.Size(1664, 303);
            this.textBox1.TabIndex = 3;
            this.textBox1.WordWrap = false;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(40, 40);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 1008);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(3, 0, 37, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1664, 46);
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(297, 41);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _twain32
            // 
            this._twain32.AppProductName = "Saraff.Twain";
            this._twain32.IsTwain2Enable = true;
            this._twain32.Parent = this;
            this._twain32.ShowUI = false;
            this._twain32.AcquireCompleted += new System.EventHandler(this._twain32_AcquireCompleted);
            this._twain32.AcquireError += new System.EventHandler<Saraff.Twain.Twain32.AcquireErrorEventArgs>(this._twain32_AcquireError);
            this._twain32.EndXfer += new System.EventHandler<Saraff.Twain.Twain32.EndXferEventArgs>(this._twain32_EndXfer);
            this._twain32.XferDone += new System.EventHandler<Saraff.Twain.Twain32.XferDoneEventArgs>(this._twain32_XferDone);
            this._twain32.SetupMemXferEvent += new System.EventHandler<Saraff.Twain.Twain32.SetupMemXferEventArgs>(this._twain32_SetupMemXferEvent);
            this._twain32.MemXferEvent += new System.EventHandler<Saraff.Twain.Twain32.MemXferEventArgs>(this._twain32_MemXferEvent);
            this._twain32.SetupFileXferEvent += new System.EventHandler<Saraff.Twain.Twain32.SetupFileXferEventArgs>(this._twain32_SetupFileXferEvent);
            this._twain32.FileXferEvent += new System.EventHandler<Saraff.Twain.Twain32.FileXferEventArgs>(this._twain32_FileXferEvent);
            this._twain32.DeviceEvent += new System.EventHandler<Saraff.Twain.Twain32.DeviceEventEventArgs>(this._twain32_DeviceEvent);
            // 
            // textBox2
            // 
            this.textBox2.Dock = System.Windows.Forms.DockStyle.Right;
            this.textBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox2.Location = new System.Drawing.Point(1364, 49);
            this.textBox2.MinimumSize = new System.Drawing.Size(200, 100);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(300, 113);
            this.textBox2.TabIndex = 8;
            this.textBox2.Text = "0";
            this.textBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // SmartScan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1664, 1054);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStripPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.Name = "SmartScan";
            this.Text = "AlisJK : SmartScan";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form1Load);
            this.toolStripPanel1.ResumeLayout(false);
            this.toolStripPanel1.PerformLayout();
            this._toolStrip1.ResumeLayout(false);
            this._toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        private System.Windows.Forms.ToolStripButton toolStripFolderBrowserDialogButton;

        #endregion

        private Saraff.Twain.Twain32 _twain32;
        private System.Windows.Forms.ToolStripPanel toolStripPanel1;
        private System.Windows.Forms.ToolStrip _toolStrip1;
        private System.Windows.Forms.ToolStripButton newToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripComboBox dataSourcesToolStripComboBox;
        private System.Windows.Forms.ToolStripDropDownButton resolutionToolStripDropDownButton;
        private System.Windows.Forms.ToolStripDropDownButton pixelTypesToolStripDropDownButton;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.PrintDialog printDialog1;
        private System.Drawing.Printing.PrintDocument printDocument1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ToolStripDropDownButton xferModeToolStripDropDownButton;
        private System.Windows.Forms.ToolStripDropDownButton fileFormatToolStripDropDownButton;
        
        void XferModeToolStripDropDownButtonClick(object sender, System.EventArgs e)
        {
        	
        }
        
        void _toolStrip1ItemClicked(object sender, System.Windows.Forms.ToolStripItemClickedEventArgs e)
        {
        	
        }

        private System.Windows.Forms.TextBox textBox2;
    }
}

