/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.Collections.Generic;

namespace Prolog
{
    internal sealed class WamReferenceTargetList : IReadableList<WamReferenceTarget>
    {
        readonly List<WamReferenceTarget> _referenceTargets = new List<WamReferenceTarget>();

        public WamReferenceTargetList()
        {
            _referenceTargets = new List<WamReferenceTarget>();
        }

        public WamReferenceTargetList(IEnumerable<WamReferenceTarget> items)
        {
            _referenceTargets = new List<WamReferenceTarget>(items);
        }

        public int IndexOf(WamReferenceTarget item)
        {
            return _referenceTargets.IndexOf(item);
        }

        public WamReferenceTarget this[int index]
        {
            get
            {
                if (index < 0)
                {
                    throw new ArgumentOutOfRangeException("index");
                }
                return index < _referenceTargets.Count ? _referenceTargets[index] : null;
            }
            set
            {
                if (index < 0)
                {
                    throw new ArgumentOutOfRangeException("index");
                }
                while (_referenceTargets.Count <= index)
                {
                    _referenceTargets.Add(null);
                }
                _referenceTargets[index] = value;
            }
        }

        public int Count
        {
            get { return _referenceTargets.Count; }
        }

        public bool Contains(WamReferenceTarget item)
        {
            return _referenceTargets.Contains(item);
        }


        public IEnumerator<WamReferenceTarget> GetEnumerator()
        {
            return _referenceTargets.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        internal void Clear()
        {
            _referenceTargets.Clear();
        }

        internal void Copy(WamReferenceTargetList referenceTargets)
        {
            Clear();

            for (int index = 0; index < referenceTargets.Count; ++index)
            {
                this[index] = referenceTargets[index];
            }
        }
    }
}
