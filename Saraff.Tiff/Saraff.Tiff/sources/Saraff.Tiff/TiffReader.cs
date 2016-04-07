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
using Saraff.Tiff.Core;

namespace Saraff.Tiff {

    /// <summary>
    /// Предоставляет средство чтения, обеспечивающее быстрый прямой доступ (без кэширования) к данным TIFF-изображения. 
    /// Represents a reader that provides fast, non-cached, forward-only access to TIFF data.
    /// </summary>
    public abstract class TiffReader:IDisposable {

        /// <summary>
        /// Создает и возвращает новый экземпляр класса <c>TiffReader</c> с использованием указанного потока.
        /// Creates a new <c>TiffReader</c> instance using the specified stream.
        /// </summary>
        /// <param name="stream">
        /// Поток, содержащий данные TIFF.
        /// The stream containing the TIFF data.
        /// </param>
        /// <returns>
        /// Объект <c>TiffReader</c>, используемый для считывания данных, содержащихся в потоке.
        /// An <c>TiffReader</c> object used to read the data contained in the stream.
        /// </returns>
        public static TiffReader Create(Stream stream) {
            return new _TiffReaderCore {
                BaseStream=stream
            };
        }

        /// <summary>
        /// При переопределении в производном классе считывает заголовок TIFF-файла.
        /// When overridden in a derived class, reads header of a TIFF file.
        /// </summary>
        public abstract void ReadHeader();

        /// <summary>
        /// При переопределении в производном классе считывает директорию файла изображения (IFD).
        /// When overridden in a derived class, reads the image file directory (IFD).
        /// </summary>
        /// <returns>
        /// Количество тегов в директории или 0, если все директории были считаны.
        /// The number of tags in a directory, or 0 if the directory has been read.
        /// </returns>
        public abstract int ReadImageFileDirectory();

        /// <summary>
        /// При переопределении в производном классе считывает тег.
        /// When overridden in a derived class, reads the tag.
        /// </summary>
        /// <returns>
        /// Тег или null, если все теги были считаны.
        /// Tag, or null, if all the tags were read.
        /// </returns>
        public abstract ITag ReadTag();

        /// <summary>
        /// При переопределении в производном классе считывает значение тега.
        /// When overridden in a derived class, reads the value of tag.
        /// </summary>
        /// <returns>
        /// Значение тега или null, если все значения были считаны.
        /// Tag value, or null, if all the values ​​have been read.
        /// </returns>
        public abstract object ReadValue();

        /// <summary>
        /// При переопределении в производном классе считывает значение тега и представляет его ввиде дескриптора TIFF-файла.
        /// When overridden in a derived class, reads the value of tag and represent it as a handle of a TIFF file.
        /// </summary>
        /// <returns>
        /// Значение тега или null, если все значения были считаны.
        /// Tag value, or null, if all the values ​​have been read.
        /// </returns>
        public abstract TiffHandle ReadHandle();

        /// <summary>
        /// При переопределении в производном классе считывает указанное количество байтов из потока, начиная с заданной точки.
        /// When overridden in a derived class, reads the specified number of bytes from the stream, starting from a specified point.
        /// </summary>
        /// <param name="handle">Дескриптор TIFF-файла. Handle of TIFF file.</param>
        /// <param name="count">Количество байтов, чтение которых необходимо выполнить. The number of bytes to read.</param>
        /// <returns>Двоичные данные. Binary data.</returns>
        public abstract byte[] ReadData(TiffHandle handle,long count);

        #region IDisposable Members

        /// Освобождает неуправляемые ресурсы, используемые классом <c>TiffReader</c>, а при необходимости освобождает также управляемые ресурсы. 
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        public void Dispose() {
            if(this.BaseStream!=null) {
                this.BaseStream.Dispose();
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Предоставляет доступ к базовому потоку <c>TiffReader</c>.
        /// Exposes access to the underlying stream of the <c>TiffReader</c>.
        /// </summary>
        public Stream BaseStream {
            get;
            private set;
        }

        /// <summary>
        /// Возвращает версию TIFF-файла.
        /// Get version of TIFF file.
        /// </summary>
        public ushort TiffVersion {
            get;
            internal set;
        }

        #endregion
    }
}
