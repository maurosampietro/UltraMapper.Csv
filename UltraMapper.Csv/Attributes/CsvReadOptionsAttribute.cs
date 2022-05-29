using System;

namespace UltraMapper.Csv
{
    public sealed class CsvReadOptionsAttribute : Attribute
    {
        public bool IsIgnored { get; set; }
        public bool IsRequired { get; set; }
        public int Order { get; set; }
        public string Name { get; set; }

        public object FillInValue { get; set; }
        public bool TrimWhitespaces { get; set; }
        public char TrimChar { get; set; } = '\0';

        /// <summary>
        /// Remove the first leading and the last trailing
        /// quotation-char occurence if any is found as the very 
        /// first and the very last char in the string. 
        /// 
        /// You may also want to configure <see cref="Trim"/>
        /// or <see cref="TrimWhitespaces"/> to help this work. 
        /// </summary>
        public bool Unquote { get; set; }

        /// <summary>
        /// Replaces a pair of double-quotes inside a quoted string
        /// with a single double-quote.
        /// </summary>
        public bool UnescapeQuotes { get; set; }

        /// <summary>
        /// Calls a custom method for further preprocessing.
        /// The method is searched in the class you are applying this attribute.
        /// The method signature must be 'void Preprocess( string data[] )'.
        /// You can override the method name by setting <see cref="CustomPreprocessMethodName"/>.
        /// </summary>
        public bool CustomPreprocess { get; set; }

        /// <summary>
        /// Allows you to override the default name for the <see cref="CustomPreprocess"/> method.
        /// </summary>
        public string CustomPreprocessMethodName { get; set; } = "Preprocess";
    }
}
