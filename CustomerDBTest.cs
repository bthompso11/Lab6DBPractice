using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;
using EventPropsClassses;
using EventDBClasses;

using DBCommand = System.Data.SqlClient.SqlCommand;
using System.Data;
using System.ComponentModel;

namespace EventTestClasses
{
    [TestFixture]
    public class CustomerDBTest  
    {
        private string dataSource = "Data Source=DESKTOP-U0DQ0PI\\SQLEXPRESS;Initial Catalog=MMABooksUpdated;Integrated Security=True";

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
        public void TestRetrieve()
        {
           

        CustomerSQLDB db = new CustomerSQLDB(dataSource);
        CustomerProps props = (CustomerProps)db.Retrieve(23);

            Assert.AreEqual(23, props.ID);
            Assert.AreEqual("Newlin, Sherman",props.name);
            Assert.AreEqual("2400 Bel Air, Apt.345", props.address);
            Assert.AreEqual("Broomfield", props.city);
            Assert.AreEqual("CO", props.state);          
            Assert.AreEqual("80020", props.zipcode);
        }

        [Test]
        public void TestUpdate()
        {
            CustomerSQLDB db = new CustomerSQLDB(dataSource);
            CustomerProps props = (CustomerProps)db.Retrieve(23);

            props.state = "NY";
           // props.zipcode = 54321;
            props.name = "Harry";
            

            bool ok = db.Update(props);
            Assert.IsTrue(ok);

            CustomerProps propsUpdated = (CustomerProps)db.Retrieve(23);

            Assert.AreEqual("NY", props.state);

        }

        [Test]
        public void TestDelete()
        {

            CustomerSQLDB db = new CustomerSQLDB(dataSource);
            CustomerProps props = (CustomerProps)db.Retrieve(23);

            Assert.IsNotNull(props);

            db.Delete(props);

            CustomerProps deleted;

            Assert.Throws<Exception>( () =>   deleted = (CustomerProps)db.Retrieve(23) );            
           
        }

        [Test]
        public void TestRetriveAll()
        {
            //RetriveAll returns a list

            CustomerSQLDB db = new CustomerSQLDB(dataSource);

           object customerList = db.RetrieveAll(typeof(CustomerProps));

            Console.WriteLine(customerList);

          

              //1st line of thought
            //somehow call the entire database???
            //CustomerProps customersList = (CustomerProps)db.RetrieveAll();

                //2nd line of thought, but object can't be treated like a list
            //for (int j = 0; j > 700; j++)
            //{
            //    Console.WriteLine(customerList(j));
            //}

                //3rd thought
            // Found this code on the internet
            // looks like it is displaying the total count in the output which
                //I think matches what should be in the database??

            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(customerList))
            {
                string name = descriptor.Name;
                object value = descriptor.GetValue(customerList);
                Console.WriteLine("{0}={1}", name, value);
            }


        }

        [Test]
        public void TestCreate()
        {

            CustomerSQLDB db = new CustomerSQLDB(dataSource);

            CustomerProps newProps = new CustomerProps();

            newProps.ID = 700;
            newProps.name = "Harry";
            newProps.address = "Main Street";
            newProps.city = "zzzz";
            newProps.state = "PA";
            newProps.zipcode = "12345";
            newProps.ConcurrencyID = 1;

            db.Create(newProps);
            CustomerProps retrievedNewProps = (CustomerProps)db.Retrieve(700);


            //Got an error probably something wrong with the sql statments in MSSQL Server
                  //----->  Fixed error - I forgot to add state and zipcode to the SQL statement which
                  //        was informing me that State and zipcode could not be null.
            Assert.AreEqual(700, retrievedNewProps.ID);
            Assert.AreEqual("Harry", retrievedNewProps.name);
            Assert.AreEqual("Main Street", retrievedNewProps.address);
            Assert.AreEqual("zzzz", retrievedNewProps.city);
            Assert.AreEqual("PA", retrievedNewProps.state);
            Assert.AreEqual("12345", retrievedNewProps.zipcode);
            Assert.AreEqual(1, retrievedNewProps.ConcurrencyID);





        }

    }
           

 }

