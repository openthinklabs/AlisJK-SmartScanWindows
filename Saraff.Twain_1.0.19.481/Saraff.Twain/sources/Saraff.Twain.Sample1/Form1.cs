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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Saraff.Twain.Sample1 {

    public partial class Form1:Form {
        private bool _isEnable=false;

        public Form1() {
            InitializeComponent();
            try {
                this._twain.OpenDSM();
            } catch(Exception ex) {
                MessageBox.Show(string.Format("{0}\n\n{1}",ex.Message,ex.StackTrace),"SAMPLE1",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender,EventArgs e) {
            try {
                if(Environment.OSVersion.Platform==PlatformID.Unix) {
                    using(SelectSourceForm _dlg=new SelectSourceForm {Twain=this._twain}) {
                        if(_dlg.ShowDialog()==System.Windows.Forms.DialogResult.OK) {
                            this._twain.SetDefaultSource(_dlg.SourceIndex);
                            this._twain.SourceIndex=_dlg.SourceIndex;
                        }
                    }
                } else {
                    this._twain.CloseDataSource();
                    this._twain.SelectSource();
                }
            } catch(Exception ex) {
                MessageBox.Show(ex.Message,"SAMPLE1",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender,EventArgs e) {
            try {
                this._twain.Acquire();
            } catch(Exception ex) {
                MessageBox.Show(ex.Message,"SAMPLE1",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void _twain_AcquireCompleted(object sender,EventArgs e) {
            try {
                if(this.pictureBox1.Image!=null) {
                    this.pictureBox1.Image.Dispose();
                }
                if(this._twain.ImageCount>0) {
                    this.pictureBox1.Image=this._twain.GetImage(0);
                }
            } catch(Exception ex) {
                MessageBox.Show(ex.Message,"SAMPLE1",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void _twain_TwainStateChanged(object sender,Twain32.TwainStateEventArgs e) {
            try {
                if((e.TwainState&Twain32.TwainStateFlag.DSEnabled)==0&&this._isEnable) {
                    this._isEnable=false;
                    // <<< scaning finished (or closed)
                }
                this._isEnable=(e.TwainState&Twain32.TwainStateFlag.DSEnabled)!=0;
            } catch(Exception ex) {
                MessageBox.Show(ex.Message,"SAMPLE1",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
    }
}
