using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using GuaranteedRateProject.Helpers;
using OwinSelfhostSample;
using Microsoft.Owin.Hosting;
using System.Net.Http;
using GuaranteedRateProject.Models;
using m = GuaranteedRateProject.Models.Model;

namespace GuaranteedRateProject
{
    class Program
    { 
        static void Main(string[] args)
        {


            string baseAddress = "http://localhost:9000/";

            // Start OWIN host 
            using (WebApp.Start<Startup>(url: baseAddress))
            {
                // Create HttpCient and make a request to api/values 
                HttpClient client = new HttpClient();

                var response = client.GetAsync(baseAddress + "api/records/birthdate").Result;

                Console.WriteLine(response);
                Console.WriteLine(response.Content.ReadAsStringAsync().Result);
                Console.ReadLine();
            }

            ProcessFiles();
            DisplayData();

            Console.ReadKey(); 
        }
        
        static void ProcessFiles()
        {
            //In the real life scenario, I would either prompt user to select a folder with file, 
            //or have a preconfigured filepath in App.config / Web.config
            //Assuming txt format only.

            Methods m = new Methods();

            string folderPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"TestData");

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
            Console.WriteLine("Output 1 – sorted by gender(females before males) then by last name ascending.");
            Console.WriteLine("------------------------------------------------------------------------------");
            m.ListOfPersons = m.ListOfPersons.OrderBy(c => c.Gender).ThenBy(n => n.LastName).ToList();
            m.ListOfPersons.ForEach(x => PrintDataLine(x));
            Console.WriteLine();

            Console.WriteLine("Output 2 – sorted by birth date, ascending.");
            Console.WriteLine("------------------------------------------------------------------------------");
            m.ListOfPersons = m.ListOfPersons.OrderBy(c => c.DateOfBirth).ToList(); 
            m.ListOfPersons.ForEach(x => PrintDataLine(x));
            Console.WriteLine();

            Console.WriteLine("Output 3 – sorted by last name, descending.");
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
