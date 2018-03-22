using GuaranteedRateProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using GuaranteedRateProject.Models;

namespace GuaranteedRateProject.Controllers
{ 
    public class RecordsController : ApiController
    {  
        //GET /records/birthdate - returns records sorted by birthdate
        [Route("api/records/birthdate")]
        [AcceptVerbs("GET")]
        public IHttpActionResult GetByBirthDate()
        {
            return Ok();
        }

        //GET /records/gender - returns records sorted by gender
        [Route("api/records/gender")]
        [AcceptVerbs("GET")]
        public IHttpActionResult GetByGender()
        {
            return Ok();
        }

        //GET /records/name - returns records sorted by name
        [Route("api/records/name")]
        [AcceptVerbs("GET")]
        public IHttpActionResult GetByName()
        {
            return Ok();
        }

        //POST /records - Post a single data line in any of the 3 formats supported by your existing code
        [Route("api/records")]
        [AcceptVerbs("POST")]
        public void Post([FromBody]string value)
        {
        } 
    }
}
