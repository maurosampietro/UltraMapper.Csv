using System;

namespace UltraMapper.Csv
{
    /// <summary>
    /// Controls the call to a method that allows for custom preprocessing of record data.
    /// The method signature must be 'void Preprocess( string data[] )'.
    /// You can override the method name by setting <see cref="MethodName"/>.
    /// </summary>
    [AttributeUsage( AttributeTargets.Class, AllowMultiple = false )]
    public class CustomPreprocessAttribute : Attribute
    {
        /// <summary>       
        /// If set to true, the method is searched.
        /// The default value for this property is set to true.
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// Allows you to override the default name of the method to invoke.
        /// </summary>
        public string MethodName { get; set; } = "Preprocess";
    }
}
