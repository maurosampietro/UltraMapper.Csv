using System;
using UltraMapper.Csv.Config.FieldOptions;

namespace UltraMapper.Csv
{
    public sealed class CsvReadOptionsAttribute : Attribute, IFieldConfig
    {
        public bool IsIgnored { get; set; }
        public bool IsRequired { get; set; }
        public int Order { get; set; } = -1;
        public string Name { get; set; }

        public string FillInValue { get; set; }
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

        public string Format { get; set; }
    }
}
