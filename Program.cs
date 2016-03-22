using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;

namespace BIML_Project
{
    public class Program
    {
        static void Main(string[] args)
        {
            Biml newBiml = BimlFactory();
            //newBiml.xmlns = "http://schemas.varigence.com/biml.xsd";
            //newBiml.connections = new List<Connection>();
            //newBiml.connections.Add(Connection.connectionBuilder("Source", "MAREK", "AdventureWorks2012"));
            //newBiml.connections.Add(Connection.connectionBuilder("Destination", "MAREK", "Baza_testowa"));
            Serialize(newBiml);
        }

        static public void Serialize(Biml biml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Biml));
            using (TextWriter writer = new StreamWriter(@"C:\Users\Administrator\Documents\Visual Studio 2013\projects\Biml Project\biml.biml"))
            {
                serializer.Serialize(writer, biml);
            }
        }

        public static Biml BimlFactory()
        {
            Biml newBiml = Biml.initialize();
            newBiml.connections.Add(OleDbConnection.initialize("Source", "MAREK", "AdventureWorks2012"));
            newBiml.connections.Add(OleDbConnection.initialize("Destination", "MAREK", "Baza_testowa"));

            Package package = Package.initialize("Paczka Ein");
            Dataflow dft = Dataflow.initialize("DFT");
            //dft.transformations.Add(OleDbSource.initialize("Source", "Source", "Person.Address"));
            //dft.transformations.Add(OleDbDestination.initialize("Destination", "Destination", "dbo.DestAddress"));

            package.tasks.Add(dft);
            newBiml.packages.Add(package);
            return newBiml;
        }
    }
}
