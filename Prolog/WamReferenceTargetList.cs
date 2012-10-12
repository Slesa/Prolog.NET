/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.Collections.Generic;

namespace Prolog
{
    internal sealed class WamReferenceTargetList : IReadableList<WamReferenceTarget>
    {
        #region Fields

        private List<WamReferenceTarget> m_referenceTargets = new List<WamReferenceTarget>();

        #endregion

        #region Constructors

        public WamReferenceTargetList()
        {
            m_referenceTargets = new List<WamReferenceTarget>();
        }

        public WamReferenceTargetList(IEnumerable<WamReferenceTarget> items)
        {
            m_referenceTargets = new List<WamReferenceTarget>(items);
        }

        #endregion

        #region IReadableList<ReferenceTarget> Members

        public int IndexOf(WamReferenceTarget item)
        {
            return m_referenceTargets.IndexOf(item);
        }

        public WamReferenceTarget this[int index]
        {
            get
            {
                if (index < 0)
                {
                    throw new ArgumentOutOfRangeException("index");
                }

                if (index < m_referenceTargets.Count)
                {
                    return m_referenceTargets[index];
                }

                return null;
            }
            set
            {
                if (index < 0)
                {
                    throw new ArgumentOutOfRangeException("index");
                }

                while (m_referenceTargets.Count <= index)
                {
                    m_referenceTargets.Add(null);
                }

                m_referenceTargets[index] = value;
            }
        }

        #endregion

        #region IReadableCollection<ReferenceTarget> Members

        public int Count
        {
            get
            {
                return m_referenceTargets.Count;
            }
        }

        public bool Contains(WamReferenceTarget item)
        {
            return m_referenceTargets.Contains(item);
        }

        #endregion

        #region IEnumerable<ReferenceTarget> Members

        public IEnumerator<WamReferenceTarget> GetEnumerator()
        {
            return m_referenceTargets.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Internal Members

        internal void Clear()
        {
            m_referenceTargets.Clear();
        }

        internal void Copy(WamReferenceTargetList referenceTargets)
        {
            Clear();

            for (int index = 0; index < referenceTargets.Count; ++index)
            {
                this[index] = referenceTargets[index];
            }
        }

        #endregion
    }
}
