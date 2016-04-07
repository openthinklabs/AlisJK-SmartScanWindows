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

namespace Saraff.Tiff.Automaton {

    /// <summary>
    /// Базовый класс состояний.
    /// </summary>
    /// <typeparam name="T">Интерфейс автомата.</typeparam>
    internal abstract class StateBase<T> where T:class {

        /// <summary>
        /// Добавляет переход.
        /// </summary>
        /// <param name="eventName">Имя события.</param>
        /// <param name="target">Тип состояния.</param>
        internal void AddEdge(string eventName,T target) {
            this.Automaton.AddEdge(this.GetType(),eventName,target);
        }

        /// <summary>
        /// Fires the event.
        /// </summary>
        /// <param name="eventName">Name of the event.</param>
        protected void FireEvent(string eventName) {
            this.OnStateChanged(new AutomatonEventArgs(eventName));
        }

        /// <summary>
        /// Raises the <see cref="E:StateChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="Saraff.Acs.Core.AutomatonEventArgs"/> instance containing the event data.</param>
        protected virtual void OnStateChanged(AutomatonEventArgs e) {
            if(this.StateChanged!=null) {
                this.StateChanged(this,e);
            }
        }

        /// <summary>
        /// Возвращает или устанавливает контекст.
        /// </summary>
        internal AutomatonBase<T> Automaton {
            get;
            set;
        }

        /// <summary>
        /// Возникает при изменении состояния.
        /// </summary>
        internal event EventHandler<AutomatonEventArgs> StateChanged;
    }
}
