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
        public new const string ElementName = "CodeVariable";

        public CodeVariable(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            Name = name;
        }

        public new static CodeVariable Create(XElement xCodeVariable)
        {
            var name = xCodeVariable.Value;
            return new CodeVariable(name);
        }

        public string Name { get; private set; }

        public override bool IsCodeVariable
        {
            get { return true; }
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            var rhs = obj as CodeVariable;
            if (rhs == null) return false;

            return Name == rhs.Name;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public static bool operator ==(CodeVariable lhs, CodeVariable rhs)
        {
            if (ReferenceEquals(lhs, rhs)) return true;

            if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null)) return false;

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

        public override bool Equals(CodeTerm other)
        {
            return Equals(other as CodeVariable);
        }

        public bool Equals(CodeVariable other)
        {
            if (ReferenceEquals(other, null)) return false;

            return Name == other.Name;
        }
    }
}
