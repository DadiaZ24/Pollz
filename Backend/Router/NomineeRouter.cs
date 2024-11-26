using Microsoft.AspNetCore.Mvc;
using Oscars.Backend.Dtos;
using Oscars.Backend.Service;

namespace Oscars.Backend.Router
{
    [ApiController]
    [Route("api/[controller]")]
    public class NomineeRouter : ControllerBase
    {
        private readonly NomineeService _nomineeService;
        public NomineeRouter(NomineeService nomineeService)
        {
            _nomineeService = nomineeService;
        }

        //GET ALL NOMINEE
        [HttpGet]
        public ActionResult GetNominees()
        {
            var nominee = _nomineeService.GetNominees();
            if (nominee == null)
            {
                return NotFound("No nominees found");
            }
            return Ok(nominee);
        }

        //GET A NOMINEE
        [HttpGet("{id}")]
        public ActionResult GetNominee(int id)
        {
            var nominee = _nomineeService.GetNomineeById(id);
            if (nominee == null)
            {
                return NotFound("Nominee not found");
            }
            return Ok(nominee);
        }

        //CREATE A NOMINEE
        [HttpPost]
        public ActionResult CreateNominee([FromBody] NomineeDto nomineeDtoo)
        {
            _nomineeService.CreateNominee(nomineeDtoo);
            return CreatedAtAction(nameof(GetNominee), new { id = nomineeDtoo.Id }, nomineeDtoo);
        }

        //UPDATE A NOMINEE
        [HttpPut("{id}")]
        public ActionResult UpdateNominee(int id, [FromBody] NomineeDto nomineeDtoo)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid nominee id");
            }
            _nomineeService.UpdateNominee(nomineeDtoo);
            return NoContent();
        }

        //DELETE A NOMINEE
        [HttpDelete("{id}")]
        public ActionResult DeleteNominee(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid nominee id");
            }
            _nomineeService.DeleteNominee(id);
            return NoContent();
        }

    }
}