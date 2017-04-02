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
using Saraff.Twain;
using Saraff.Tiff;
using Saraff.Tiff.Core;
using System.Reflection;
using System.Text.RegularExpressions;



namespace com.openthinklabs.alisjk.SmartScan {

	public partial class SmartScan:Form {
		private TwQC _extImageInfoCap;
		private static string root_folder   = "";
		private static string target_folder = "" ;
		private static string nama_batch    = "";		
		private static string template      = "";
        private int counter = 0;

		public SmartScan() {
			InitializeComponent();

			try {
				
				this._twain32.ShowUI         = false;
				this._twain32.IsTwain2Enable = true;
				this._twain32.OpenDSM();								                
				#region isi dalam daftar sumber data

				this.dataSourcesToolStripComboBox.Items.Clear();
				for(int i=0;i<this._twain32.SourcesCount;i++) {
					this.dataSourcesToolStripComboBox.Items.Add(this._twain32.GetSourceProductName(i));
				}
				if(this._twain32.SourcesCount>0) {
					this.dataSourcesToolStripComboBox.SelectedIndex=this._twain32.SourceIndex;
				}

				#endregion    
			} catch(TwainException ex) {
				MessageBox.Show(ex.Message,"SmartScan",MessageBoxButtons.OK,MessageBoxIcon.Error);
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
					//this._twain32.Capabilities.XferMech.Set((TwSX)_item.Tag); //seluruh-nya
					this._twain32.Capabilities.XferMech.Set(TwSX.File); //hanya file
					this.fileFormatToolStripDropDownButton.Enabled=(TwSX)_item.Tag==TwSX.File;
				}
			} catch(Exception ex) {
				//MessageBox.Show(ex.Message,ex.GetType().Name,MessageBoxButtons.OK,MessageBoxIcon.Error);
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
					System.Diagnostics.Debug.WriteLine(string.Format("Ukuran Gambar yang Diambil {0}x{1}",e.Image.Width,e.Image.Height));
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
		///  
		private void _twain32_XferDone(object sender,Twain32.XferDoneEventArgs e) {
			try {				
				this.textBox1.ResetText();

				#region Image info

				
				//this.textBox1.AppendText(string.Format("Image Info:{0}",Environment.NewLine));
				//var _imageInfo=e.GetImageInfo();
				//this.textBox1.AppendText(string.Format("YResolution={0}; XResolution={1}; Compression={7} PixelType={2}; BitsPerSample.Length={3}; ImageWidth={4}; ImageLength={5};{6}",_imageInfo.YResolution,_imageInfo.XResolution,_imageInfo.PixelType,_imageInfo.BitsPerSample.Length,_imageInfo.ImageWidth,_imageInfo.ImageLength,Environment.NewLine,_imageInfo.Compression));

				#endregion

				#region Extended Image Info
                
				if((this._extImageInfoCap&TwQC.Get)!=0) { // if extended image info support				
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
                SmartScan.target_folder = SmartScan.root_folder+"/"+SmartScan.nama_batch+"/"+SmartScan.template+"/";
                if(!System.IO.Directory.Exists(SmartScan.target_folder)) {
                  System.IO.Directory.CreateDirectory(SmartScan.target_folder);
                }
				e.FileName=string.Format(SmartScan.target_folder+"/"+LoginForm.username+"_FileXferTransfer_{0}.{1}",DateTime.Now.ToString("yyyyMMddHHmmss"),this._twain32.Capabilities.ImageFileFormat.GetCurrent().ToString().ToLower());
                this.counter++;
                this.textBox2.Text = this.counter.ToString();
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

				#region Isi dalam daftar yang tersedia izin dpi

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
					//100f,150f,200f,300f,600f,1200f,2400f
					foreach(var _val in new float[] { 150f }) {
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

				#region Isi dalam daftar jenis yang tersedia piksel

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

				#region Isi daftar mode yang tersedia transmisi

				this.xferModeToolStripDropDownButton.DropDownItems.Clear();
				Twain32.Enumeration _xferMech=this._twain32.Capabilities.XferMech.Get();
				for(int i=0; i<_xferMech.Count; i++) {
					if(_xferMech[i].ToString() == "File"){
						this.xferModeToolStripDropDownButton.DropDownItems.Add(
							_xferMech[i].ToString(),
							null,
							this._XferMechItemSelected).Tag=_xferMech[i];						
					}
				}
				
				//fujitsu error disini
				///this._XferMechItemSelected(this.xferModeToolStripDropDownButton.DropDownItems[_xferMech.CurrentIndex],new EventArgs());

				#endregion

				#region Mengisi daftar format file gambar yang tersedia

				this.fileFormatToolStripDropDownButton.DropDownItems.Clear();
				Twain32.Enumeration _fileFormats=this._twain32.Capabilities.ImageFileFormat.Get();
				for(int i=0; i<_fileFormats.Count; i++) {
					if(_fileFormats[i].ToString() == "Jfif") {
						this.fileFormatToolStripDropDownButton.DropDownItems.Add(
							_fileFormats[i].ToString(),
							null,
							this._FileFormatItemSelected).Tag=_fileFormats[i];
					}
				}
				//Fujitsu error
				//this._FileFormatItemSelected(this.fileFormatToolStripDropDownButton.DropDownItems[_fileFormats.CurrentIndex],new EventArgs());

				#endregion

				#region Проверяем возможность получения информации о изображении

				this._extImageInfoCap=this._twain32.IsCapSupported(TwCap.ExtImageInfo);
				this.toolStripStatusLabel1.Text=string.Format("Extended image info: {0}",this._extImageInfoCap==0?"not support":this._extImageInfoCap.ToString());

				#endregion

				this.pictureBox1.Select();
			} catch(TwainException ex) {
				Debug.WriteLine(string.Format("{0}: {1}\nReturnCode = {3}; ConditionCode = {4};\n{2}",ex.GetType().Name,ex.Message,ex.StackTrace,ex.ReturnCode,ex.ConditionCode));
			} catch(Exception ex) {
				//MessageBox.Show(ex.Message,ex.GetType().Name,MessageBoxButtons.OK,MessageBoxIcon.Error);
				Debug.WriteLine(string.Format("{0}: {1}\n{2}",ex.GetType().Name,ex.Message,ex.StackTrace));
			}
		}

		private void newToolStripButton_Click(object sender,EventArgs e) {
            this.counter = 0;
            if (SmartScan.root_folder == "") {
				MessageBox.Show("Silahkan pilih folder lokasi menyimpan hasil scan terlebih dahulu", "Peringatan",
				                MessageBoxButtons.OK, MessageBoxIcon.Error);
			} else {
				InputBoxValidation validation = delegate(string val) {
					if (val == "")
						return "Nama Batch harus diisi.";
					return "";
				};
				
				/**
				 * Debug : get capabilites 
				 **/
                 /**
					System.Diagnostics.Debug.WriteLine("SUPPORTED CAPS:");
					foreach(TwCap _cap in this._twain32.GetCap(TwCap.SupportedCaps) as object[]) {
					    System.Diagnostics.Debug.WriteLine(string.Format("{0}: {1}",_cap,this._twain32.IsCapSupported(_cap)));
					}				
				**/
				
				string value = "";
				if(InputBox.Show("Silahkan masukkan nama Batch","Nama Batch",ref value, validation) == DialogResult.OK) {
					string[] tmp = Regex.Split(value, ":");
					SmartScan.nama_batch = tmp[0] ;
					SmartScan.template   = tmp[1].Replace(" ",""); ;
										
					try {
						#region Examples of the capabilities

						//Feeder
						if((this._twain32.IsCapSupported(TwCap.Duplex)&TwQC.Get)!=0) {
							var _duplexCapValue=(TwDX)this._twain32.GetCap(TwCap.Duplex);							
							if(_duplexCapValue!=TwDX.None) {
								if((this._twain32.IsCapSupported(TwCap.FeederEnabled)&TwQC.Set)!=0) {
									this._twain32.SetCap(TwCap.FeederEnabled,true);

									if((this._twain32.IsCapSupported(TwCap.XferCount)&TwQC.Set)!=0) {
										this._twain32.SetCap(TwCap.XferCount,(short)-1);
									}
									
									if((this._twain32.IsCapSupported(TwCap.DuplexEnabled)&TwQC.Set)!=0) {
									    this._twain32.SetCap(TwCap.DuplexEnabled,false);
									}									
									
									if((this._twain32.IsCapSupported(TwCap.AutomaticRotate)&TwQC.Set)!=0) {
									   this._twain32.SetCap(TwCap.AutomaticRotate, false);
									}
									
									if((this._twain32.IsCapSupported(TwCap.Rotation)&TwQC.Set)!=0) {
									   this._twain32.SetCap(TwCap.Rotation, 180f); //jika di set selain 0, error
									}
                                    if ((this._twain32.IsCapSupported(TwCap.FlipRotation) & TwQC.Set) != 0)
                                    {
                                        this._twain32.SetCap(TwCap.FlipRotation, false);
                                    }
                                }
						   }
						}
						
						//MessageBox.Show("OK : Brightness");
						//Brightness
						if((this._twain32.IsCapSupported(TwCap.Brightness)&TwQC.Set)!=0) {
							this._twain32.SetCap(TwCap.Brightness,0f); //Allowed Values: -1000f to +1000f; Default Value: 0f;
						}

						//MessageBox.Show("OK : Contrast");
						//Contrast
						if((this._twain32.IsCapSupported(TwCap.Contrast)&TwQC.Set)!=0) {
							this._twain32.SetCap(TwCap.Contrast,0f); //Allowed Values: -1000f to +1000f; Default Value: 0f;
						}
						
						//MessageBox.Show("OK : ICAP_AUTODISCARDBLANKPAGES");
						//ICAP_AUTODISCARDBLANKPAGES
						if((this._twain32.IsCapSupported(TwCap.AutoDiscardBlankPages)&TwQC.Set)!=0) {
						    this._twain32.SetCap(TwCap.AutoDiscardBlankPages,TwBP.Auto);	
						}
                        #endregion

                        this._twain32.Acquire();
					} catch(Exception ex) {
						MessageBox.Show(ex.Message,ex.GetType().Name,MessageBoxButtons.OK,MessageBoxIcon.Error);
					}
				}
			}
		}
		
		private void toolStripFolderBrowserDialogButtonClick(object sender, System.EventArgs e)
		{
			FolderBrowserDialog folderDlg = new FolderBrowserDialog();
			folderDlg.ShowNewFolderButton = false;
			folderDlg.Description = "Silahkan pilih folder utama untuk menyimpan hasil scan Anda";
			folderDlg.SelectedPath = Application.StartupPath;
			
			DialogResult result = folderDlg.ShowDialog();
			if (result == DialogResult.OK)
			{
				textBox1.Text = folderDlg.SelectedPath;
				SmartScan.root_folder = folderDlg.SelectedPath;
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
		
		void Form1Load(object sender, EventArgs e)
		{
			
		}
		
		void SaveFileDialog1FileOk(object sender, CancelEventArgs e)
		{
			
		}
	}

	internal enum TwDX:ushort {
		None=0,          // TWDX_NONE
		OnePassDuplex=1, // TWDX_1PASSDUPLEX
		TwoPassDuplex=2  // TWDX_2PASSDUPLEX
	}
	
	internal enum TwBP:int {
	    Disable=-2, // TWBP_DISABLE
	    Auto=-1  // TWBP_AUTO
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
