using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;
using EventPropsClasses;
using EventDBClasses;

using DBCommand = System.Data.SqlClient.SqlCommand;
using System.Data;
using System.ComponentModel;



namespace EventTestClasses
{    

    public class ProductDBTest
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
        public void TestRetrieve()
        {


            ProductSQLDB db = new ProductSQLDB(dataSource);
            ProductProps props = (ProductProps)db.Retrieve(1);

            Assert.AreEqual(1, props.ID);
            Assert.AreEqual("A4CS      ", props.productcode);
            Assert.AreEqual("Murach's ASP.NET 4 Web Programming with C# 2010", props.description);
            Assert.AreEqual(56.50, props.unitprice);
            Assert.AreEqual(4637, props.onhandquantity);

        }

        [Test]
        public void TestRetrieveAll()
        {

            ProductSQLDB db = new ProductSQLDB(dataSource);

            object productList = db.RetrieveAll(typeof(ProductProps));

            Console.WriteLine(productList);


            //There should be 16 different products.
            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(productList))
            {
                string name = descriptor.Name;
                object value = descriptor.GetValue(productList);
                Console.WriteLine("{0}={1}", name, value);
            }

        }

        [Test]
        public void TestCreate()
        {

            ProductSQLDB db = new ProductSQLDB(dataSource);

            ProductProps newProps = new ProductProps();

            
            newProps.productcode = "BB24";
            newProps.description = "Best Book Ever";
            newProps.unitprice = 51M;
            newProps.onhandquantity = 100;
            newProps.ConcurrencyID = 1;

            db.Create(newProps);
            ProductProps retrievedNewProps = (ProductProps)db.Retrieve(17);

            Assert.AreEqual(17, retrievedNewProps.ID);
            Assert.AreEqual("BB24      ", retrievedNewProps.productcode);
            Assert.AreEqual("Best Book Ever", retrievedNewProps.description);
            Assert.AreEqual(51, retrievedNewProps.unitprice);
            Assert.AreEqual(100, retrievedNewProps.onhandquantity);
            Assert.AreEqual(1, retrievedNewProps.ConcurrencyID);                                          
        }

        [Test]
        public void TestDelete()
        {

            ProductSQLDB db = new ProductSQLDB(dataSource);
            ProductProps props = (ProductProps)db.Retrieve(1);

            Assert.IsNotNull(props);

            db.Delete(props);

            ProductProps deleted;

            Assert.Throws<Exception>(() => deleted = (ProductProps)db.Retrieve(1));

        }

        [Test]
        public void TestUpdate()
        {
            ProductSQLDB db = new ProductSQLDB(dataSource);
            ProductProps props = (ProductProps)db.Retrieve(1);

            props.productcode = "DM23";
            props.description = "Dungeon Master Guide";
            props.onhandquantity = 5;


            bool ok = db.Update(props);
            Assert.IsTrue(ok);

            ProductProps propsUpdated = (ProductProps)db.Retrieve(1);

            Assert.AreEqual("DM23", props.productcode);
            Assert.AreEqual("Dungeon Master Guide", props.description);
            Assert.AreEqual(5, props.onhandquantity);

        }


    }
}
