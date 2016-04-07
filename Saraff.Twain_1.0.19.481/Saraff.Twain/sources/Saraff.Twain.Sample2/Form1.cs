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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Runtime.InteropServices;
using Saraff.Tiff;
using Saraff.Tiff.Core;
using System.Reflection;

namespace Saraff.Twain.Sample2 {

    public partial class Form1:Form {
        private bool _isExtImageInfoAllowed;

        public Form1() {
            InitializeComponent();

            try {
                this._twain32.OpenDSM();

                #region Заполняем список источников данных

                this.dataSourcesToolStripComboBox.Items.Clear();
                for(int i=0;i<this._twain32.SourcesCount;i++) {
                    this.dataSourcesToolStripComboBox.Items.Add(this._twain32.GetSourceProductName(i));
                }
                if(this._twain32.SourcesCount>0) {
                    this.dataSourcesToolStripComboBox.SelectedIndex=this._twain32.SourceIndex;
                }

                #endregion

            } catch(TwainException ex) {
                MessageBox.Show(ex.Message,"SAMPLE2",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void _ResolutionItemSelected(object sender,EventArgs e) {
            try {
                ToolStripItem _item=sender as ToolStripItem;
                if(_item!=null) {
                    this.resolutionToolStripDropDownButton.Text=_item.Text;
                    this.resolutionToolStripDropDownButton.Tag=_item.Tag;
                    this._twain32.Capabilities.XResolution.Set((float)_item.Tag);
                    this._twain32.Capabilities.YResolution.Set((float)_item.Tag);
                }
            } catch(Exception ex) {
                MessageBox.Show(ex.Message,ex.GetType().Name,MessageBoxButtons.OK,MessageBoxIcon.Error);
                Debug.WriteLine(string.Format("{0}: {1}\n{2}",ex.GetType().Name,ex.Message,ex.StackTrace));
            }
        }

        private void _PixelTypeItemSelected(object sender,EventArgs e) {
            try {
                ToolStripItem _item=sender as ToolStripItem;
                if(_item!=null) {
                    this.pixelTypesToolStripDropDownButton.Text=_item.Text;
                    this.pixelTypesToolStripDropDownButton.Tag=_item.Tag;
                    this._twain32.Capabilities.PixelType.Set((TwPixelType)_item.Tag);
                }
            } catch(Exception ex) {
                MessageBox.Show(ex.Message,ex.GetType().Name,MessageBoxButtons.OK,MessageBoxIcon.Error);
                Debug.WriteLine(string.Format("{0}: {1}\n{2}",ex.GetType().Name,ex.Message,ex.StackTrace));
            }
        }

        private void _XferMechItemSelected(object sender,EventArgs e) {
            try {
                ToolStripItem _item=sender as ToolStripItem;
                if(_item!=null) {
                    this.xferModeToolStripDropDownButton.Text=_item.Text;
                    this.xferModeToolStripDropDownButton.Tag=_item.Tag;
                    this._twain32.Capabilities.XferMech.Set((TwSX)_item.Tag);
                    this.fileFormatToolStripDropDownButton.Enabled=(TwSX)_item.Tag==TwSX.File;
                }
            } catch(Exception ex) {
                MessageBox.Show(ex.Message,ex.GetType().Name,MessageBoxButtons.OK,MessageBoxIcon.Error);
                Debug.WriteLine(string.Format("{0}: {1}\n{2}",ex.GetType().Name,ex.Message,ex.StackTrace));
            }
        }

        private void _FileFormatItemSelected(object sender,EventArgs e) {
            try {
                ToolStripItem _item=sender as ToolStripItem;
                if(_item!=null) {
                    this.fileFormatToolStripDropDownButton.Text=_item.Text;
                    this.fileFormatToolStripDropDownButton.Tag=_item.Tag;
                    this._twain32.Capabilities.ImageFileFormat.Set((TwFF)_item.Tag);
                }
            } catch(Exception ex) {
                MessageBox.Show(ex.Message,ex.GetType().Name,MessageBoxButtons.OK,MessageBoxIcon.Error);
                Debug.WriteLine(string.Format("{0}: {1}\n{2}",ex.GetType().Name,ex.Message,ex.StackTrace));
            }
        }

        private Bitmap CurrentBitmap {
            get {
                return this.pictureBox1.Image as Bitmap;
            }
            set {
                if(this.pictureBox1.Image!=null) {
                    this.pictureBox1.Image.Dispose();
                }
                this.pictureBox1.Image=value;
            }
        }

        #region Twain32 events handlers

        /// <summary>
        /// Occurs when acquire completed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _twain32_AcquireCompleted(object sender,EventArgs e) {
            try {
                if(this._twain32.ImageCount>0) {
                    this.CurrentBitmap=this._twain32.GetImage(0) as Bitmap;
                }

                #region Tiff-file

                if(TiffMemXfer.Current!=null) {
                    try {
                        TiffMemXfer.Current.Writer.BaseStream.Seek(0,SeekOrigin.Begin);
                        this.CurrentBitmap=Image.FromStream(TiffMemXfer.Current.Writer.BaseStream) as Bitmap;
                    } finally {
                        TiffMemXfer.Dispose();
                    }
                }

                #endregion

                #region MemFile

                if(MemFileXfer.Current!=null) {
                    try {
                        MemFileXfer.Current.Writer.BaseStream.Seek(0,SeekOrigin.Begin);
                        this.CurrentBitmap=Image.FromStream(MemFileXfer.Current.Writer.BaseStream) as Bitmap;
                    } finally {
                        MemFileXfer.Dispose();
                    }
                }

                #endregion

            } catch(Exception ex) {
                MessageBox.Show(ex.Message,ex.GetType().Name,MessageBoxButtons.OK,MessageBoxIcon.Error);
                Debug.WriteLine(string.Format("{0}: {1}\n{2}",ex.GetType().Name,ex.Message,ex.StackTrace));
            }
        }

        private void _twain32_AcquireError(object sender,Twain32.AcquireErrorEventArgs e) {
            try {
                this.textBox1.AppendText(string.Format("{4}{0}: {1} ConditionCode = {2}; ReturnCode = {3};{4}",e.Exception.GetType().Name,e.Exception.Message,e.Exception.ConditionCode,e.Exception.ReturnCode,Environment.NewLine));
                Debug.WriteLine(string.Format(string.Format("{4}{0}: {1}{4}{5}{4} ConditionCode = {2}; ReturnCode = {3};",e.Exception.GetType().Name,e.Exception.Message,e.Exception.ConditionCode,e.Exception.ReturnCode,Environment.NewLine,e.Exception.StackTrace)));
                for(Exception _ex=e.Exception.InnerException; _ex!=null; _ex=_ex.InnerException) {
                    this.textBox1.AppendText(string.Format("{2}{0}: {1} {2}",_ex.GetType().Name,_ex.Message,Environment.NewLine));
                    Debug.WriteLine(string.Format(string.Format("{2}{0}: {1}{2}{3}",_ex.GetType().Name,_ex.Message,Environment.NewLine,_ex.StackTrace)));
                }
            } catch(Exception ex) {
                MessageBox.Show(ex.Message,ex.GetType().Name,MessageBoxButtons.OK,MessageBoxIcon.Error);
                Debug.WriteLine(string.Format("{0}: {1}\n{2}",ex.GetType().Name,ex.Message,ex.StackTrace));
            }
        }

        /// <summary>
        /// Occurs at the end of every transfer when the application has received all the data it expected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _twain32_EndXfer(object sender,Twain32.EndXferEventArgs e) {
            try {
                if(e.Image!=null) {
                    System.Diagnostics.Debug.WriteLine(string.Format("Получено изображение размером {0}x{1}",e.Image.Width,e.Image.Height));
                }
            } catch(Exception ex) {
                MessageBox.Show(ex.Message,ex.GetType().Name,MessageBoxButtons.OK,MessageBoxIcon.Error);
                Debug.WriteLine(string.Format("{0}: {1}\n{2}",ex.GetType().Name,ex.Message,ex.StackTrace));
            }
        }

        /// <summary>
        /// Occurs when the image is completely transferred.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _twain32_XferDone(object sender,Twain32.XferDoneEventArgs e) {
            try {
                this.textBox1.ResetText();

                #region Image info

                this.textBox1.AppendText(string.Format("Image Info:{0}",Environment.NewLine));
                var _imageInfo=e.GetImageInfo();
                this.textBox1.AppendText(string.Format("YResolution={0}; XResolution={1}; Compression={7} PixelType={2}; SamplesPerPixel={3}; ImageWidth={4}; ImageLength={5};{6}",_imageInfo.YResolution,_imageInfo.XResolution,_imageInfo.PixelType,_imageInfo.BitsPerSample.Length,_imageInfo.ImageWidth,_imageInfo.ImageLength,Environment.NewLine,_imageInfo.Compression));

                #endregion

                #region Extended Image Info

                if(this._isExtImageInfoAllowed) { // if extended image info support
                    this.textBox1.AppendText("Extended Image Info:");
                    var _extImageInfo=e.GetExtImageInfo(new TwEI[] { TwEI.BarCodeCount,TwEI.BarCodeType,TwEI.BarCodeTextLength,TwEI.Camera,TwEI.Frame,TwEI.PixelFlavor,TwEI.DocumentNumber });

                    #region Show info

                    foreach(var _item in _extImageInfo) {
                        this.textBox1.AppendText(string.Format("{0}{1} = ",Environment.NewLine,_item.InfoId));
                        if(_item.IsSuccess) {
                            if(_item.Value is object[]) {
                                foreach(var _value in _item.Value as object[]) {
                                    this.textBox1.AppendText(string.Format("{0}; ",_value));
                                }
                            } else {
                                this.textBox1.AppendText(string.Format("{0}; ",_item.Value));
                            }
                        }
                        if(_item.IsNotAvailable) {
                            this.textBox1.AppendText("NotAvailable");
                        }
                        if(_item.IsNotSupported) {
                            this.textBox1.AppendText("NotSupported");
                        }
                    }

                }

                #endregion

                #endregion

                #region Tiff-file

                if(TiffMemXfer.Current!=null&&TiffMemXfer.Current.Writer!=null) {
                    Twain32.ImageInfo _info=e.GetImageInfo();
                    Collection<ITag> _tags=new Collection<ITag> {
                        Tag<uint>.Create(TiffTags.ImageWidth,(uint)_info.ImageWidth),
                        Tag<uint>.Create(TiffTags.ImageLength,(uint)_info.ImageLength),
                        Tag<ushort>.Create(TiffTags.SamplesPerPixel,(ushort)_info.BitsPerSample.Length),
                        Tag<ushort>.Create(TiffTags.BitsPerSample,new Func<short[],ushort[]>(val=>{
                            ushort[] _result=new ushort[_info.BitsPerSample.Length];
                            for(int i=0; i<_result.Length; i++) {
                                _result[i]=(ushort)_info.BitsPerSample[i];
                            }
                            return _result;
                        })(_info.BitsPerSample)),
                        Tag<ushort>.Create(TiffTags.Orientation,(ushort)TiffOrientation.TOPLEFT),
                        Tag<TiffCompression>.Create(TiffTags.Compression,TiffCompression.NONE),
                        Tag<TiffResolutionUnit>.Create(TiffTags.ResolutionUnit,new Func<TwUnits,TiffResolutionUnit>(val=>{
                            switch(val){
                                case TwUnits.Centimeters:
                                    return TiffResolutionUnit.CENTIMETER;
                                case TwUnits.Inches:
                                    return TiffResolutionUnit.INCH;
                                default:
                                    return TiffResolutionUnit.NONE;
                            }
                        })(this._twain32.Capabilities.Units.GetCurrent())),
                        Tag<ulong>.Create(TiffTags.XResolution,(1UL<<32)|(ulong)_info.XResolution),
                        Tag<ulong>.Create(TiffTags.YResolution,(1UL<<32)|(ulong)_info.YResolution),
                        Tag<TiffHandle>.Create(TiffTags.StripOffsets,TiffMemXfer.Current.Strips.ToArray()),
                        Tag<uint>.Create(TiffTags.StripByteCounts,TiffMemXfer.Current.StripByteCounts.ToArray()),
                        Tag<uint>.Create(TiffTags.RowsPerStrip,1u),
                        Tag<char>.Create(TiffTags.Software,Application.ProductName.ToCharArray()),
                        Tag<char>.Create(TiffTags.Model,this._twain32.GetSourceProductName(this._twain32.SourceIndex).ToCharArray()),
                        Tag<char>.Create(TiffTags.DateTime,DateTime.Now.ToString("yyyy:MM:dd HH:mm:ss").PadRight(20,'\0').ToCharArray()),
                        Tag<char>.Create(TiffTags.HostComputer,Environment.MachineName.ToCharArray()),
                        Tag<ushort>.Create(TiffTags.PlanarConfiguration,(ushort)TiffPlanarConfig.CONTIG),
                        Tag<char>.Create(TiffTags.Copyright,((AssemblyCopyrightAttribute)this.GetType().Assembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute),false)[0]).Copyright.ToCharArray())
                    };
                    switch(_info.PixelType) {
                        case TwPixelType.BW:
                            _tags.Add(Tag<TiffPhotoMetric>.Create(TiffTags.PhotometricInterpretation,TiffPhotoMetric.BlackIsZero));
                            break;
                        case TwPixelType.Gray:
                            _tags.Add(Tag<TiffPhotoMetric>.Create(TiffTags.PhotometricInterpretation,TiffPhotoMetric.BlackIsZero));
                            break;
                        case TwPixelType.Palette:
                            _tags.Add(Tag<TiffPhotoMetric>.Create(TiffTags.PhotometricInterpretation,TiffPhotoMetric.Palette));
                            _tags.Add(Tag<ushort>.Create(TiffTags.ColorMap,TiffMemXfer.Current.ColorMap));
                            break;
                        case TwPixelType.RGB:
                            _tags.Add(Tag<TiffPhotoMetric>.Create(TiffTags.PhotometricInterpretation,TiffPhotoMetric.RGB));
                            break;
                        default:
                            break;
                    }
                    TiffMemXfer.Current.Handle=TiffMemXfer.Current.Writer.WriteImageFileDirectory(TiffMemXfer.Current.Handle,_tags);
                    TiffMemXfer.Current.Strips.Clear();
                    TiffMemXfer.Current.StripByteCounts.Clear();
                }

                #endregion

            } catch(Exception ex) {
                this.textBox1.AppendText(string.Format("{0}: {1}",ex.GetType().Name,ex.Message));
                Debug.WriteLine(string.Format("{0}: {1}\n{2}",ex.GetType().Name,ex.Message,ex.StackTrace));
            }
        }

        private void _twain32_SetupMemXferEvent(object sender,Twain32.SetupMemXferEventArgs e) {
            try {
                if(this._twain32.Capabilities.XferMech.GetCurrent()==TwSX.Memory) {
                    if(TiffMemXfer.Current==null) {
                        TiffMemXfer.Create((int)e.BufferSize);
                    }

                    #region Color Map

                    if(e.ImageInfo.PixelType==TwPixelType.Palette) {
                        Twain32.ColorPalette _palette=this._twain32.Palette.Get();
                        TiffMemXfer.Current.ColorMap=new ushort[_palette.Colors.Length*3];
                        for(int i=0; i<_palette.Colors.Length; i++) {
                            TiffMemXfer.Current.ColorMap[i]=(ushort)(_palette.Colors[i].R);
                            TiffMemXfer.Current.ColorMap[i+_palette.Colors.Length]=(ushort)(_palette.Colors[i].G);
                            TiffMemXfer.Current.ColorMap[i+(_palette.Colors.Length<<1)]=(ushort)(_palette.Colors[i].B);
                        }
                    }

                    #endregion

                } else { // MemFile
                    MemFileXfer.Create((int)e.BufferSize,this._twain32.Capabilities.ImageFileFormat.GetCurrent().ToString().ToLower());
                }
            } catch(Exception ex) {
                MessageBox.Show(ex.Message,ex.GetType().Name,MessageBoxButtons.OK,MessageBoxIcon.Error);
                Debug.WriteLine(string.Format("{0}: {1}\n{2}",ex.GetType().Name,ex.Message,ex.StackTrace));
            }
        }

        private void _twain32_MemXferEvent(object sender,Twain32.MemXferEventArgs e) {
            try {
                if(TiffMemXfer.Current!=null) {
                    long _bitsPerRow=e.ImageInfo.BitsPerPixel*e.ImageMemXfer.Columns;
                    long _bytesPerRow=Math.Min(e.ImageMemXfer.BytesPerRow,(_bitsPerRow>>3)+((_bitsPerRow&0x07)!=0?1:0));
                    using(MemoryStream _stream=new MemoryStream()) {
                        for(int i=0; i<e.ImageMemXfer.Rows; i++) {
                            _stream.Position=0;
                            _stream.Write(e.ImageMemXfer.ImageData,(int)(e.ImageMemXfer.BytesPerRow*i),(int)_bytesPerRow);
                            TiffMemXfer.Current.Strips.Add(TiffMemXfer.Current.Writer.WriteData(_stream.ToArray()));
                            TiffMemXfer.Current.StripByteCounts.Add((uint)_stream.Length);
                        }
                    }
                } else {
                    MemFileXfer.Current.Writer.Write(e.ImageMemXfer.ImageData);
                }
            } catch(Exception ex) {
                MessageBox.Show(ex.Message,ex.GetType().Name,MessageBoxButtons.OK,MessageBoxIcon.Error);
                Debug.WriteLine(string.Format("{0}: {1}\n{2}",ex.GetType().Name,ex.Message,ex.StackTrace));
            }
        }

        private void _twain32_SetupFileXferEvent(object sender,Twain32.SetupFileXferEventArgs e) {
            try {
                e.FileName=string.Format(@"FileXferTransfer_{0}.{1}",DateTime.Now.ToString("yyyyMMddHHmmss"),this._twain32.Capabilities.ImageFileFormat.GetCurrent().ToString().ToLower());
            } catch(Exception ex) {
                MessageBox.Show(ex.Message,ex.GetType().Name,MessageBoxButtons.OK,MessageBoxIcon.Error);
                Debug.WriteLine(string.Format("{0}: {1}\n{2}",ex.GetType().Name,ex.Message,ex.StackTrace));
            }
        }

        private void _twain32_FileXferEvent(object sender,Twain32.FileXferEventArgs e) {
            try {
                this.CurrentBitmap=Image.FromFile(e.ImageFileXfer.FileName) as Bitmap;
            } catch(Exception ex) {
                MessageBox.Show(ex.Message,ex.GetType().Name,MessageBoxButtons.OK,MessageBoxIcon.Error);
                Debug.WriteLine(string.Format("{0}: {1}\n{2}",ex.GetType().Name,ex.Message,ex.StackTrace));
            }
        }

        private void _twain32_DeviceEvent(object sender,Twain32.DeviceEventEventArgs e) {
            try {
                this.textBox1.AppendText(string.Format("{2}{0}: {1};{2}",e.DeviceName,e.Event,Environment.NewLine));
            } catch(Exception ex) {
                MessageBox.Show(ex.Message,ex.GetType().Name,MessageBoxButtons.OK,MessageBoxIcon.Error);
                Debug.WriteLine(string.Format("{0}: {1}\n{2}",ex.GetType().Name,ex.Message,ex.StackTrace));
            }
        }

        #endregion

        #region Toolbar events handlers

        private void dataSourcesToolStripComboBox_SelectedIndexChanged(object sender,EventArgs e) {
            try {
                this._twain32.CloseDataSource();
                this._twain32.SourceIndex=this.dataSourcesToolStripComboBox.SelectedIndex;
                this._twain32.OpenDataSource();

                #region Заполняем список доступных разрешений dpi

                this.resolutionToolStripDropDownButton.DropDownItems.Clear();
                Twain32.Enumeration _resolutions=this._twain32.Capabilities.XResolution.Get();
                if(_resolutions.Count<20) {
                    for(int i=0; i<_resolutions.Count; i++) {
                        this.resolutionToolStripDropDownButton.DropDownItems.Add(
                            string.Format("{0:N0} dpi",_resolutions[i]),
                            null,
                            _ResolutionItemSelected).Tag=_resolutions[i];
                    }
                    this._ResolutionItemSelected(this.resolutionToolStripDropDownButton.DropDownItems[_resolutions.CurrentIndex],new EventArgs());
                } else {
                    foreach(var _val in new float[] { 100f,200f,300f,600f,1200f,2400f }) {
                        for(int i=_resolutions.Count-1; i>=0; i--) {
                            if(Math.Abs((float)_resolutions[i]-_val)<50f) {
                                var _item=this.resolutionToolStripDropDownButton.DropDownItems.Add(
                                    string.Format("{0:N0} dpi",_val),
                                    null,
                                    _ResolutionItemSelected);
                                _item.Tag=_resolutions[i];
                                if(i==_resolutions.CurrentIndex) {
                                    this._ResolutionItemSelected(_item,new EventArgs());
                                }
                                break;
                            }
                        }
                    }
                }

                #endregion

                #region Заполняем список доступных типов пикселей

                this.pixelTypesToolStripDropDownButton.DropDownItems.Clear();
                Twain32.Enumeration _pixelTypes=this._twain32.Capabilities.PixelType.Get();
                for(int i=0; i<_pixelTypes.Count; i++) {
                    this.pixelTypesToolStripDropDownButton.DropDownItems.Add(
                        _pixelTypes[i].ToString(),
                        null,
                        _PixelTypeItemSelected).Tag=_pixelTypes[i];
                }
                this._PixelTypeItemSelected(this.pixelTypesToolStripDropDownButton.DropDownItems[_pixelTypes.CurrentIndex],new EventArgs());

                #endregion

                #region Заполняем список доступных режимов передачи данных

                this.xferModeToolStripDropDownButton.DropDownItems.Clear();
                Twain32.Enumeration _xferMech=this._twain32.Capabilities.XferMech.Get();
                for(int i=0; i<_xferMech.Count; i++) {
                    this.xferModeToolStripDropDownButton.DropDownItems.Add(
                        _xferMech[i].ToString(),
                        null,
                        this._XferMechItemSelected).Tag=_xferMech[i];
                }
                this._XferMechItemSelected(this.xferModeToolStripDropDownButton.DropDownItems[_xferMech.CurrentIndex],new EventArgs());

                #endregion

                #region Заполняет список доступных форматов файла изображения

                this.fileFormatToolStripDropDownButton.DropDownItems.Clear();
                Twain32.Enumeration _fileFormats=this._twain32.Capabilities.ImageFileFormat.Get();
                for(int i=0; i<_fileFormats.Count; i++) {
                    this.fileFormatToolStripDropDownButton.DropDownItems.Add(
                        _fileFormats[i].ToString(),
                        null,
                        this._FileFormatItemSelected).Tag=_fileFormats[i];
                }
                this._FileFormatItemSelected(this.fileFormatToolStripDropDownButton.DropDownItems[_fileFormats.CurrentIndex],new EventArgs());

                #endregion

                #region Проверяем возможность получения информации о изображении

                var _isSupported=(this._twain32.Capabilities.ExtImageInfo.IsSupported()&TwQC.Get)!=0;
                this._isExtImageInfoAllowed=_isSupported&&this._twain32.Capabilities.ExtImageInfo.GetCurrent();
                this.toolStripStatusLabel1.Text=string.Format("Extended image info: {0}supported{1}",_isSupported?"":"not ",_isSupported?this._isExtImageInfoAllowed?", allowed":", not allowed":"");

                #endregion

                this.pictureBox1.Select();
            } catch(TwainException ex) {
                Debug.WriteLine(string.Format("{0}: {1}\nReturnCode = {3}; ConditionCode = {4};\n{2}",ex.GetType().Name,ex.Message,ex.StackTrace,ex.ReturnCode,ex.ConditionCode));
            } catch(Exception ex) {
                MessageBox.Show(ex.Message,ex.GetType().Name,MessageBoxButtons.OK,MessageBoxIcon.Error);
                Debug.WriteLine(string.Format("{0}: {1}\n{2}",ex.GetType().Name,ex.Message,ex.StackTrace));
            }
        }

        private void newToolStripButton_Click(object sender,EventArgs e) {
            try {

                #region Examples of the capabilities

                //Feeder
                if((this._twain32.Capabilities.Duplex.IsSupported()&TwQC.GetCurrent)!=0) {
                    if(this._twain32.Capabilities.Duplex.GetCurrent()!=TwDX.None) {
                        if((this._twain32.Capabilities.FeederEnabled.IsSupported()&TwQC.Set)!=0) {
                            this._twain32.Capabilities.FeederEnabled.Set(true);

                            if((this._twain32.Capabilities.XferCount.IsSupported()&TwQC.Set)!=0) {
                                this._twain32.Capabilities.XferCount.Set(-1);
                            }
                            if((this._twain32.Capabilities.DuplexEnabled.IsSupported()&TwQC.Set)!=0) {
                                this._twain32.Capabilities.DuplexEnabled.Set(true);
                            }
                        }
                    }
                }

                //Brightness
                if((this._twain32.Capabilities.Brightness.IsSupported()&TwQC.Set)!=0) {
                    this._twain32.Capabilities.Brightness.Set(0f); //Allowed Values: -1000f to +1000f; Default Value: 0f;
                }

                //Contrast
                if((this._twain32.Capabilities.Contrast.IsSupported()&TwQC.Set)!=0) {
                    this._twain32.Capabilities.Contrast.Set(0f); //Allowed Values: -1000f to +1000f; Default Value: 0f;
                }

                #endregion

                this._twain32.Acquire();
            } catch(Exception ex) {
                MessageBox.Show(ex.Message,ex.GetType().Name,MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void saveToolStripButton_Click(object sender,EventArgs e) {
            try {
                if(this.saveFileDialog1.ShowDialog()==DialogResult.OK) {
                    this.pictureBox1.Image.Save(this.saveFileDialog1.FileName,ImageFormat.Bmp);
                }
            } catch(Exception ex) {
                MessageBox.Show(ex.Message,ex.GetType().Name,MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void printToolStripButton_Click(object sender,EventArgs e) {
            try {
                if(this.printDialog1.ShowDialog()==DialogResult.OK) {
                    this.printDialog1.Document.Print();
                }
            } catch(Exception ex) {
                MessageBox.Show(ex.Message,ex.GetType().Name,MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void copyToolStripButton_Click(object sender,EventArgs e) {
            try {
                Clipboard.SetImage(this.CurrentBitmap);
            } catch(Exception ex) {
                MessageBox.Show(ex.Message,ex.GetType().Name,MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void pasteToolStripButton_Click(object sender,EventArgs e) {
            try {
                if(Clipboard.ContainsImage()) {
                    this.CurrentBitmap=Clipboard.GetImage() as Bitmap;
                }
            } catch(Exception ex) {
                MessageBox.Show(ex.Message,ex.GetType().Name,MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void printDocument1_PrintPage(object sender,PrintPageEventArgs e) {
            try {
                e.Graphics.DrawImageUnscaled(
                    this.CurrentBitmap,
                    e.PageSettings.Margins.Left,e.PageSettings.Margins.Top);
                e.HasMorePages=false;
            } catch(Exception ex) {
                MessageBox.Show(ex.Message,ex.GetType().Name,MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        #endregion

        private sealed class TiffMemXfer {

            public static void Create(int bufferSize) {
                TiffMemXfer.Current=new TiffMemXfer {
                    Writer=TiffWriter.Create(File.Create(string.Format("MemXferTransfer_{0}.tif",DateTime.Now.ToString("yyyyMMddHHmmss")),bufferSize)),
                    Strips=new List<TiffHandle>(),
                    StripByteCounts=new List<uint>()
                };
                TiffMemXfer.Current.Handle=TiffMemXfer.Current.Writer.WriteHeader();
            }

            public static void Dispose() {
                if(TiffMemXfer.Current!=null) {
                    TiffMemXfer.Current=null;
                }
            }

            public static TiffMemXfer Current {
                get;
                private set;
            }

            public TiffHandle Handle {
                get;
                set;
            }

            public TiffWriter Writer {
                get;
                private set;
            }

            public List<TiffHandle> Strips {
                get;
                private set;
            }

            public List<uint> StripByteCounts {
                get;
                private set;
            }

            public ushort[] ColorMap {
                get;
                set;
            }
        }

        private sealed class MemFileXfer {

            public static void Create(int bufferSize,string ext) {
                if(MemFileXfer.Current!=null&&MemFileXfer.Current.Writer!=null) {
                    MemFileXfer.Current.Writer.BaseStream.Dispose();
                }
                MemFileXfer.Current=new MemFileXfer {
                    Writer=new BinaryWriter(File.Create(string.Format("MemFileXferTransfer_{0}.{1}",DateTime.Now.ToString("yyyyMMddHHmmss"),ext),bufferSize))
                };
            }

            public static void Dispose() {
                if(MemFileXfer.Current==null) {
                    MemFileXfer.Current=null;
                }
            }

            public static MemFileXfer Current {
                get;
                private set;
            }

            public BinaryWriter Writer {
                get;
                private set;
            }
        }

        private delegate TResult Func<T,TResult>(T arg);
    }

    internal sealed class Debug {

        public static void WriteLine(string text) {
            switch(Environment.OSVersion.Platform) {
                case PlatformID.Unix:
                    Console.Error.WriteLine(text);
                    break;
                case PlatformID.MacOSX:
                    throw new NotImplementedException();
                default:
                    System.Diagnostics.Debug.WriteLine(text);
                    break;
            }
        }
    }
}
