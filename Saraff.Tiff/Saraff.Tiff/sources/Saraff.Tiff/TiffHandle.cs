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
using Saraff.Tiff.Core;
using System.Diagnostics;

namespace Saraff.Tiff {

    /// <summary>
    /// Дескриптор TIFF-файла. Handle of a TIFF file.
    /// </summary>
    [DebuggerDisplay("{Offset}; IsOpen = {IsOpen};")]
    public sealed class TiffHandle:IDisposable {

        private TiffHandle() {
        }

        internal static TiffHandle Create(long offset) {
            return new TiffHandle {
                Offset=offset,
                IsOpen=true
            };
        }

        internal static TiffHandle Create(Stream stream) {
            return TiffHandle.Create(stream.Position);
        }

        internal void Close(Stream stream) {
            var _position=stream.Position;
            stream.Position=this.Offset;
            try {
                Helper.Write(stream,(uint)_position);
            } finally {
                stream.Position=_position;
                this.Dispose();
            }
        }

        internal long Offset {
            get;
            private set;
        }

        internal bool IsOpen {
            get;
            private set;
        }

        #region IDisposable Members

        /// <summary>
        /// Освобождает неуправляемые ресурсы, используемые классом <c>TiffWriter</c>, а при необходимости освобождает также управляемые ресурсы. 
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose() {
            this.IsOpen=false;
        }

        #endregion

        /// <summary>
        /// Возвращает строку, представляющую текущий объект.
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>Строка.</returns>
        public override string ToString() {
            return string.Format("{0:X8}",this.Offset);
        }
    }
}
