using System.IO;
using System.Reflection;

namespace UltraMapper.Csv.Tests
{
    public static class Resources
    {
        public static string GetFileLocation( string fileName )
        {
            var path = Path.GetDirectoryName( Assembly.GetExecutingAssembly().Location );
            return Path.Combine( path, "Resources", fileName );
        }
    }
}