using Newtonsoft.Json;
using NUnit.Framework;
using QbSync.WebConnector.Impl;
using QbSync.WebConnector.Models;
using System;

namespace QbSync.WebConnector.Tests.Impl
{
    [TestFixture]
    class WebConnectorQwcTests
    {
        [Test]
        public void GetQWCFile_Null()
        {
            // Arrange
            var services = new WebConnectorQwc();

            // Act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => services.GetQwcFile(model: null));

            Assert.AreEqual("model", exception.ParamName);
        }

        [TestCase("AppDescription", "{}")]
        [TestCase("AppName", @"{""AppDescription"":""A""}")]
        [TestCase("AppSupport", @"{""AppDescription"":""A"",""AppName"":""A""}")]
        [TestCase("AppURL", @"{""AppDescription"":""A"",""AppName"":""A"",""AppSupport"":""A""}")]
        [TestCase("FileID", @"{""AppDescription"":""A"",""AppName"":""A"",""AppSupport"":""A"",""AppURL"":""A""}")]
        [TestCase("OwnerID", @"{""AppDescription"":""A"",""AppName"":""A"",""AppSupport"":""A"",""AppURL"":""A"",""FileID"":""56e8badb-7364-4e4a-ac08-28b988b3cd74""}")]
        [TestCase("UserName", @"{""AppDescription"":""A"",""AppName"":""A"",""AppSupport"":""A"",""AppURL"":""A"",""FileID"":""56e8badb-7364-4e4a-ac08-28b988b3cd74"",""OwnerID"":""0d400059-de9a-4c67-85f4-dd912ea23398""}")]
        public void GetQWCFile_ArgumentRequired(string paramName, string jsonModel)
        {
            // Arrange
            var services = new WebConnectorQwc();

            var model = JsonConvert.DeserializeObject<WebConnectorQwcModel>(jsonModel);

            // Act and assert
            var exception = Assert.Throws<ArgumentException>(() => services.GetQwcFile(model));

            Assert.AreEqual(paramName, exception.ParamName);
        }

        [Test]
        public void GetQWCFile_SuccessBasic()
        {
            // Arrange
            var services = new WebConnectorQwc();

            var model = new WebConnectorQwcModel
            {
                AppDescription = "Description",
                AppName = "Name",
                AppSupport = "Support",
                AppURL = "URL",
                FileID = Guid.Parse("56e8badb-7364-4e4a-ac08-28b988b3cd74"),
                OwnerID = Guid.Parse("0d400059-de9a-4c67-85f4-dd912ea23398"),
                UserName = "UserName"
            };

            // Act
            var result = services.GetQwcFile(model);

            // Assert
            Assert.AreEqual("<?xml version=\"1.0\"?><QBWCXML><AppDescription>Description</AppDescription><AppID></AppID><AppName>Name</AppName><AppSupport>Support</AppSupport><AppURL>URL</AppURL><FileID>{56e8badb-7364-4e4a-ac08-28b988b3cd74}</FileID><OwnerID>{0d400059-de9a-4c67-85f4-dd912ea23398}</OwnerID><QBType>QBFS</QBType><UserName>UserName</UserName></QBWCXML>", result);
        }

        [Test]
        public void GetQWCFile_SuccessExtended()
        {
            // Arrange
            var services = new WebConnectorQwc();

            var model = new WebConnectorQwcModel
            {
                AppDescription = "Description",
                AppName = "Name",
                AppSupport = "Support",
                AppURL = "URL",
                FileID = Guid.Parse("56e8badb-7364-4e4a-ac08-28b988b3cd74"),
                OwnerID = Guid.Parse("0d400059-de9a-4c67-85f4-dd912ea23398"),
                UserName = "UserName",

                AppDisplayName = "DisplayName",
                AppID = "AppID",
                AppUniqueName = "UniqueName",
                AuthFlags = AuthFlag.SupportQBEnterprise | AuthFlag.SupportQBPremier | AuthFlag.SupportQBPro | AuthFlag.SupportQBSimpleStart,
                IsReadOnly = true,
                Notify = true,
                PersonalDataPrep = PersonalDataPref.PdpRequired,
                QBType = QBType.QBPOS,
                UnattendedModePref = UnattendedModePref.UmpRequired
            };

            // Act
            var result = services.GetQwcFile(model);

            // Assert
            Assert.AreEqual("<?xml version=\"1.0\"?><QBWCXML><AppDescription>Description</AppDescription><AppDisplayName>DisplayName</AppDisplayName><AppID>AppID</AppID><AppName>Name</AppName><AppSupport>Support</AppSupport><AppUniqueName>UniqueName</AppUniqueName><AppURL>URL</AppURL><AuthFlags>0xF</AuthFlags><FileID>{56e8badb-7364-4e4a-ac08-28b988b3cd74}</FileID><IsReadOnly>true</IsReadOnly><Notify>true</Notify><OwnerID>{0d400059-de9a-4c67-85f4-dd912ea23398}</OwnerID><PersonalDataPrep>pdpRequired</PersonalDataPrep><QBType>QBPOS</QBType><UnattendedModePref>umpRequired</UnattendedModePref><UserName>UserName</UserName></QBWCXML>", result);
        }

        [Test]
        public void GetQWCFile_SuccessRunEverySeconds()
        {
            // Arrange
            var services = new WebConnectorQwc();

            var model = new WebConnectorQwcModel
            {
                AppDescription = "Description",
                AppName = "Name",
                AppSupport = "Support",
                AppURL = "URL",
                FileID = Guid.Parse("56e8badb-7364-4e4a-ac08-28b988b3cd74"),
                OwnerID = Guid.Parse("0d400059-de9a-4c67-85f4-dd912ea23398"),
                UserName = "UserName",

                RunEvery = new TimeSpan(0, 0, 30)
            };

            // Act
            var result = services.GetQwcFile(model);

            // Assert
            Assert.AreEqual("<?xml version=\"1.0\"?><QBWCXML><AppDescription>Description</AppDescription><AppID></AppID><AppName>Name</AppName><AppSupport>Support</AppSupport><AppURL>URL</AppURL><FileID>{56e8badb-7364-4e4a-ac08-28b988b3cd74}</FileID><OwnerID>{0d400059-de9a-4c67-85f4-dd912ea23398}</OwnerID><QBType>QBFS</QBType><Scheduler><RunEveryNSeconds>30</RunEveryNSeconds></Scheduler><UserName>UserName</UserName></QBWCXML>", result);
        }

        [Test]
        public void GetQWCFile_SuccessRunEveryMinutes()
        {
            // Arrange
            var services = new WebConnectorQwc();

            var model = new WebConnectorQwcModel
            {
                AppDescription = "Description",
                AppName = "Name",
                AppSupport = "Support",
                AppURL = "URL",
                FileID = Guid.Parse("56e8badb-7364-4e4a-ac08-28b988b3cd74"),
                OwnerID = Guid.Parse("0d400059-de9a-4c67-85f4-dd912ea23398"),
                UserName = "UserName",

                RunEvery = new TimeSpan(0, 30, 0)
            };

            // Act
            var result = services.GetQwcFile(model);

            // Assert
            Assert.AreEqual("<?xml version=\"1.0\"?><QBWCXML><AppDescription>Description</AppDescription><AppID></AppID><AppName>Name</AppName><AppSupport>Support</AppSupport><AppURL>URL</AppURL><FileID>{56e8badb-7364-4e4a-ac08-28b988b3cd74}</FileID><OwnerID>{0d400059-de9a-4c67-85f4-dd912ea23398}</OwnerID><QBType>QBFS</QBType><Scheduler><RunEveryNMinutes>30</RunEveryNMinutes></Scheduler><UserName>UserName</UserName></QBWCXML>", result);
        }

        [Test]
        public void GetQWCFile_SuccessRunEveryRoundingMinutes()
        {
            // Arrange
            var services = new WebConnectorQwc();

            var model = new WebConnectorQwcModel
            {
                AppDescription = "Description",
                AppName = "Name",
                AppSupport = "Support",
                AppURL = "URL",
                FileID = Guid.Parse("56e8badb-7364-4e4a-ac08-28b988b3cd74"),
                OwnerID = Guid.Parse("0d400059-de9a-4c67-85f4-dd912ea23398"),
                UserName = "UserName",

                RunEvery = new TimeSpan(0, 30, 59)
            };

            // Act
            var result = services.GetQwcFile(model);

            // Assert
            Assert.AreEqual("<?xml version=\"1.0\"?><QBWCXML><AppDescription>Description</AppDescription><AppID></AppID><AppName>Name</AppName><AppSupport>Support</AppSupport><AppURL>URL</AppURL><FileID>{56e8badb-7364-4e4a-ac08-28b988b3cd74}</FileID><OwnerID>{0d400059-de9a-4c67-85f4-dd912ea23398}</OwnerID><QBType>QBFS</QBType><Scheduler><RunEveryNMinutes>31</RunEveryNMinutes></Scheduler><UserName>UserName</UserName></QBWCXML>", result);
        }
    }
}
