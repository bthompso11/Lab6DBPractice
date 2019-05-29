using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;
using EventClasses;


using EventPropsClasses;
using EventDBClasses;
using ToolsCSharp;

using System.Xml;
using System.Xml.Serialization;
using System.IO;


using System.Data;
using System.Data.SqlClient;

using DBCommand = System.Data.SqlClient.SqlCommand;

namespace EventTestClasses
{
    [TestFixture]
    public class ProductTests
    {
        string dataSource = "Data Source=DESKTOP-U0DQ0PI\\SQLEXPRESS;Initial Catalog=MMABooksUpdated;Integrated Security=True";

        [SetUp]
        public void TestResetDatabase()
        {
            ProductSQLDB db = new ProductSQLDB(dataSource);
            DBCommand command = new DBCommand();
            command.CommandText = "usp_testingResetData";
            command.CommandType = CommandType.StoredProcedure;
            db.RunNonQueryProcedure(command);
        }

        [Test]
        public void TestNewEventConstructor()
        {
            // not in Data Store - no id
            Product p = new Product(dataSource);
            Console.WriteLine(p.ToString());
            Assert.Greater(p.ToString().Length, 1);
        }

        [Test]
        public void TestRetrieveFromDataStoreContructor()
        {
            // retrieves from Data Store
            Product p = new Product(1, dataSource);
            Assert.AreEqual(p.ID, 1);
            Assert.AreEqual(p.ProductCode, "A4CS      ");
            Assert.AreEqual(p.Description, "Murach's ASP.NET 4 Web Programming with C# 2010");
            Assert.AreEqual(p.UnitPrice, 56.5000);
            Assert.AreEqual(p.OnHandQuantity, 4637);
            Console.WriteLine(p.ToString());
        }

        [Test]
        public void TestSaveToDataStore()
        {

            //Test failed when I used 
                //Product p = new Product(dataSource); ?????

            Product p = new Product(1,dataSource);
            p.ProductCode = "GR201     ";
            p.Description = "Good Read Test";
            p.UnitPrice = 25.95M;
            p.OnHandQuantity = 25;
            p.Save();

            Product p2 = new Product(p.ID, dataSource);

            Assert.AreEqual(p.ProductCode, p2.ProductCode);
            Assert.AreEqual("Good Read Test", p2.Description);
            Assert.AreEqual(25.95M, p2.UnitPrice);
            Assert.AreEqual(25, p2.OnHandQuantity);
        }

        [Test]
        public void TestUpdate()
        {
            Product p = new Product(1, dataSource);
            p.ProductCode = "GR09";
            p.Description = "Great Read 9th Edition";
            p.OnHandQuantity = 42;
            //Saved updates
            p.Save();

            //Retrieved the updated customer from the database
            Product p2 = new Product(1, dataSource);

            // 3 out of 4 values updated
            Assert.AreEqual(p2.ProductCode, "GR09      ");
            Assert.AreEqual(p2.Description, "Great Read 9th Edition");
            Assert.AreEqual(p2.UnitPrice, 56.50);
            Assert.AreEqual(p2.OnHandQuantity, 42);           
        }

        [Test]
        public void TestDelete()
        {
            Product p = new Product(2, dataSource);
            p.Delete();
            p.Save();
            Assert.Throws<Exception>(() => new Product(2, dataSource));
        }

        [Test]
        public void TestNoRequiredPropertiesNotSet()
        {
            // not in Data Store - userid, title and description must be provided
            Product p = new Product(dataSource);
            Assert.Throws<Exception>(() => p.Save());
        }

        [Test]
        public void TestSomeRequiredPropertiesNotSet()
        {
            // not in Data Store - only a couple of fields must be provided
            Product p = new Product(dataSource);
            Assert.Throws<Exception>(() => p.Save());
            p.Description = "Tester Book";
            Assert.Throws<Exception>(() => p.Save());
            p.ProductCode = "BKTest";
            Assert.Throws<Exception>(() => p.Save());
        }

        [Test]
        public void TestInvalidPropertyUserIDSet()
        {
            Product p = new Product(dataSource);
            Assert.Throws<ArgumentException>(() => p.OnHandQuantity = -1);
        }

        [Test]
        public void TestConcurrencyIssue()
        {
            Product p1 = new Product(1, dataSource);
            Product p2 = new Product(1, dataSource);

            p1.Description = "Updated this first";
            p1.Save();

            p2.Description = "Updated this second";
            Assert.Throws<Exception>(() => p2.Save());
        }
    }



}
