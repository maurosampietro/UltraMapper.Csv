using System;

namespace UltraMapper.Csv
{
    [AttributeUsage( AttributeTargets.Class, AllowMultiple = false )]
    public class CustomPreprocessAttribute : Attribute
    {
        /// <summary>
        /// Calls a custom method for further preprocessing.
        /// If set to true, the method is searched in the class you are applying this attribute on.
        /// The method signature must be 'void Preprocess( string data[] )'.
        /// You can override the method name by setting <see cref="MethodName"/>.
        /// The default value for this property is set to true.
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// Allows you to override the default name of the method to invoke.
        /// </summary>
        public string MethodName { get; set; } = "Preprocess";
    }
}
