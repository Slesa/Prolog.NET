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
        #region Fields

        private static string m_codeSentenceDataFormat = "CodeSentenceDataFormat";

        private object m_object;
        private string m_format;
        private bool m_autoConvert;

        #endregion

        #region Constructors

        public CodeSentenceDataObject(CodeSentence codeSentence)
        {
            if (codeSentence == null)
            {
                throw new ArgumentNullException("codeSentence");
            }

            m_object = codeSentence;
            m_format = CodeSentenceDataFormat;
            m_autoConvert = true;
        }

        #endregion

        #region Public Properties

        public static string CodeSentenceDataFormat
        {
            get { return m_codeSentenceDataFormat; }
        }

        #endregion

        #region IDataObject Members

        public object GetData(string format, bool autoConvert)
        {
            if (format != m_format)
            {
                if (!m_autoConvert || !autoConvert)
                {
                    return null;
                }
            }

            if (format == CodeSentenceDataFormat)
            {
                if (m_format == CodeSentenceDataFormat)
                {
                    CodeSentence codeSentenceValue = m_object as CodeSentence;
                    return codeSentenceValue;
                }

                if (m_format == DataFormats.StringFormat
                    || m_format == DataFormats.UnicodeText)
                {
                    string stringValue = m_object as string;
                    if (stringValue != null)
                    {
                        CodeSentence[] codeSentences = Parser.Parse(stringValue);
                        if (codeSentences != null
                            && codeSentences.Length >= 1)
                        {
                            return codeSentences[0];
                        }
                    }
                    return null;
                }
            }

            if (format == DataFormats.StringFormat
                || format == DataFormats.UnicodeText)
            {
                if (m_format == CodeSentenceDataFormat)
                {
                    CodeSentence codeSentenceValue = m_object as CodeSentence;
                    return codeSentenceValue.ToString();
                }

                if (m_format == DataFormats.StringFormat
                    || m_format == DataFormats.UnicodeText)
                {
                    string stringValue = m_object as string;
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
            if (format == m_format)
            {
                return true;
            }

            // Conversion not allowed...
            //
            if (!m_autoConvert || !autoConvert)
            {
                return false;
            }

            // Convertable formats...
            //
            if (format == CodeSentenceDataFormat
                || format == DataFormats.StringFormat
                || format == DataFormats.UnicodeText)
            {
                return true;
            }

            return false;
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
            if (!autoConvert)
            {
                return new string[] { m_format };
            }

            return new string[] { CodeSentenceDataFormat, DataFormats.StringFormat, DataFormats.UnicodeText };
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

            m_format = format;
            m_object = data;
            m_autoConvert = autoConvert;
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

        #endregion
    }
}
