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
    public class CustomerTests
    {
        string dataSource = "Data Source=DESKTOP-U0DQ0PI\\SQLEXPRESS;Initial Catalog=MMABooksUpdated;Integrated Security=True";

        [SetUp]
        public void TestResetDatabase()
        {
            CustomerSQLDB db = new CustomerSQLDB(dataSource);
            DBCommand command = new DBCommand();
            command.CommandText = "usp_testingResetData";
            command.CommandType = CommandType.StoredProcedure;
            db.RunNonQueryProcedure(command);
        }
        

        [Test]
        public void TestNewEventConstructor()
        {
            // not in Data Store - no id
            Customer c = new Customer(dataSource);
            Console.WriteLine(c.ToString());
            Assert.Greater(c.ToString().Length, 1);
        }

        [Test]
        public void TestRetrieveFromDataStoreContructor()
        {
            // retrieves from Data Store
            Customer c = new Customer(1, dataSource);
            Assert.AreEqual(c.ID, 1);
            Assert.AreEqual(c.Name, "Molunguri, A");
            Assert.AreEqual(c.Address, "1108 Johanna Bay Drive");
            Assert.AreEqual(c.City, "Birmingham");
            Assert.AreEqual(c.State, "AL");
            Assert.AreEqual(c.Zipcode, "35216-6909");
            Console.WriteLine(c.ToString());
        }

        [Test]
        public void TestSaveToDataStore()
        {
            Customer c = new Customer(dataSource);
            c.Name = "Third Event";
            c.Address = "This is the third event in my event list.";
            c.City = "zzzz";
            c.State = "PA";
            c.Zipcode = "12345-1234";
            c.Save();

            Customer c2 = new Customer(c.ID, dataSource);


            Assert.AreEqual("Third Event", c2.Name);
            Assert.AreEqual("zzzz", c2.City);
            Assert.AreEqual("PA", c2.State);
            Assert.AreEqual("12345-1234", c2.Zipcode);
        }

        [Test]
        public void TestUpdate()
        {
            Customer c = new Customer(1, dataSource);
            c.Name = "Jon Smith";
            c.Address = "Main Street";
            c.State = "PA";
            //Saved updates
            c.Save();

            //Retrieved the updated customer from the database
            Customer c2 = new Customer(1, dataSource);

            // 3 out of 6 values updated
            Assert.AreEqual(c2.Name, "Jon Smith");
            Assert.AreEqual(c2.Address, "Main Street");
            Assert.AreEqual(c2.City, "Birmingham");
            Assert.AreEqual(c2.State, "PA");
            Assert.AreEqual(c2.Zipcode, "35216-6909");
        }

        [Test]
        public void TestDelete()
        {
            Customer e = new Customer(2, dataSource);
            e.Delete();
            e.Save();
            Assert.Throws<Exception>(() => new Customer(2, dataSource));
        }

        [Test]
        public void TestNoRequiredPropertiesNotSet()
        {
            // not in Data Store - userid, title and description must be provided
            Customer e = new Customer(dataSource);
            Assert.Throws<Exception>(() => e.Save());
        }

        [Test]
        public void TestSomeRequiredPropertiesNotSet()
        {
            // not in Data Store - only a couple of fields must be provided
            Customer c = new Customer(dataSource);
            Assert.Throws<Exception>(() => c.Save());
            c.Address = "123 Main street";
            Assert.Throws<Exception>(() => c.Save());
            c.Name = "this is a test";
            Assert.Throws<Exception>(() => c.Save());
        }

        [Test]
        public void TestInvalidPropertyUserIDSet()
        {
            Customer c = new Customer(dataSource);
            Assert.Throws<ArgumentException>(() => c.State = "PAS");
        }

        [Test]
        public void TestConcurrencyIssue()
        {
            Customer c1 = new Customer(1, dataSource);
            Customer c2 = new Customer(1, dataSource);

            c1.Name = "Updated this first";
            c1.Save();

            c2.Name = "Updated this second";
            Assert.Throws<Exception>(() => c2.Save());
        }
    }
}
