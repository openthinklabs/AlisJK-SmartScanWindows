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
using Saraff.Tiff.Automaton.Reader;

namespace Saraff.Tiff.Core {

    internal sealed class _TiffReaderCore:TiffReader {
        private ReaderAutomatonContext _context;

        public override void ReadHeader() {
            this.Context.ReadHeader();
        }

        public override int ReadImageFileDirectory() {
            return this.Context.ReadImageFileDirectory();
        }

        public override ITag ReadTag() {
            return this.Context.ReadTag();
        }

        public override object ReadValue() {
            var _val=this.Context.ReadValue();
            return _val!=null?Helper.ToClrType(this.Context.CurrentTag.TagId,_val):_val;
        }

        public override TiffHandle ReadHandle() {
            var _val=this.Context.ReadValue();
            return _val!=null?TiffHandle.Create(Convert.ToInt64(_val)):null;
        }

        public override byte[] ReadData(TiffHandle handle,long count) {
            return this.Context.ReadData(handle,count);
        }

        private ReaderAutomatonContext Context {
            get {
                if(this._context==null) {
                    this._context=new ReaderAutomatonContext(this);
                }
                return this._context;
            }
        }
    }
}
