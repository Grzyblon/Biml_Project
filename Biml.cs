using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BIML_Project
{
    [XmlRoot("Biml")]
    public class Biml
    {
        [XmlArray("Connections")]
        public List<OleDbConnection> connections { get; set; }

        [XmlArray("Packages")]
        public List<Package> packages { get; set; }

        public static Biml initialize()
        {
            Biml biml = new Biml();
            biml.connections = new List<OleDbConnection>();
            biml.packages = new List<Package>();
            return biml;
        }
    }

    public class OleDbConnection
    {
        private static string provider = "SQLNCLI11.1";
        private static string integratedSecurity = "SSPI";

        [XmlAttribute("Name")]
        public string name { get; set; }

        [XmlAttribute("ConnectionString")]
        public string connectionString { get; set; }

        public static OleDbConnection initialize(string name, string dataSource, string initialCatalog)
        {
            OleDbConnection con = new OleDbConnection();
            con.name = name;
            con.connectionString = String.Format("Data Source={0};Initial Catalog={1};Provider={2};Integrated Security={3};", dataSource, initialCatalog, provider, integratedSecurity);
            return con;
        }
    }

    public class Package
    {
        [XmlAttribute("Name")]
        public string name { get; set; }

        [XmlElement("Tasks")]
        public List<Task> tasks { get; set; }

        public static Package initialize(string name)
        {
            Package package = new Package();
            package.name = name;
            package.tasks = new List<Task>();
            return package;
        }
    }

    [XmlRoot("Task")]
    public abstract class Task
    {
        
    }

    [XmlRoot("Dataflow")]
    public class Dataflow : Task
    {
        [XmlAttribute("Name")]
        public string name { get; set; }

        [XmlArray("Transformations")]
        public List<Transformation> transformations { get; set; }

        public static Dataflow initialize(string name)
        {
            Dataflow dataflow = new Dataflow();
            dataflow.name = name;
            dataflow.transformations = new List<Transformation>();
            return dataflow;
        }
    }

    public abstract class Transformation
    {
    }

    public class OleDbSource : Transformation
    {
        [XmlAttribute("Name")]
        public string name { get; set; }

        [XmlAttribute("ConnectionName")]
        public string connectionName;

        [XmlElement("ExternalTableInput")]
        public ExternalTable externalTableInput { get; set; }

        public static OleDbSource initialize(string name, string connectionName, string tableName)
        {
            OleDbSource trans = new OleDbSource();
            trans.name = name;
            trans.connectionName = connectionName;
            trans.externalTableInput = ExternalTable.initialize(tableName);
            return trans;
        }
    }

    public class OleDbDestination : Transformation
    {
        [XmlAttribute("Name")]
        public string name { get; set; }

        [XmlAttribute("ConnectionName")]
        public string connectionName;

        [XmlElement("ExternalTableOutput")]
        public ExternalTable externalTableOutput { get; set; }

        public static OleDbDestination initialize(string name, string connectionName, string tableName)
        {
            OleDbDestination trans = new OleDbDestination();
            trans.name = name;
            trans.connectionName = connectionName;
            trans.externalTableOutput = ExternalTable.initialize(tableName);
            return trans;
        }
    }

    public class ExternalTable
    {
        [XmlAttribute("Table")]
        public string table { get; set; }

        public static ExternalTable initialize(string tableName)
        {
            ExternalTable table = new ExternalTable();
            table.table = tableName;
            return table;
        }
    }
}
