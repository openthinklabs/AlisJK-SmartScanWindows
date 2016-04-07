/* Этот файл является частью библиотеки Saraff.Tiff.NET
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
 * This file is part of Saraff.Tiff.NET.
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
using System.Text;
using System.IO;

namespace Saraff.Tiff.Core {

    internal sealed class _TiffWriterCore:TiffWriter {

        public override void Flush() {
            this.BaseStream.Flush();
        }

        public override TiffHandle WriteHeader() {
            return Helper.Write(this.BaseStream,new TiffHeader {
                magic=MagicValues.LittleEndian,
                version=0x2a
            });
        }

        public override TiffHandle WriteImageFileDirectory(TiffHandle owner,Collection<ITag> tags) {
            if(!owner.IsOpen) {
                throw new ArgumentException("Дескриптор владельца закрыт. Owner handle is closed.");
            }
            owner.Close(this.BaseStream);
            var _delayed=new Collection<_DelayedData>();

            #region IFD

            Collection<ITag> _tags=new Collection<ITag>(tags);
            for(int i=0; i<_tags.Count; i++) {
                for(int ii=i; ii<_tags.Count; ii++) {
                    if((ushort)_tags[ii].TagId<(ushort)_tags[i].TagId) {
                        var _tag=_tags[i];
                        _tags[i]=_tags[ii];
                        _tags[ii]=_tag;
                    }
                }
            }
            Helper.Write(this.BaseStream,(ushort)_tags.Count);
            foreach(_Tag _tag in _tags) {
                if(_tag.ItemType!=typeof(TiffHandle)) {
                    int _itemSize=TiffDataTypeHelper.Sizeof(_tag.TiffDataType);
                    if(_itemSize*_tag.Count>4) {
                        #region Массив значений

                        var _handle=Helper.Write(this.BaseStream,new TiffDirEntry {
                            tag=_tag.TagId,
                            type=_tag.TiffDataType,
                            count=(uint)_tag.Count
                        });
                        using(var _stream=new MemoryStream()) {
                            for(int i=0; i<_tag.Count; i++) {
                                Helper.Write(_stream,_tag.GetValue(i));
                            }
                            _delayed.Add(new _DelayedData(_handle,_stream.ToArray()));
                        }

                        #endregion
                    } else {
                        #region Массив, вписанный в поле offset

                        using(var _stream=new MemoryStream(new byte[4],true)) {
                            for(int i=0; i<_tag.Count; i++) {
                                Helper.Write(_stream,_tag.GetValue(i));
                            }
                            _stream.Position=0;
                            Helper.Write(this.BaseStream,new TiffDirEntry {
                                tag=_tag.TagId,
                                type=_tag.TiffDataType,
                                count=(uint)_tag.Count,
                                offset=new BinaryReader(_stream).ReadUInt32()
                            });
                        }

                        #endregion
                    }
                } else {
                    if(_tag.Count>1) {
                        #region Массив смещений

                        var _handle=Helper.Write(this.BaseStream,new TiffDirEntry {
                            tag=_tag.TagId,
                            type=_tag.TiffDataType,
                            count=(uint)_tag.Count
                        });
                        using(var _stream=new MemoryStream()) {
                            for(int i=0; i<_tag.Count; i++) {
                                using(var _value=_tag.GetValue(i) as TiffHandle) {
                                    Helper.Write(_stream,(uint)_value.Offset);
                                }
                            }
                            _delayed.Add(new _DelayedData(_handle,_stream.ToArray()));
                        }

                        #endregion
                    } else {
                        #region Смещение

                        using(var _value=_tag.GetValue(0) as TiffHandle) {
                            Helper.Write(this.BaseStream,new TiffDirEntry {
                                tag=_tag.TagId,
                                type=_tag.TiffDataType,
                                count=(uint)_tag.Count,
                                offset=(uint)_value.Offset
                            });
                        }

                        #endregion
                    }
                }
            }
            var _ifdHandle=TiffHandle.Create(this.BaseStream);
            Helper.Write(this.BaseStream,(uint)0);

            #endregion

            #region Отложенные фрагменты

            foreach(var _item in _delayed) {
                _item.Owner.Close(this.BaseStream);
                this.BaseStream.Write(_item.Data,0,_item.Data.Length);
            }

            #endregion

            return _ifdHandle;
        }

        public override TiffHandle WriteData(byte[] data) {
            var _handle=TiffHandle.Create(this.BaseStream);
            this.BaseStream.Write(data,0,data.Length);
            return _handle;
        }

        private sealed class _DelayedData {

            internal _DelayedData(TiffHandle owner,byte[] data) {
                this.Owner=owner;
                this.Data=data;
            }

            internal TiffHandle Owner {
                get;
                private set;
            }

            internal byte[] Data {
                get;
                private set;
            }
        }
    }
}
