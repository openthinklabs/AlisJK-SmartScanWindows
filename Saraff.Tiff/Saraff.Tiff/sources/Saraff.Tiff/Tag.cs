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
using Saraff.Tiff.Core;
using System.Diagnostics;

namespace Saraff.Tiff {

    /// <summary>
    /// Тег TIFF-файла.
    /// Tag of TIFF file.
    /// </summary>
    [DebuggerDisplay("{TagId}; TiffDataType = {TiffDataType}; Count = {Count};")]
    public sealed class Tag<T>:_Tag {

        private Tag() {
        }

        /// <summary>
        /// Создает и возвращает новый экземпляр класса <c>Tag</c>. Creates a new <c>Tag</c> instance.
        /// </summary>
        /// <param name="tagId">
        /// Кол тега. Tag id.
        /// </param>
        /// <param name="values">Значения тега. Values of tag.</param>
        /// <returns>
        /// Объект <c>Tag</c>. An <c>Tag</c> object.
        /// </returns>
        public static Tag<T> Create(TiffTags tagId,params T[] values) {
            return new Tag<T> {
                TagId=tagId,
                Values=values
            };
        }

        /// <summary>
        /// Возвращает значения тега. Get values of tag.
        /// </summary>
        public T[] Values {
            get;
            private set;
        }

        internal override TiffDataType TiffDataType {
            get {
                return TiffDataTypeHelper.TiffDataTypeof(typeof(T));
            }
        }

        internal override Type ItemType {
            get {
                return typeof(T);
            }
        }

        internal override int Count {
            get {
                return this.Values.Length;
            }
        }

        internal override object GetValue(int index) {
            return this.Values[index];
        }
    }

    /// <summary>
    /// Представляет тег TIFF-файла.
    /// Represents tag of TIFF file.
    /// </summary>
    public abstract class _Tag:ITag {

        #region ITag Members

        /// <summary>
        /// Возвращает код тега.
        /// Get tag id.
        /// </summary>
        public TiffTags TagId {
            get;
            protected set;
        }

        #endregion

        internal abstract TiffDataType TiffDataType {
            get;
        }

        internal abstract Type ItemType {
            get;
        }

        internal abstract int Count {
            get;
        }

        internal abstract object GetValue(int index);
    }
}
