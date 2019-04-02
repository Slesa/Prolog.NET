#if WPFCODE
/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.Windows;

using Prolog.Grammar;

namespace Prolog.Code
{
    /// <summary>
    /// Creates a connection between a caller and the data encapsulated by a <see cref="CodeSentence"/> object.
    /// </summary>
    [Serializable]
    public sealed class CodeSentenceDataObject : IDataObject
    {
        static string _codeSentenceDataFormat = "CodeSentenceDataFormat";

        object _object;
        string _format;
        bool _autoConvert;

        public CodeSentenceDataObject(CodeSentence codeSentence)
        {
            if (codeSentence == null)
            {
                throw new ArgumentNullException("codeSentence");
            }

            _object = codeSentence;
            _format = CodeSentenceDataFormat;
            _autoConvert = true;
        }

        public static string CodeSentenceDataFormat
        {
            get { return _codeSentenceDataFormat; }
        }

        public object GetData(string format, bool autoConvert)
        {
            if (format != _format)
            {
                if (!_autoConvert || !autoConvert)
                {
                    return null;
                }
            }

            if (format == CodeSentenceDataFormat)
            {
                if (_format == CodeSentenceDataFormat)
                {
                    var codeSentenceValue = _object as CodeSentence;
                    return codeSentenceValue;
                }

                if (_format == DataFormats.StringFormat || _format == DataFormats.UnicodeText)
                {
                    var stringValue = _object as string;
                    if (stringValue != null)
                    {
                        var codeSentences = Parser.Parse(stringValue);
                        if (codeSentences != null
                            && codeSentences.Length >= 1)
                        {
                            return codeSentences[0];
                        }
                    }
                    return null;
                }
            }

            if (format == DataFormats.StringFormat || format == DataFormats.UnicodeText)
            {
                if (_format == CodeSentenceDataFormat)
                {
                    var codeSentenceValue = _object as CodeSentence;
                    return codeSentenceValue.ToString();
                }

                if (_format == DataFormats.StringFormat
                    || _format == DataFormats.UnicodeText)
                {
                    var stringValue = _object as string;
                    return stringValue;
                }
            }

            return null;
        }

        public object GetData(Type format)
        {
            if (format == typeof(CodeSentence)) return GetData(CodeSentenceDataFormat, true);
            if (format == typeof(string)) return GetData(DataFormats.StringFormat, true);

            if (format == typeof(object)) return GetData(CodeSentenceDataFormat, true);

            return null;
        }

        public object GetData(string format)
        {
            return GetData(format, true);
        }

        public bool GetDataPresent(string format, bool autoConvert)
        {
            // We can always retrieve the data if the requested format matches the format on the clipboard.
            //
            if (format == _format)
            {
                return true;
            }

            // Conversion not allowed...
            //
            if (!_autoConvert || !autoConvert)
            {
                return false;
            }

            // Convertable formats...
            //
            return format == CodeSentenceDataFormat
                   || format == DataFormats.StringFormat
                   || format == DataFormats.UnicodeText;
        }

        public bool GetDataPresent(Type format)
        {
            if (format == typeof(CodeSentence)) return GetDataPresent(CodeSentenceDataFormat, true);
            if (format == typeof(string)) return GetDataPresent(DataFormats.StringFormat, true);

            if (format == typeof(object)) return GetDataPresent(CodeSentenceDataFormat, true);

            return false;
        }

        public bool GetDataPresent(string format)
        {
            return GetDataPresent(format, true);
        }

        public string[] GetFormats(bool autoConvert)
        {
            return !autoConvert 
                ? new[] { _format } 
                : new[] { CodeSentenceDataFormat, DataFormats.StringFormat, DataFormats.UnicodeText };
        }

        public string[] GetFormats()
        {
            return GetFormats(true);
        }

        public void SetData(string format, object data, bool autoConvert)
        {
            if (string.IsNullOrEmpty(format))
            {
                throw new ArgumentNullException("format");
            }
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            _format = format;
            _object = data;
            _autoConvert = autoConvert;
        }

        public void SetData(Type format, object data)
        {
            if (format == typeof(CodeSentence)) SetData(CodeSentenceDataFormat, data, true);
            if (format == typeof(string)) SetData(DataFormats.StringFormat, data, true);
        }

        public void SetData(string format, object data)
        {
            SetData(format, data, true);
        }

        public void SetData(object data)
        {
            SetData(data.GetType(), data);
        }
    }
}
#endif