/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;

using Prolog.Code;

namespace Prolog
{
    internal abstract class WamValue : WamReferenceTarget
    {
        #region Constructors

        /// <remarks>
        /// See also CodeValue.Create.
        /// </remarks>
        public static WamValue Create(CodeValue value)
        {
            if (value == null) return WamValueObject.Create(null);

            if (value is CodeValueBoolean) return WamValueBoolean.Create(((CodeValueBoolean)value).Value);
            if (value is CodeValueDateTime) return WamValueDateTime.Create(((CodeValueDateTime)value).Value);
            if (value is CodeValueDouble) return WamValueDouble.Create(((CodeValueDouble)value).Value);
            if (value is CodeValueException) return WamValueException.Create(((CodeValueException)value).Value);
            if (value is CodeValueInteger) return WamValueInteger.Create(((CodeValueInteger)value).Value);
            if (value is CodeValueString) return WamValueString.Create(((CodeValueString)value).Value);
            if (value is CodeValueType) return WamValueType.Create(((CodeValueType)value).Value);

            return WamValueObject.Create(value.Object);
        }

        #endregion

        #region Public Properties

        public abstract object Object { get; }

        #endregion

        #region Public Methods

        public override WamReferenceTarget Dereference()
        {
            return this;
        }

        #endregion
    }
}
