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

namespace EventPropsClasses
{

    /// <summary>
    /// Setting the values for the CustomerProps class
    /// See indiviual instance variables or any specific instructsions
    /// </summary>
    [Serializable()]
    public class ProductProps : IBaseProps
    {

        #region instance variables
        /// <summary>
        /// 
        /// </summary>
        public int ID = Int32.MinValue;

   
        /// <summary>
        /// 
        /// </summary>
        public string productcode = "";

        /// <summary>
        /// 
        /// </summary>
        public string description = "";

        /// <summary>
        /// 
        /// </summary>
        public decimal unitprice = 0;

        /// <summary>
        /// 
        /// </summary>
        public int onhandquantity = 0;

        /// <summary>
        /// ConcurrencyID. See main docs, don't manipulate directly
        /// </summary>
        public int ConcurrencyID = 0;
        #endregion

        #region constructor
        /// <summary>
        /// Constructor. This object should only be instantiated by Customer, not used directly.
        /// </summary>
        public ProductProps()
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
            this.ID = (Int32)dr["ProductID"];
            this.productcode = (string)dr["ProductCode"];
            this.unitprice = (decimal)dr["UnitPrice"];
            this.description = (string)dr["Description"];
            this.onhandquantity = (Int32)dr["OnHandQuantity"];
            this.ConcurrencyID = (Int32)dr["ConcurrencyID"];
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetState(string xml)
        {
            XmlSerializer serializer = new XmlSerializer(this.GetType());
            StringReader reader = new StringReader(xml);
            ProductProps p = (ProductProps)serializer.Deserialize(reader);
            this.ID = p.ID;
            this.productcode = p.productcode;
            this.unitprice = p.unitprice;
            this.onhandquantity = p.onhandquantity;
            this.description = p.description;
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
            ProductProps p = new ProductProps();
            p.ID = this.ID;
            p.productcode = this.productcode;
            p.unitprice = this.unitprice;
            p.onhandquantity = this.onhandquantity;
            p.description = this.description;
            p.ConcurrencyID = this.ConcurrencyID;
            return p;
        }
        #endregion

    }
}
