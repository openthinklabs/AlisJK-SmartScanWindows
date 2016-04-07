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

namespace Saraff.Tiff.Core {

    internal static class Helper {

        internal static TiffHandle Write<T>(Stream stream, T obj) {
            var _size=Marshal.SizeOf(typeof(T));
            var _ptr=Marshal.AllocHGlobal(_size);
            try {
                Marshal.StructureToPtr(obj, _ptr, true);
                var _data=new byte[_size];
                Marshal.Copy(_ptr, _data, 0, _data.Length);
                var _position=stream.Position;
                stream.Write(_data, 0, _data.Length);
                foreach(var _field in typeof(T).GetFields()) {
                    if(_field.GetCustomAttributes(typeof(TiffHandleAttribute), false).Length>0) {
                        return TiffHandle.Create(_position+Marshal.OffsetOf(typeof(T), _field.Name).ToInt64());
                    }
                }
            } finally {
                Marshal.FreeHGlobal(_ptr);
            }
            return null;
        }

        internal static void Write(Stream stream, object obj) {
            var _obj=obj.GetType().IsEnum?Convert.ChangeType(obj, Enum.GetUnderlyingType(obj.GetType())):obj;
            var _size=Marshal.SizeOf(_obj);
            var _ptr=Marshal.AllocHGlobal(_size);
            try {
                Marshal.StructureToPtr(_obj, _ptr, true);
                var _data=new byte[_size];
                Marshal.Copy(_ptr, _data, 0, _data.Length);
                stream.Write(_data, 0, _data.Length);
            } finally {
                Marshal.FreeHGlobal(_ptr);
            }
        }

        internal static T Read<T>(Stream stream) {
            return (T)Helper.Read(stream,typeof(T));
        }

        internal static object Read(Stream stream,Type type) {
            var _size=Marshal.SizeOf(type);
            var _data=new byte[_size];
            stream.Read(_data,0,_data.Length);
            var _ptr=Marshal.AllocHGlobal(_size);
            try {
                Marshal.Copy(_data,0,_ptr,_data.Length);
                return Marshal.PtrToStructure(_ptr,type);
            } finally {
                Marshal.FreeHGlobal(_ptr);
            }
        }

        internal static object ToClrType(TiffTags tag,object value) {
            foreach(var _type in typeof(TiffTags).Assembly.GetTypes()) {
                if(_type.IsEnum) {
                    foreach(TiffTagAttribute _attr in _type.GetCustomAttributes(typeof(TiffTagAttribute),false)) {
                        if(_attr.Tag==tag) {
                            return Enum.ToObject(_type,value);
                        }
                    }
                }
            }
            return value;
        }
    }
}
