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
using System.Text;
using Saraff.Twain;
using System.Drawing.Imaging;
using System.Reflection;
using System.IO;

namespace Saraff.Twain.Sample3 {

    internal sealed class Program {

        [STAThread]
        private static void Main(string[] args) {
            try {
                using(Twain32 _twain32=new Twain32()) {
                    var _asm=_twain32.GetType().Assembly;
                    Console.WriteLine(
                        "{1} {2}{0}{3}{0}",
                        Environment.NewLine,
                        ((AssemblyTitleAttribute)_asm.GetCustomAttributes(typeof(AssemblyTitleAttribute),false)[0]).Title,
                        ((AssemblyFileVersionAttribute)_asm.GetCustomAttributes(typeof(AssemblyFileVersionAttribute),false)[0]).Version,
                        ((AssemblyCopyrightAttribute)_asm.GetCustomAttributes(typeof(AssemblyCopyrightAttribute),false)[0]).Copyright);

                    #region ShowUI

                    Console.Write("ShowUI {0}: ",_twain32.ShowUI?"[Y/n]":"[y/N]");
                    for(var _res=Console.ReadLine().Trim().ToUpper(); !string.IsNullOrEmpty(_res); ) {
                        _twain32.ShowUI=_res=="Y";
                        break;
                    }
                    Console.WriteLine("ShowUI = {0}",_twain32.ShowUI?"Y":"N");

                    #endregion

                    #region IsTwain2Enable

                    Console.Write("IsTwain2Enable {0}: ",_twain32.IsTwain2Enable?"[Y/n]":"[y/N]");
                    for(var _res=Console.ReadLine().Trim().ToUpper(); !string.IsNullOrEmpty(_res); ) {
                        _twain32.IsTwain2Enable=_res=="Y";
                        break;
                    }
                    Console.WriteLine("IsTwain2Enable = {0}",_twain32.IsTwain2Enable?"Y":"N");

                    #endregion

                    _twain32.OpenDSM();
                    if(_twain32.IsTwain2Enable) {
                        Console.WriteLine("IsTwain2Supported = {0}",_twain32.IsTwain2Supported?"Y":"N");
                    }

                    #region Select Data Source

                    Console.WriteLine();
                    Console.WriteLine("Select Data Source:");
                    for(var i=0; i<_twain32.SourcesCount; i++) {
                        Console.WriteLine("{0}: {1}{2}",i,_twain32.GetSourceProductName(i),_twain32.IsTwain2Supported&&_twain32.GetIsSourceTwain2Compatible(i)?" (TWAIN 2.x)":string.Empty);
                    }
                    Console.Write("[{0}]: ",_twain32.SourceIndex);
                    for(var _res=Console.ReadLine().Trim(); !string.IsNullOrEmpty(_res); ) {
                        _twain32.SourceIndex=Convert.ToInt32(_res);
                        break;
                    }
                    Console.WriteLine(string.Format("Data Source: {0}",_twain32.GetSourceProductName(_twain32.SourceIndex)));

                    #endregion

                    _twain32.OpenDataSource();

                    if(!_twain32.ShowUI) {

                        #region Select Resolution

                        Console.WriteLine();
                        Console.WriteLine("Select Resolution:");
                        var _resolutions=_twain32.Capabilities.XResolution.Get();
                        for(var i=0; i<_resolutions.Count; i++) {
                            Console.WriteLine("{0}: {1} dpi",i,_resolutions[i]);
                        }
                        Console.Write("[{0}]: ",_resolutions.CurrentIndex);
                        for(var _res=Console.ReadLine().Trim(); !string.IsNullOrEmpty(_res); ) {
                            var _val=(float)_resolutions[Convert.ToInt32(_res)];
                            _twain32.Capabilities.XResolution.Set(_val);
                            _twain32.Capabilities.YResolution.Set(_val);
                            break;
                        }
                        Console.WriteLine(string.Format("Resolution: {0}",_twain32.Capabilities.XResolution.GetCurrent()));

                        #endregion

                        #region Select Pixel Type

                        Console.WriteLine();
                        Console.WriteLine("Select Pixel Type:");
                        var _pixels=_twain32.Capabilities.PixelType.Get();
                        for(var i=0; i<_pixels.Count; i++) {
                            Console.WriteLine("{0}: {1}",i,_pixels[i]);
                        }
                        Console.Write("[{0}]: ",_pixels.CurrentIndex);
                        for(var _res=Console.ReadLine().Trim(); !string.IsNullOrEmpty(_res); ) {
                            var _val=(TwPixelType)_pixels[Convert.ToInt32(_res)];
                            _twain32.Capabilities.PixelType.Set(_val);
                            break;
                        }
                        Console.WriteLine(string.Format("Pixel Type: {0}",_twain32.Capabilities.PixelType.GetCurrent()));

                        #endregion

                    }

                    _twain32.EndXfer+=(object sender,Twain32.EndXferEventArgs e) => {
                        try {
                            var _file=Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),Path.ChangeExtension(Path.GetFileName(Path.GetTempFileName()),".jpg"));
                            e.Image.Save(_file,ImageFormat.Jpeg);
                            Console.WriteLine();
                            Console.WriteLine(string.Format("Saved in: {0}",_file));
                            e.Image.Dispose();
                        } catch(Exception ex) {
                            Console.WriteLine("{0}: {1}{2}{3}{2}",ex.GetType().Name,ex.Message,Environment.NewLine,ex.StackTrace);
                        }
                    };

                    _twain32.AcquireCompleted+=(sender,e) => {
                        try {
                            Console.WriteLine();
                            Console.WriteLine("Acquire Completed.");
                        } catch(Exception ex) {
                            Program.WriteException(ex);
                        }
                    };

                    _twain32.AcquireError+=(object sender,Twain32.AcquireErrorEventArgs e) => {
                        try {
                            Console.WriteLine();
                            Console.WriteLine("Acquire Error: ReturnCode = {0}; ConditionCode = {1};",e.Exception.ReturnCode,e.Exception.ConditionCode);
                            Program.WriteException(e.Exception);
                        } catch(Exception ex) {
                            Program.WriteException(ex);
                        }
                    };

                    _twain32.Acquire();
                }
            } catch(Exception ex) {
                Program.WriteException(ex);
            }
        }

        private static void WriteException(Exception ex) {
            for(var _ex=ex; _ex!=null; _ex=_ex.InnerException) {
                Console.WriteLine("{0}: {1}{2}{3}{2}",_ex.GetType().Name,_ex.Message,Environment.NewLine,_ex.StackTrace);
            }
        }
    }
}
