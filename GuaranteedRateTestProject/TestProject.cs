using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GuaranteedRateProject.Models;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using OwinSelfhostSample;
using Microsoft.Owin.Hosting;
using Newtonsoft.Json;

namespace GuaranteedRateTestProject
{
    [TestClass]
    public class TestProject
    {
        Methods m;
        HttpClient client;
        string sampleFilePath;
        const string BASE_ADDRESS = "http://localhost:8080/";
        private IDisposable _webApiConnection;

        [TestInitialize]
        public void Setup()
        {
            m = new Methods();
            client = new HttpClient();

            Model.ListOfPersons = new List<Person>();

            string folderPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"TestData");
            sampleFilePath = Path.Combine(folderPath, "Mixed Data.nfo");

            _webApiConnection = WebApp.Start<Startup>(url: BASE_ADDRESS);
        }

        private void PopulateSampleData()
        {
            Model.ListOfPersons = new List<Person>();

            HttpClient client = new HttpClient();
            client.BaseAddress = new System.Uri(BASE_ADDRESS);

            string folderPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"TestData");
            string filePath = Path.Combine(folderPath, "Mixed Data.nfo");

            foreach (string line in File.ReadLines(filePath))
            {
                var content = new StringContent("=" + line, Encoding.UTF8, "application/x-www-form-urlencoded");
                HttpResponseMessage httpResponse = client.PostAsync("/api/records", content).Result;
            }
        }

        [TestMethod]
        public void TestExtensionCleanup()
        {
            string ext = m.CleanExtension(".PHP");
             
            Assert.AreEqual(ext, "php");
        }

        [TestMethod]
        public void TestDataPopulationFromFile()
        {
            Result result = m.FileDataPopulated(sampleFilePath);
             
            Assert.AreEqual(Model.ListOfPersons.Count > 0, true);
        }

        [TestMethod]
        public void TestLinePopulated()
        {
            bool shouldFail = m.LinePopulated("Callaro, Brandon M SaddleBrown 11/30/1970");

            Assert.AreEqual(shouldFail, false);
        }

        [TestMethod]
        public void TestIfReadable()
        { 
            bool isReadable = m.IsReadable(sampleFilePath);

            Assert.AreEqual(isReadable, true);
        }

        [TestMethod]
        public void Test_POST_Records_API()
        {
            PopulateSampleData();
            Assert.AreEqual(Model.ListOfPersons.Count > 0, true); 
        }

        [TestMethod]
        public void TEST_GET_Records_API_ByBirthday()
        {
            PopulateSampleData();

            var response = client.GetAsync(BASE_ADDRESS + "api/records/birthdate").Result;
            string jsonResult = response.Content.ReadAsStringAsync().Result;

            var DataReceived = JsonConvert.DeserializeObject<List<Person>>(jsonResult);

            if (DataReceived[0].DateOfBirth >= DataReceived[DataReceived.Count - 1].DateOfBirth)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void TEST_GET_Records_API_ByGender()
        {
            PopulateSampleData();

            var response = client.GetAsync(BASE_ADDRESS + "api/records/gender").Result;
            string jsonResult = response.Content.ReadAsStringAsync().Result;

            var DataReceived = JsonConvert.DeserializeObject<List<Person>>(jsonResult);

            if (!DataReceived[0].Gender.Equals("F"))
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void TEST_GET_Records_API_ByName()
        {
            PopulateSampleData();

            var response = client.GetAsync(BASE_ADDRESS + "api/records/name").Result;
            string jsonResult = response.Content.ReadAsStringAsync().Result;

            var DataReceived = JsonConvert.DeserializeObject<List<Person>>(jsonResult);

            if (!DataReceived[DataReceived.Count - 1].LastName.Equals("Vanheusen"))
            {
                Assert.Fail();
            }
        }

        [TestCleanup]
        public void ShutDown()
        {
            m = null;
            _webApiConnection.Dispose();
        }
    }
} 