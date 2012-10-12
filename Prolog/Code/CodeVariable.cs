/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.Xml.Linq;

namespace Prolog.Code
{
    /// <summary>
    /// Represents a variable.
    /// </summary>
    [Serializable]
    public sealed class CodeVariable : CodeTerm, IEquatable<CodeVariable>, IImmuttable
    {
        #region Fields

        public new const string ElementName = "CodeVariable";

        private string m_name;

        #endregion

        #region Constructors

        public CodeVariable(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            m_name = name;
        }

        public new static CodeVariable Create(XElement xCodeVariable)
        {
            string name = xCodeVariable.Value;

            return new CodeVariable(name);
        }

        #endregion

        #region Public Properties

        public string Name
        {
            get { return m_name; }
        }

        public override bool IsCodeVariable
        {
            get { return true; }
        }

        #endregion

        #region Public Methods

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            CodeVariable rhs = obj as CodeVariable;
            if (rhs == null) return false;

            return Name == rhs.Name;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public static bool operator ==(CodeVariable lhs, CodeVariable rhs)
        {
            if (object.ReferenceEquals(lhs, rhs)) return true;

            if (object.ReferenceEquals(lhs, null) || object.ReferenceEquals(rhs, null)) return false;

            return lhs.Equals(rhs);
        }

        public static bool operator !=(CodeVariable lhs, CodeVariable rhs)
        {
            return !(lhs == rhs);
        }

        public override string ToString()
        {
            return Name;
        }

        public override XElement ToXElement()
        {
            return ToXElementBase(
                new XElement(ElementName, Name));
        }

        #endregion

        #region IEquatable<CodeVariable> Members

        public override bool Equals(CodeTerm other)
        {
            return Equals(other as CodeVariable);
        }

        public bool Equals(CodeVariable other)
        {
            if (object.ReferenceEquals(other, null)) return false;

            return Name == other.Name;
        }

        #endregion
    }
}
