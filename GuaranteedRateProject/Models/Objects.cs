using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuaranteedRateProject.Models
{ 
    public class Person
    {
        public string Gender { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FavoriteColor { get; set; }
        public DateTime DateOfBirth { get; set; }
    }

    public class Result
    {
        public bool Successful { get; set; }

        private string _information;
        public string Information
        { 
            get { return _information ?? string.Empty; }
            set { _information = value; }
        }
    }

    public static class Model
    {
        public static List<Person> ListOfPersons = new List<Person>();
    }
     
}
