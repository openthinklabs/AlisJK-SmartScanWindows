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

namespace Saraff.Tiff.Automaton.Reader {

    internal sealed class ReaderAutomatonContext:AutomatonBase<IReaderAutomaton>,IReaderAutomaton {

        internal ReaderAutomatonContext(TiffReader reader) {
            if(reader==null) {
                throw new ArgumentNullException();
            }
            this.Reader=reader;

            this.HeaderState.AddEdge(HeaderState.ToIfd,this.ImageFileDirectoryState);

            this.ImageFileDirectoryState.AddEdge(ImageFileDirectoryState.ToFirstTag,this.TagState);
            this.ImageFileDirectoryState.AddEdge(ImageFileDirectoryState.ToEnd,this.HeaderState);

            this.TagState.AddEdge(TagState.ToValue,this.ValueState);
            this.TagState.AddEdge(TagState.ToNextIfd,this.ImageFileDirectoryState);

            this.ValueState.AddEdge(ValueState.ToNextTag,this.TagState);

            this.Current=this.HeaderState;
        }

        #region IReaderAutomaton

        public void ReadHeader() {
            this.Current.ReadHeader();
        }

        public int ReadImageFileDirectory() {
            return this.Current.ReadImageFileDirectory();
        }

        public ITag ReadTag() {
            return this.Current.ReadTag();
        }

        public object ReadValue() {
            return this.Current.ReadValue();
        }

        public byte[] ReadData(TiffHandle handle,long count) {
            return this.Current.ReadData(handle,count);
        }

        #endregion

        #region States

        private HeaderState HeaderState {
            get {
                return this.GetState<HeaderState>();
            }
        }

        private ImageFileDirectoryState ImageFileDirectoryState {
            get {
                return this.GetState<ImageFileDirectoryState>();
            }
        }

        private TagState TagState {
            get {
                return this.GetState<TagState>();
            }
        }

        private ValueState ValueState {
            get {
                return this.GetState<ValueState>();
            }
        }

        #endregion

        internal TiffReader Reader {
            get;
            private set;
        }

        internal long NextImageFileDirectory {
            get;
            set;
        }

        internal int TagCountLeft {
            get;
            set;
        }

        internal TiffDirEntry CurrentTag {
            get;
            set;
        }

        internal int ValueCountLeft {
            get;
            set;
        }
    }
}
