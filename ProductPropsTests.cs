using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using EventPropsClasses;

namespace EventTestClasses
{
    [TestFixture]
    public class ProductPropsTests
    {
        ProductProps product1;

        string xml;

        [SetUp]
        public void Setup()
        {

            product1 = new ProductProps();

            product1.ID = 1;
            product1.productcode = "1234";
            product1.description = "thing";
            product1.unitprice = 100.00M;
            product1.onhandquantity = 25;
            product1.ConcurrencyID = 12;

            xml = product1.GetState();

        }

        [Test]
        public void TestClone()
        {
            
            ProductProps product2 = (ProductProps)product1.Clone();

            Assert.NotNull(product2);

            Assert.AreNotSame(product1, product2);

            Console.WriteLine(product1.description);
            Console.WriteLine(product2.description);       
                   
        }

        [Test]
        public void TestGetState()
        {

            string xml = product1.GetState();
            Console.WriteLine(xml);


        }

        [Test]
        public void TestSetSate()
        {

            ProductProps product2 = new ProductProps();

            product2.SetState(xml);

            Assert.AreEqual(product1.description, product2.description);
            Assert.AreEqual(product1.unitprice, product2.unitprice);


        }



    }
}
