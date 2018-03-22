using GuaranteedRateProject.Helpers;
using GuaranteedRateProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using m = GuaranteedRateProject.Models.Model;

namespace GuaranteedRateProject.Controllers
{ 
    public class RecordsController : ApiController
    {  
        //GET /records/birthdate - returns records sorted by birthdate
        [Route("api/records/birthdate")]
        [AcceptVerbs("GET")]
        public IHttpActionResult GetByBirthDate()
        { 
            m.ListOfPersons = m.ListOfPersons.OrderBy(c => c.DateOfBirth).ToList();

            return Json(m.ListOfPersons);
        }

        //GET /records/gender - returns records sorted by gender
        [Route("api/records/gender")]
        [AcceptVerbs("GET")]
        public IHttpActionResult GetByGender()
        { 
            m.ListOfPersons = m.ListOfPersons.OrderBy(c => c.Gender).ToList();

            return Json(m.ListOfPersons);
        }

        //GET /records/name - returns records sorted by name
        [Route("api/records/name")]
        [AcceptVerbs("GET")]
        public IHttpActionResult GetByName()
        { 
            m.ListOfPersons = m.ListOfPersons.OrderBy(c => c.LastName).ToList();

            return Json(m.ListOfPersons);
        }

        //POST /records - Post a single data line in any of the 3 formats supported by your existing code
        [Route("api/records")]
        [AcceptVerbs("POST")]
        public IHttpActionResult Post([FromBody]string value)
        {
            Methods methods = new Methods(); 
            bool LinePopulated = methods.LinePopulated(value);

            return Ok(value);
        } 
    }
}
