using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using GuaranteedRateProject.Models;
using OwinSelfhostSample;
using Microsoft.Owin.Hosting;
using System.Net.Http;
using GuaranteedRateProject.Models;
using m = GuaranteedRateProject.Models.Model;
using Newtonsoft.Json;

namespace GuaranteedRateProject
{
    class Program
    { 
        const string BASE_ADDRESS = "http://localhost:8080/";

        static void Main(string[] args)
        { 
            try
            {
                m.ListOfPersons = new List<Person>();
                ProcessFiles();
                DisplayData();

                m.ListOfPersons = new List<Person>();
                PerformWebAPIOperations();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            } 
        }

        static void PerformWebAPIOperations()
        { 
            // Start OWIN host 
            using (WebApp.Start<Startup>(url: BASE_ADDRESS))
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new System.Uri(BASE_ADDRESS);
                 
                Console.WriteLine("POST / records - Click any key to post a set of single data lines via WebAPI.");
                Console.ReadKey();
                Console.WriteLine("------------------------------------------------------------------------------");

                //Loop through lines found in one of test files (containing all kinds of delimiters)
                //And performs a POST request
                string folderPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"TestData");
                string filePath = Path.Combine(folderPath, "Mixed Data.nfo");

                foreach (string line in File.ReadLines(filePath))
                {
                    var content = new StringContent("=" + line, Encoding.UTF8, "application/x-www-form-urlencoded");
                    HttpResponseMessage httpResponse = client.PostAsync("/api/records", content).Result;
                    Console.WriteLine("POSTED: " + line);
                }

                Console.WriteLine();
                Console.WriteLine("GET / records / birthdate -  Click any key to return records sorted by birthdate.");
                Console.ReadKey();
                Console.WriteLine("------------------------------------------------------------------------------");

                var response = client.GetAsync(BASE_ADDRESS + "api/records/birthdate").Result; 
                string jsonResult = response.Content.ReadAsStringAsync().Result;

                var DataReceived = JsonConvert.DeserializeObject<List<Person>>(jsonResult);
                DataReceived.ForEach(x => PrintDataLine(x));

                Console.WriteLine();
                Console.WriteLine("GET / records / gender - Click any key to return records sorted by gender.");
                Console.ReadKey();
                Console.WriteLine("------------------------------------------------------------------------------");

                response = client.GetAsync(BASE_ADDRESS + "api/records/gender").Result; 
                jsonResult = response.Content.ReadAsStringAsync().Result;

                DataReceived = JsonConvert.DeserializeObject<List<Person>>(jsonResult);
                DataReceived.ForEach(x => PrintDataLine(x));

                Console.WriteLine();
                Console.WriteLine("GET / records / name - Click any key to return records sorted by name.");
                Console.ReadKey();
                Console.WriteLine("------------------------------------------------------------------------------");

                response = client.GetAsync(BASE_ADDRESS + "api/records/name").Result; 

                jsonResult = response.Content.ReadAsStringAsync().Result;

                DataReceived = JsonConvert.DeserializeObject<List<Person>>(jsonResult);
                DataReceived.ForEach(x => PrintDataLine(x)); 
            }

            Console.WriteLine();
            Console.WriteLine("Click any key to exit an application.");
            Console.ReadKey();

        }

        static void ProcessFiles()
        {
            //In the real life scenario, I would either prompt user to select a folder with file, 
            //or have a preconfigured filepath in App.config / Web.config
            //Assuming txt format only.

            Methods m = new Methods();

            string folderPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"TestData");

            Console.WriteLine("Click any key to read sample data from files");
            Console.ReadKey();

            Console.WriteLine("Reading files at: " + folderPath);

            string[] files = Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories);

            foreach (string filePath in files)
            {
                Result fileCheck = m.IsValidFile(filePath, "txt");

                if (fileCheck.Successful)
                {
                    Result fileLinesCheck = m.FileDataPopulated(filePath);

                    if (fileLinesCheck.Successful)
                    {
                        Console.WriteLine("ENTIRE FILE SUCCESSFULLY READ: " + Path.GetFileName(filePath));
                    }
                    else
                    {
                        Console.WriteLine("SOME LINES IN FILE COULD NOT BE READ: {0}.{1}Issues: {2}", 
                            Path.GetFileName(filePath), "\n", fileLinesCheck.Information);
                    }

                }
                else
                {
                    Console.WriteLine(string.Format("COULD NOT READ: {0}. Error: {1}", Path.GetFileName(filePath), fileCheck.Information));
                }
            }

            Console.WriteLine();
        }

        static void DisplayData()
        {
            Console.WriteLine("Output 1 – Click any key to see data sorted by gender(females before males) then by last name ascending.");
            Console.ReadKey();
            Console.WriteLine("------------------------------------------------------------------------------");
            m.ListOfPersons = m.ListOfPersons.OrderBy(c => c.Gender).ThenBy(n => n.LastName).ToList();
            m.ListOfPersons.ForEach(x => PrintDataLine(x));
            Console.WriteLine();

            Console.WriteLine("Output 2 – Click any key to see data sorted by birth date, ascending.");
            Console.ReadKey();
            Console.WriteLine("------------------------------------------------------------------------------");
            m.ListOfPersons = m.ListOfPersons.OrderBy(c => c.DateOfBirth).ToList(); 
            m.ListOfPersons.ForEach(x => PrintDataLine(x));
            Console.WriteLine();

            Console.WriteLine("Output 3 – Click any key to see data sorted by last name, descending.");
            Console.ReadKey();
            Console.WriteLine("------------------------------------------------------------------------------");
            m.ListOfPersons = m.ListOfPersons.OrderByDescending(c => c.LastName).ToList();
            m.ListOfPersons.ForEach(x => PrintDataLine(x));
            Console.WriteLine();
        }

        static void PrintDataLine(Person p)
        {
            Console.WriteLine("{0,-15} {1,-15} {2,-5} {3,-20} {4,-5}", p.LastName, p.FirstName, p.Gender, p.FavoriteColor, p.DateOfBirth.ToShortDateString());
        }
    } 
}
