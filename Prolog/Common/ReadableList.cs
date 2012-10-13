/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.Serialization;

namespace Prolog
{
    [Serializable]
    public class ReadableList<T> : IReadableList<T>, INotifyCollectionChanged, ISerializable
    {
        readonly IList<T> _items;
        readonly ObservableCollection<T> _observableCollection;

        public ReadableList()
        {
            var observableCollection = new ObservableCollection<T>();

            _items = observableCollection;

            _observableCollection = observableCollection;
            _observableCollection.CollectionChanged += notifyCollectionChanged_CollectionChanged;
        }

        public ReadableList(IList<T> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }

            _items = items;

            _observableCollection = items as ObservableCollection<T>;
            if (_observableCollection != null)
            {
                _observableCollection.CollectionChanged += notifyCollectionChanged_CollectionChanged;
            }
        }

        protected ReadableList(SerializationInfo info, StreamingContext context)
        {
            T[] items;
            try
            {
                items = (T[])info.GetValue("items", typeof(T[]));
            }
            catch (SerializationException)
            {
                items = new T[0];
            }

            bool isObservable;
            try
            {
                isObservable = info.GetBoolean("isObservable");
            }
            catch (SerializationException)
            {
                isObservable = false;
            }

            if (isObservable)
            {
                var observableCollection = new ObservableCollection<T>(items);

                _items = observableCollection;

                _observableCollection = observableCollection;
                _observableCollection.CollectionChanged += notifyCollectionChanged_CollectionChanged;
            }
            else
            {
                _items = items;
            }
        }

        public int IndexOf(T item)
        {
            return _items.IndexOf(item);
        }

        public T this[int index]
        {
            get { return _items[index]; }
        }

        public int Count
        {
            get { return _items.Count; }
        }

        public bool Contains(T item)
        {
            return _items.Contains(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (_items != null)
            {
                info.AddValue("items", _items.ToArray());
            }
            if (_observableCollection != null)
            {
                info.AddValue("isObservable", true);
            }
        }

        protected IList<T> Items
        {
            get { return _items; }
        }

        protected void Move(int oldIndex, int newIndex)
        {
            if (_observableCollection != null)
            {
                _observableCollection.Move(oldIndex, newIndex);
            }
            else
            {
                T item = _items[oldIndex];

                _items.RemoveAt(oldIndex);

                if (newIndex > oldIndex)
                {
                    _items.Insert(newIndex - 1, item);
                }
                else
                {
                    _items.Insert(newIndex, item);
                }
            }
        }

        void RaiseCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (CollectionChanged != null)
            {
                CollectionChanged(this, e);
            }
        }

        void notifyCollectionChanged_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            RaiseCollectionChanged(e);
        }
    }
}
