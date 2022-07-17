using System.Diagnostics;

namespace UltraMapper.Csv.Tests.Biostats
{
    [CustomPreprocess( IsEnabled = false )]
    internal class Titanic
    {
        public int PassengerId { get; set; }
        public bool Survived { get; set; }
        public short Pclass { get; set; }
        public string Name { get; set; }
        public string Sex { get; set; }

        [CsvFieldOptions( FillInValue = "0" )]
        public float Age { get; set; }
        public int SibSp { get; set; }
        public int Parch { get; set; }
        public string Ticket { get; set; }
        public double Fare { get; set; }
        public string Cabin { get; set; }
        public string Embarked { get; set; }

        public void Preprocess( string[] record )
        {
            Debugger.Break();
        }
    }
}