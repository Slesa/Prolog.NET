/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using Prolog.Code;

namespace Prolog.Library
{
    public static class CoreExpressions
    {
        public static CodeTerm Construct(CodeTerm value)
        {
            CodeValueType codeValueObjectType = value as CodeValueType;
            if (codeValueObjectType == null)
            {
                return null;
            }

            ConstructorInfo constructorInfo = codeValueObjectType.Value.GetConstructor(new Type[] { });
            object objectInstance = constructorInfo.Invoke(new object[] { });

            return new CodeValueObject(objectInstance);
        }
    }
}
