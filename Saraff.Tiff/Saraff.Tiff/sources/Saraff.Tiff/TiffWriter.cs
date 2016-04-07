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
using System.Collections.ObjectModel;

namespace Saraff.Tiff {

    /// <summary>
    /// Предоставляет средство записи, обеспечивающее быстрый прямой доступ (без кэширования) к данным TIFF-изображения. 
    /// Represents a writer that provides fast, non-cached, forward-only access to TIFF data.
    /// </summary>
    public abstract class TiffWriter:IDisposable {

        /// <summary>
        /// Создает и возвращает новый экземпляр класса <c>TiffWriter</c> с использованием указанного потока.
        /// Creates a new <c>TiffWriter</c> instance using the specified stream.
        /// </summary>
        /// <param name="stream">
        /// Поток, содержащий данные TIFF.
        /// The stream containing the TIFF data.
        /// </param>
        /// <returns>
        /// Объект <c>TiffWriter</c>, используемый для считывания данных, содержащихся в потоке.
        /// An <c>TiffWriter</c> object used to read the data contained in the stream.
        /// </returns>
        public static TiffWriter Create(Stream stream) {
            return new _TiffWriterCore {
                BaseStream=stream
            };
        }

        /// <summary>
        /// При переопределении в производном классе сбрасывает в основной поток содержимое буфера, а также очищает основной поток.
        /// When overridden in a derived class, flushes whatever is in the buffer to the underlying streams and also flushes the underlying stream.
        /// </summary>
        public abstract void Flush();

        /// <summary>
        /// При переопределении в производном классе записывает заголовок TIFF-файла.
        /// When overridden in a derived class, writes the header of a TIFF file.
        /// </summary>
        /// <returns>Дескриптор TIFF-файла. Handle of a TIFF file.</returns>
        public abstract TiffHandle WriteHeader();

        /// <summary>
        /// При переопределении в производном классе записывает директорию файла изображения (IFD).
        /// When overridden in a derived class, writes the image file directory (IFD).
        /// </summary>
        /// <param name="owner">
        /// Владелелец создаваемой директории файла изображения (IFD).
        /// Owner of creating the image file directory (IFD).
        /// </param>
        /// <param name="tags">Коллекция тегов. Collection of the tags.</param>
        /// <returns>Дескриптор TIFF-файла. Handle of a TIFF file.</returns>
        public abstract TiffHandle WriteImageFileDirectory(TiffHandle owner,Collection<ITag> tags);

        /// <summary>
        /// При переопределении в производном классе записывает произвольные двоичные данные.
        /// When overridden in a derived class, writes binary data.
        /// </summary>
        /// <param name="data">Двоичные данные. Binary data.</param>
        /// <returns>Дескриптор TIFF-файла. Handle of a TIFF file.</returns>
        public abstract TiffHandle WriteData(byte[] data);

        #region IDisposable Members

        /// <summary>
        /// Освобождает неуправляемые ресурсы, используемые классом <c>TiffWriter</c>, а при необходимости освобождает также управляемые ресурсы. 
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose() {
            if(this.BaseStream!=null) {
                this.BaseStream.Dispose();
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Предоставляет доступ к базовому потоку <c>TiffWriter</c>.
        /// Exposes access to the underlying stream of the <c>TiffWriter</c>.
        /// </summary>
        public Stream BaseStream {
            get;
            private set;
        }

        #endregion
    }
}
