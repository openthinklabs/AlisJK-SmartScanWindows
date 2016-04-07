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

namespace Saraff.Tiff.Automaton.Reader {

    internal abstract class ReaderStateBase:StateBase<IReaderAutomaton>,IReaderAutomaton {

        #region IReaderAutomaton

        public virtual void ReadHeader() {
            throw new InvalidOperationException();
        }

        public virtual int ReadImageFileDirectory() {
            throw new InvalidOperationException();
        }

        public virtual ITag ReadTag() {
            throw new InvalidOperationException();
        }

        public virtual object ReadValue() {
            throw new InvalidOperationException();
        }

        public byte[] ReadData(TiffHandle handle,long count) {
            if(!handle.IsOpen) {
                throw new ArgumentException("Дескриптор закрыт. Handle is closed.");
            }
            var _position=this.Context.Reader.BaseStream.Position;
            try {
                this.Context.Reader.BaseStream.Seek(handle.Offset,SeekOrigin.Begin);
                var _result=new byte[count];
                this.Context.Reader.BaseStream.Read(_result,0,_result.Length);
                return _result;
            } finally {
                this.Context.Reader.BaseStream.Seek(_position,SeekOrigin.Begin);
            }
        }

        #endregion

        protected ReaderAutomatonContext Context {
            get {
                return (ReaderAutomatonContext)this.Automaton;
            }
        }
    }
}
