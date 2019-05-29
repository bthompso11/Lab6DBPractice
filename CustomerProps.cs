using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
using ToolsCSharp;

// DBDataReader is being used as an alias.   
//if wanted to make an oracle reader all that would need to change is the .SqlDataReader
using DBDataReader = System.Data.SqlClient.SqlDataReader;

namespace EventPropsClassses
{
    /// <summary>
    /// Setting the values for the CustomerProps class
    /// See indiviual instance variables or any specific instructsions
    /// </summary>
    [Serializable()]
    public class CustomerProps : IBaseProps
    {

        #region instance variables
        /// <summary>
        /// 
        /// </summary>
        public int ID = Int32.MinValue;

      
        /// <summary>
        /// 
        /// </summary>
        public string name = "";

        /// <summary>
        /// 
        /// </summary>
        public string address = "";

        /// <summary>
        /// 
        /// </summary>
        public string city = "";

        /// <summary>
        /// 
        /// </summary>
        public string state = "";

        /// <summary>
        /// 
        /// </summary>
        public string zipcode = "";

        /// <summary>
        /// ConcurrencyID. See main docs, don't manipulate directly
        /// </summary>
        public int ConcurrencyID = 0;
        #endregion

        #region constructor
        /// <summary>
        /// Constructor. This object should only be instantiated by Customer, not used directly.
        /// </summary>
        public CustomerProps()
        {
        }

        #endregion

        #region BaseProps Members
        /// <summary>
        /// Serializes this props object to XML, and writes the key-value pairs to a string.
        /// </summary>
        /// <returns>String containing key-value pairs</returns>	
        public string GetState()
        {
            XmlSerializer serializer = new XmlSerializer(this.GetType());
            StringWriter writer = new StringWriter();
            serializer.Serialize(writer, this);
            return writer.GetStringBuilder().ToString();
        }

        // I don't always want to generate xml in the db class so the 
        // props class can read in from xml
        public void SetState(DBDataReader dr)
        {
            this.ID = (Int32)dr["CustomerID"];
            this.name = (string)dr["name"];
            this.address = (string)dr["address"];
            this.city = (string)dr["city"];
            this.state = (string)dr["state"];
            this.zipcode = ((string)dr["zipcode"]).Trim();
            this.ConcurrencyID = (Int32)dr["ConcurrencyID"];
        }

        /// <summary>
        /// Sets the state which will allow it to be read
        /// </summary>
        public void SetState(string xml)
        {
            XmlSerializer serializer = new XmlSerializer(this.GetType());
            StringReader reader = new StringReader(xml);
            CustomerProps p = (CustomerProps)serializer.Deserialize(reader);
            this.ID = p.ID;
            this.name = p.name;
            this.address= p.address;
            this.city = p.city;
            this.state = p.state;
            this.zipcode = p.zipcode;
            this.ConcurrencyID = p.ConcurrencyID;
        }
        #endregion

        #region ICloneable Members
        /// <summary>
        /// Clones this object.
        /// </summary>
        /// <returns>A clone of this object.</returns>
        public Object Clone()
        {
            CustomerProps p = new CustomerProps();
            p.ID = this.ID;
            p.name = this.name;
            p.address = this.address;
            p.state = this.state;
            p.city = this.city;
            p.zipcode = this.zipcode;
            p.ConcurrencyID = this.ConcurrencyID;
            return p;
        }
        #endregion

    }
}



