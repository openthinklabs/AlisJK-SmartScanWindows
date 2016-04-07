/* Этот файл является частью примеров использования библиотеки Saraff.Tiff.NET
 * © SARAFF SOFTWARE (Кирножицкий Андрей), 2014.
 * Saraff.Tiff.NET - свободная программа: вы можете перераспространять ее и/или
 * изменять ее на условиях Меньшей Стандартной общественной лицензии GNU в том виде,
 * в каком она была опубликована Фондом свободного программного обеспечения;
 * либо версии 3 лицензии, либо (по вашему выбору) любой более поздней
 * версии.
 * Saraff.Tiff.NET распространяется в надежде, что она будет полезной,
 * но БЕЗО ВСЯКИХ ГАРАНТИЙ; даже без неявной гарантии ТОВАРНОГО ВИДА
 * или ПРИГОДНОСТИ ДЛЯ ОПРЕДЕЛЕННЫХ ЦЕЛЕЙ. Подробнее см. в Меньшей Стандартной
 * общественной лицензии GNU.
 * Вы должны были получить копию Меньшей Стандартной общественной лицензии GNU
 * вместе с этой программой. Если это не так, см.
 * <http://www.gnu.org/licenses/>.)
 * 
 * This file is part of samples of Saraff.Tiff.NET.
 * © SARAFF SOFTWARE (Kirnazhytski Andrei), 2014.
 * Saraff.Tiff.NET is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * Saraff.Tiff.NET is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 * You should have received a copy of the GNU Lesser General Public License
 * along with Saraff.Tiff.NET. If not, see <http://www.gnu.org/licenses/>.
 * 
 * PLEASE SEND EMAIL TO:  tiff@saraff.ru.
 */
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.IO;
using Saraff.Tiff;
using Saraff.Tiff.Core;

namespace TiffDebug {

    internal class Program {
        private const int _imageWidth=150;
        private const int _imageHeight=160;
        private const int _imageStripCount=Program._imageHeight/10;
        private const string _file=@"d:\test.tif";

        private static void Main(string[] args) {
            using(var _stream=File.Create(Program._file)) {
                var _writer=TiffWriter.Create(_stream);

                var _handle=_writer.WriteHeader();

                for(int _page=0; _page<4; _page++) {
                    var _strips=new TiffHandle[Program._imageStripCount];
                    var _stripByteCounts=new uint[_strips.Length];
                    for(byte i=0,_val=0; i<_strips.Length; i++,_val+=0x11) {
                        var _buf=new byte[Program._imageWidth*(Program._imageHeight/_strips.Length)*3];
                        for(int ii=0; ii<_buf.Length; ii++) {
                            switch(_page) {
                                case 0:
                                case 1:
                                case 2:
                                    _buf[ii]=ii%3==_page?_val:(byte)0;
                                    break;
                                default:
                                    _buf[ii]=_val;
                                    break;
                            }
                        }
                        _strips[i]=_writer.WriteData(_buf);
                        _stripByteCounts[i]=(uint)_buf.Length;
                    }
                    _handle=_writer.WriteImageFileDirectory(_handle, new Collection<ITag> {
                        Tag<uint>.Create(TiffTags.ImageWidth,Program._imageWidth),
                        Tag<uint>.Create(TiffTags.ImageLength,Program._imageHeight),
                        Tag<ushort>.Create(TiffTags.BitsPerSample,8,8,8),
                        Tag<TiffCompression>.Create(TiffTags.Compression,TiffCompression.NONE),
                        Tag<TiffPhotoMetric>.Create(TiffTags.PhotometricInterpretation,TiffPhotoMetric.RGB),
                        Tag<TiffHandle>.Create(TiffTags.StripOffsets,_strips),
                        Tag<ushort>.Create(TiffTags.SamplesPerPixel,3),
                        Tag<uint>.Create(TiffTags.RowsPerStrip,Program._imageHeight/16),
                        Tag<uint>.Create(TiffTags.StripByteCounts,_stripByteCounts),
                        Tag<ulong>.Create(TiffTags.XResolution,(1UL<<32)|300UL),
                        Tag<ulong>.Create(TiffTags.YResolution,(1UL<<32)|300UL),
                        Tag<TiffResolutionUnit>.Create(TiffTags.ResolutionUnit,TiffResolutionUnit.INCH),
                        Tag<char>.Create(TiffTags.Software,"SARAFF SOFTWARE".ToCharArray()),
                        Tag<char>.Create(TiffTags.Copyright,"(c) SARAFF 2014".ToCharArray())
                    });
                }
            }
            using(var _stream=File.Open(Program._file,FileMode.Open)) {
                var _reader=TiffReader.Create(_stream);

                _reader.ReadHeader();
                for(var _count=_reader.ReadImageFileDirectory(); _count!=0; _count=_reader.ReadImageFileDirectory()) {
                    Console.WriteLine("ImageFileDirectory: {0} tags.",_count);
                    var _dict=new Dictionary<TiffTags,Collection<object>>();

                    for(ITag _tag=_reader.ReadTag(); _tag!=null; _tag=_reader.ReadTag()) {
                        Console.Write("{0}: {{ ",_tag.TagId);
                        _dict.Add(_tag.TagId,new Collection<object>());
                        switch(_tag.TagId) {
                            case TiffTags.StripOffsets:
                                for(object _value=_reader.ReadHandle(); _value!=null; _value=_reader.ReadHandle()) {
                                    Console.Write("{0} ",_value);
                                    _dict[_tag.TagId].Add(_value);
                                }
                                break;
                            default:
                                for(object _value=_reader.ReadValue(); _value!=null; _value=_reader.ReadValue()) {
                                    Console.Write("{0} ",(_value is ulong)?((float)((ulong)_value&0xffffffff)/(float)((ulong)_value>>32)):_value);
                                    _dict[_tag.TagId].Add(_value);
                                }
                                break;
                        }
                        Console.WriteLine("}");
                    }

                    for(int i=0; i<_dict[TiffTags.StripOffsets].Count; i++) {
                        Console.WriteLine();
                        Console.WriteLine();
                        Console.WriteLine();
                        Console.WriteLine("Strip {0}: ",i);
                        var _data=_reader.ReadData((TiffHandle)_dict[TiffTags.StripOffsets][i],Convert.ToInt64(_dict[TiffTags.StripByteCounts][i]));
                        for(int ii=0; ii<_data.Length; ii++) {
                            if((ii&0x0f)==0) {
                                Console.WriteLine();
                                Console.Write("{0:X4}: ",ii);
                            }
                            Console.Write("{0:X2} ",_data[ii]);
                        }
                    }
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine();
                }
            }
        }
    }
}
