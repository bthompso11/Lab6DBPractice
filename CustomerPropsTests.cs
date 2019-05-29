using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventPropsClassses;
using NUnit.Framework;

namespace EventTestClasses
{
    [TestFixture]
    public class CustomerPropsTests
    {

        CustomerProps props1;
       
        [SetUp]
        public void SetUp()
        {
            props1 = new CustomerProps();

            props1.ID = 1;
            props1.name = "mickey";
            props1.address = "Main Street";
            props1.city = "Orlando";
            props1.state = "Florida";
            props1.zipcode = "11111";
            props1.ConcurrencyID = 12;



        }


        [Test]
        public void TestClone()
        {      
            // clone object and then edit the original
            // to make sure that the clone is not just a reference to the original

            //Best practice would be to test each item to make sure anything wasn't missed.

            

            CustomerProps props2 = (CustomerProps)props1.Clone();
            Assert.AreNotSame(props1, props2);

            Assert.NotNull(props2);

            props1.name = "Minnie";

            //Tests that the objects are not the same
            Assert.AreNotSame(props1, props2);
            

        }

        [Test]
        public void TestGetStates()
        {

            string xml = props1.GetState();
            //Console.Write will provide an output link to see what is displayed.
            Console.WriteLine(xml);

        }

        [Test]
        public void TestSetState()
        {

            string xml = props1.GetState();

            CustomerProps props2 = new CustomerProps();
            props2.SetState(xml);

            //inadequate way to test this.  Would be best to write IsEqual 
            Assert.AreEqual(props1.name, props2.name);
            Assert.AreEqual(props1.address, props2.address);

        }

    }
}
