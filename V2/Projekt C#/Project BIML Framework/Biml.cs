using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BIML_Project
{
    public class Biml
    {
        [XmlArray("Connections")]
        public List<OleDbConnection> Connections { get; set; }

        [XmlArray("Packages")]
        public List<Package> Packages { get; set; }
        
        public Biml()
        {            
            Connections = new List<OleDbConnection>();
            Packages = new List<Package>();         
        }
    }

    public class OleDbConnection
    {
        private static string _provider = "SQLNCLI11.1";
        private static string _integratedSecurity = "SSPI";
        
        [XmlIgnore]
        public string DataSource { get; set; }

        [XmlIgnore]
        public string InitialCatalog { get; set; }

        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("ConnectionString")]
        public string ConnectionString
        {
            get
            {
                return String.Format("Data Source={0};Initial Catalog={1};Provider={2};Integrated Security={3};", DataSource, InitialCatalog, _provider, _integratedSecurity);
            }
            set { throw new Exception("Setting OleDbConnection.connectionString directly not allowed"); }
        }
    }

    public enum ConstraintMode { Linear, Parallel }
    public class Package
    {
        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("ConstraintMode")]
        public ConstraintMode ConstraintMode { get; set; }

        [XmlArray("Tasks")]
        [XmlArrayItem(typeof(Dataflow))]
        [XmlArrayItem(typeof(ExecuteSQL))]
        public List<Task> Tasks { get; set; }

        public Package()
        {
            Tasks = new List<Task>();
        }
    }

    // TASKS    
    public abstract class Task {
        [XmlIgnore]
        protected string _prefix = "";
        [XmlIgnore]
        private string _name;
        [XmlAttribute("Name")]
        public string Name
        {
            get { return _prefix + _name; }
            set { _name = value; }
        }
    }

    public class Dataflow : Task
    {
        [XmlArray("Transformations")]
        [XmlArrayItem(typeof(OleDbSource))]
        [XmlArrayItem(typeof(OleDbDestination))]
        public List<Transformation> Transformations { get; set; }

        public Dataflow()
        {
            base._prefix = "DFT - ";
            Transformations = new List<Transformation>();            
        }
    }

    public class ExecuteSQL : Task
    {
        [XmlAttribute("ConnectionName")]
        public string ConnectionName;

        [XmlElement("DirectInput")]
        public string DirectInput;

        public ExecuteSQL()
        {
            base._prefix = "SQL - ";
        }
    }

    // TRANSFORMATIONS        
    public abstract class Transformation 
    {
        [XmlIgnore]
        protected string _prefix = "";
        [XmlIgnore]
        private string _name;
        [XmlAttribute("Name")]
        public string Name
        {
            get { return _prefix + _name; }
            set { _name = value; }
        }
    }

    public class OleDbSource : Transformation
    {
        [XmlAttribute("ConnectionName")]
        public string ConnectionName;

        [XmlElement("ExternalTableInput")]
        public ExternalTable ExternalTableInput { get; set; }
                
        public OleDbSource(string name, string connectionName, string tableName)
        {
            base._prefix = "OLE_SRC - ";
            this.Name = name;
            this.ConnectionName = connectionName;
            ExternalTableInput = new ExternalTable(tableName);            
        }
        public OleDbSource() { throw new NotImplementedException("use parametrized constructor instead of default one"); }
    }

    public class OleDbDestination : Transformation
    {
        [XmlAttribute("ConnectionName")]
        public string ConnectionName;

        [XmlElement("ExternalTableOutput")]
        public ExternalTable ExternalTableOutput { get; set; }

        public OleDbDestination(string name, string connectionName, string tableName)
        {
            base._prefix = "OLE_DST - ";
            this.Name = name;
            this.ConnectionName = connectionName;
            ExternalTableOutput = new ExternalTable(tableName);
        }
        public OleDbDestination() { throw new NotImplementedException("use parametrized constructor instead of default one"); }
    }


    public class ExternalTable
    {
        [XmlAttribute("Table")]
        public string Table { get; set; }

        public ExternalTable(string tableName)
        {
            this.Table = tableName;
        }
        public ExternalTable() { throw new NotImplementedException("use parametrized constructor instead of default one"); }
    }
}
