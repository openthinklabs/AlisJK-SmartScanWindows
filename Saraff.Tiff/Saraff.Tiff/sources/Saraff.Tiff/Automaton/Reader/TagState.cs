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
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using Saraff.Tiff.Core;

namespace Saraff.Tiff.Automaton.Reader {

    internal sealed class TagState:ReaderStateBase {
        internal const string ToValue="To_Value";
        internal const string ToNextIfd="To_Next_IFD";
        private long _position=-1;

        public override ITag ReadTag() {
            if(this._position!=-1) {
                this.Context.Reader.BaseStream.Seek(this._position,SeekOrigin.Begin);
            }
            if(this.Context.TagCountLeft==0) {
                this._position=-1;
                this.Context.NextImageFileDirectory=Helper.Read<uint>(this.Context.Reader.BaseStream);
                this.FireEvent(TagState.ToNextIfd);
                return null;
            }
            this.Context.ValueCountLeft=(int)(this.Context.CurrentTag=Helper.Read<TiffDirEntry>(this.Context.Reader.BaseStream)).count;
            this.Context.TagCountLeft--;

            this._position=this.Context.Reader.BaseStream.Position;
            int _itemSize=TiffDataTypeHelper.Sizeof(this.Context.CurrentTag.type);
            if(_itemSize*this.Context.CurrentTag.count>4) {
                // Массив значений
                this.Context.Reader.BaseStream.Seek(this.Context.CurrentTag.offset,SeekOrigin.Begin);
            } else {
                // Массив, вписанный в поле offset
                this.Context.Reader.BaseStream.Seek(Marshal.OffsetOf(typeof(TiffDirEntry),"offset").ToInt64()-Marshal.SizeOf(typeof(TiffDirEntry)),SeekOrigin.Current);
            }

            this.FireEvent(TagState.ToValue);
            return this.Context.CurrentTag;
        }
    }
}
