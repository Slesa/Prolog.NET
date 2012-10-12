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
        #region Fields

        private IList<T> m_items;
        private ObservableCollection<T> m_observableCollection;

        #endregion

        #region Constructors

        public ReadableList()
        {
            ObservableCollection<T> observableCollection = new ObservableCollection<T>();

            m_items = observableCollection;

            m_observableCollection = observableCollection;
            m_observableCollection.CollectionChanged += notifyCollectionChanged_CollectionChanged;
        }

        public ReadableList(IList<T> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }

            m_items = items;

            m_observableCollection = items as ObservableCollection<T>;
            if (m_observableCollection != null)
            {
                m_observableCollection.CollectionChanged += notifyCollectionChanged_CollectionChanged;
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
                ObservableCollection<T> observableCollection = new ObservableCollection<T>(items);

                m_items = observableCollection;

                m_observableCollection = observableCollection;
                m_observableCollection.CollectionChanged += notifyCollectionChanged_CollectionChanged;
            }
            else
            {
                m_items = items;
            }
        }

        #endregion

        #region IReadableList<T> Members

        public int IndexOf(T item)
        {
            return m_items.IndexOf(item);
        }

        public T this[int index]
        {
            get { return m_items[index]; }
        }

        #endregion

        #region IReadableCollection<T> Members

        public int Count
        {
            get { return m_items.Count; }
        }

        public bool Contains(T item)
        {
            return m_items.Contains(item);
        }

        #endregion

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            return m_items.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return m_items.GetEnumerator();
        }

        #endregion

        #region INotifyCollectionChanged Members

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        #endregion

        #region ISerializable Members

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (m_items != null)
            {
                info.AddValue("items", m_items.ToArray());
            }
            if (m_observableCollection != null)
            {
                info.AddValue("isObservable", true);
            }
        }

        #endregion

        #region Hidden Members

        protected IList<T> Items
        {
            get { return m_items; }
        }

        protected void Move(int oldIndex, int newIndex)
        {
            if (m_observableCollection != null)
            {
                m_observableCollection.Move(oldIndex, newIndex);
            }
            else
            {
                T item = m_items[oldIndex];

                m_items.RemoveAt(oldIndex);

                if (newIndex > oldIndex)
                {
                    m_items.Insert(newIndex - 1, item);
                }
                else
                {
                    m_items.Insert(newIndex, item);
                }
            }
        }

        private void RaiseCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (CollectionChanged != null)
            {
                CollectionChanged(this, e);
            }
        }

        #endregion

        #region Event Handlers

        private void notifyCollectionChanged_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            RaiseCollectionChanged(e);
        }

        #endregion
    }
}
