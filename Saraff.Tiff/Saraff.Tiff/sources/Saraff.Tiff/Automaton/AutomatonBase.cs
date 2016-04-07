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
    /// Базовый класс контекстов.
    /// </summary>
    /// <typeparam name="T">Интерфейс автомата.</typeparam>
    internal abstract class AutomatonBase<T> where T:class {
        private Dictionary<Type,Dictionary<string,T>> _edges=new Dictionary<Type,Dictionary<string,T>>(); // Словарь переховов
        private Dictionary<Type,T> _states=new Dictionary<Type,T>(); // Словарь состояний

        /// <summary>
        /// Добавляет переход.
        /// </summary>
        /// <param name="sourceType">Тип исходного состояния.</param>
        /// <param name="name">Имя события.</param>
        /// <param name="target">Экземпляр целевого состояния.</param>
        internal void AddEdge(Type sourceType,string name,T target) {
            if(!this._edges.ContainsKey(sourceType)) {
                var _val=new Dictionary<string,T>();
                _val.Add(name,null);
                this._edges.Add(sourceType,_val);
            }
            this._edges[sourceType][name]=target;
        }

        /// <summary>
        /// Возвращает экземпляр состояния.
        /// </summary>
        /// <typeparam name="TResult">Тип состояния.</typeparam>
        /// <returns>Экземпляр состояния.</returns>
        protected TResult GetState<TResult>() where TResult:StateBase<T>,T {
            if(!this._states.ContainsKey(typeof(TResult))) {
                this._states.Add(typeof(TResult),this._CreateState<TResult>());
            }
            return this._states[typeof(TResult)] as TResult;
        }

        /// <summary>
        /// Создает и возвращает экземпляр состояния.
        /// </summary>
        /// <typeparam name="TResult">Тип состояния.</typeparam>
        /// <returns>Экземпляр состояния.</returns>
        private TResult _CreateState<TResult>() where TResult:StateBase<T>,T {
            var _result=Activator.CreateInstance(typeof(TResult)) as TResult;
            _result.Automaton=this;
            _result.StateChanged+=this._EventHandler;
            return _result;
        }

        /// <summary>
        /// Обработчик событий изменения состояния.
        /// </summary>
        /// <param name="sender">Истояник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void _EventHandler(object sender, AutomatonEventArgs e) {
            this.Current=this._edges[this.Current.GetType()][e.Name];
        }

        /// <summary>
        /// Возвращает или устанавливает текущее состояние.
        /// </summary>
        public T Current {
            get;
            internal protected set;
        }
    }

    /// <summary>
    /// Аргументы событий автомата.
    /// </summary>
    internal class AutomatonEventArgs:EventArgs {

        /// <summary>
        /// Initializes a new instance of the <see cref="AutomatonEventArgs"/> class.
        /// </summary>
        /// <param name="name">Имя события.</param>
        public AutomatonEventArgs(string name) {
            this.Name=name;
        }

        /// <summary>
        /// Возвращает имя события.
        /// </summary>
        public string Name {
            get;
            private set;
        }
    }
}
