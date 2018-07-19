using Microsoft.CSharp.RuntimeBinder;
using NUnit.Framework;
using ORMi.Sample.Models;
using System;
using System.Linq;

namespace ORMi.Tests
{
    [TestFixture]
    public class SampleTests
    {
        private WMIHelper _Helper;

        [OneTimeSetUp]
        public void SetUp()
            => _Helper = new WMIHelper("root\\CimV2");

        [Test]
        public void TestGenericQuery()
        {
            var devices = _Helper.Query<Device>().ToList();

            Assert.That(devices.Any(), "Found no devices whatsoever");

            var intelOrAMDDevices = devices.Where(p => p.Name != null && 
                                                       (p.Name.Contains("Intel") || p.Name.Contains("AMD")));

            Assert.That(intelOrAMDDevices.Any(), "Found no Intel devices");
        }

        [Test]
        public void TestDynamicExecuteMethod()
        {
            var processes = _Helper.Query<Process>();

            foreach (var p in processes)
            {
                dynamic d = p.GetOwner();

                Assert.DoesNotThrow(() => { uint i = d.ReturnValue; }, "Should have returned a value and set ReturnValue");
                Assert.Throws<RuntimeBinderException>(() => { bool b = d.ThisMemberDoesNotExist; }, "This member, however, should not exist");
            }
        }

        [Ignore("This doesn't actually work for me because I don't have Lnl_Cardholder")]
        [Test]
        public void TestAddInstance()
        {
            Person person = new Person
            {
                FirstName = "John",
                Lastname = "Doe",
                DocumentNumber = "9995",
                Segment = -1,
                Age = 43
            };

            _Helper.AddInstance(person);

            Person queryPersonSingle = _Helper.Query<Person>("SELECT * FROM Lnl_Cardholder WHERE LASTNAME = 'Doe Modified'").SingleOrDefault();

            Assert.IsNull(queryPersonSingle, "John Doe shouldn't have this name yet.");

            person.Lastname = "Doe Modified";

            _Helper.UpdateInstance(person);

            queryPersonSingle = _Helper.Query<Person>("SELECT * FROM Lnl_Cardholder WHERE LASTNAME = 'Doe Modified'").SingleOrDefault();

            Assert.IsNotNull(queryPersonSingle, "John Doe Modified still couldn't be found.");
        }
    }
}
