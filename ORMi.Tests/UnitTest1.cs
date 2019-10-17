using Microsoft.CSharp.RuntimeBinder;
using NUnit.Framework;
using Models = ORMi.Sample.Models;
using System;
using System.Linq;
using System.Security.Principal;

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
        public void TestGenericQuery_Devices()
        {
            var devices = _Helper.Query<Models.Device>().ToList();

            Assert.That(devices.Any(), "Found no devices whatsoever");

            var intelOrAMDDevices = devices.Where(p => p.Name != null && 
                                                       (p.Name.Contains("Intel") || p.Name.Contains("AMD")));

            Assert.That(intelOrAMDDevices.Any(), "Found no Intel devices");
        }

        [Test]
        public void TestDateTimeSerialization_Process()
        {
            var processes = _Helper.Query<Models.Process>().ToList();

            //Assert.That(devices.Any(), "Found no devices whatsoever");

            //var intelOrAMDDevices = devices.Where(p => p.Name != null &&
            //                                           (p.Name.Contains("Intel") || p.Name.Contains("AMD")));

            //Assert.That(intelOrAMDDevices.Any(), "Found no Intel devices");
        }

        [Test]
        public void TestGenericQuery_OperatingSystem()
        {
            var os = _Helper.Query<Models.OperatingSystem>().FirstOrDefault();

            Assert.Greater(os.LastBootUpTime, new DateTime(2018, 7, 18));
            Assert.Less(os.LastBootUpTime, DateTime.Now);

            //Assert.That(devices.Any(), "Found no devices whatsoever");

            //var intelOrAMDDevices = devices.Where(p => p.Name != null &&
            //                                           (p.Name.Contains("Intel") || p.Name.Contains("AMD")));

            //Assert.That(intelOrAMDDevices.Any(), "Found no Intel devices");
        }

        [Test]
        public void TestDynamicExecuteMethod()
        {
            var processes = _Helper.Query<Models.Process>();

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
            Models.Person person = new Models.Person
            {
                FirstName = "John",
                Lastname = "Doe",
                DocumentNumber = "9995",
                Segment = -1,
                Age = 43
            };

            _Helper.AddInstance(person);

            Models.Person queryPersonSingle = _Helper.Query<Models.Person>("SELECT * FROM Lnl_Cardholder WHERE LASTNAME = 'Doe Modified'").SingleOrDefault();

            Assert.IsNull(queryPersonSingle, "John Doe shouldn't have this name yet.");

            person.Lastname = "Doe Modified";

            _Helper.UpdateInstance(person);

            queryPersonSingle = _Helper.Query<Models.Person>("SELECT * FROM Lnl_Cardholder WHERE LASTNAME = 'Doe Modified'").SingleOrDefault();

            Assert.IsNotNull(queryPersonSingle, "John Doe Modified still couldn't be found.");
        }

        [Test]
        public void TestRecursiveQuery_UserProfiles()
        {
            var profile = _Helper.Query<Models.UserProfile>("SELECT * FROM Win32_UserProfile WHERE Loaded = 'True'")
                .Where(p => p.SID == WindowsIdentity.GetCurrent().User.Value).SingleOrDefault();

            Assert.AreEqual(WindowsIdentity.GetCurrent().User.Value, profile.SID);
            Assert.IsNotNull(profile.AppDataRoaming);
            Assert.IsNotNull(profile.Contacts);
            Assert.IsNotNull(profile.Desktop);
            Assert.IsNotNull(profile.Documents);
            Assert.IsNotNull(profile.Downloads);
            Assert.IsNotNull(profile.Favorites);
            Assert.IsNotNull(profile.Links);
            Assert.IsNotNull(profile.Music);
            Assert.IsNotNull(profile.Pictures);
            Assert.IsNotNull(profile.SavedGames);
            Assert.IsNotNull(profile.Searches);
            Assert.IsNotNull(profile.StartMenu);
            Assert.IsNotNull(profile.Videos);
        }
    }
}
