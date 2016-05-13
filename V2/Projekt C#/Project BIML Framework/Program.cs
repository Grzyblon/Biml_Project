using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
using System.Xml;

namespace BIML_Project
{
    public class Program
    {
        static void Main(string[] args)
        {
            Biml newBiml = BimlFactory(); // <- Tych fabryk może być wiele w zależności od tego ile potrzebujemy szabonów do pakietów

            Serialize(newBiml);
        }
                
        static public void Serialize(Biml biml)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.Indent = true;

            var writer = new StreamWriter(@"C:\Users\Administrator\Desktop\Projekt BIML\Framework\Framework\biml.biml");
            XmlWriter xmlWriter = XmlWriter.Create(writer, settings);

            XmlSerializerNamespaces names = new XmlSerializerNamespaces();
            names.Add(string.Empty, "http://schemas.varigence.com/biml.xsd");

            var serializer = new XmlSerializer(typeof(Biml), "http://schemas.varigence.com/biml.xsd");

            serializer.Serialize(xmlWriter, biml, names);
        }
        

        public static Biml BimlFactory()
        {
            var newBiml = new Biml();

            var srcConnection = new OleDbConnection() { Name = "Source", DataSource = ".", InitialCatalog = "AdventureWorks2012" };
            var dstConnection = new OleDbConnection() { Name = "Destination", DataSource = ".", InitialCatalog = "Baza_testowa" };

            newBiml.Connections.Add(srcConnection);
            newBiml.Connections.Add(dstConnection);

            var package = new Package() { Name = "Paczka trzecia", ConstraintMode = ConstraintMode.Linear };

                var sql = new ExecuteSQL() { Name = "Truncate dbo_Person", ConnectionName = "Destination", DirectInput = "Truncate table dbo.Person;" };
                var dft = new Dataflow() { Name = "Person" };

                dft.Transformations.Add(new OleDbSource("Person_Person", "Source", "Person.Person"));
                dft.Transformations.Add(new OleDbDestination("dbo_Person", "Destination", "dbo.Person"));

            package.Tasks.Add(sql);
            package.Tasks.Add(dft);

            newBiml.Packages.Add(package);
            return newBiml;
        }
    }
}
