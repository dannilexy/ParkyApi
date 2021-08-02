using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyApi.Models;
using ParkyApi.Models.Dtos;
using ParkyApi.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyApi.Controllers
{
    //[Route("api/Trails")]
    [Route("api/v{version.apiVersion}/trails")]

    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class TrailsController : ControllerBase
    {
        private ITrailRepository _trRepo;
        private readonly IMapper _mapper;
        public TrailsController(ITrailRepository trRepo, IMapper mapper)
        {
            _trRepo = trRepo;
            _mapper = mapper;
        }


        //Getting All Parks
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<TrailDto>))]
        public IActionResult GetTrails()
        {
            var ObjList = _trRepo.GetTrails();
            var ObjDto = new List<TrailDto>();
            foreach (var Obj in ObjList)
            {
                ObjDto.Add(_mapper.Map<TrailDto>(Obj));
            }

            return Ok(ObjDto);
        }


        //Getting Individual Park
        [HttpGet("{trId:int}", Name = "GetTrail")]
        [ProducesResponseType(200, Type = typeof(TrailDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetTrail(int trId)
        {

         var obj =   _trRepo.GetTrail(trId);
            if (obj == null)
            {
                return NotFound();
            }
            else
            {
                var ObjDto = _mapper.Map<TrailDto>(obj);
                return Ok(ObjDto);
            }
        }


        //Creating a Park
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(TrailDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public IActionResult CreateTrail([FromBody] TrailCreateDto trailDto)
        {
            //Checking for empty entry
            if (trailDto == null)
            {
                return BadRequest(ModelState);
            }

            //Checking if Entry Already Exists
            if (_trRepo.TrailExists(trailDto.Name))
            {
                ModelState.AddModelError("", "Trail Exists!");
                return StatusCode(404, ModelState);
            }

            // Checking if model State is Not valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Mapping the National Park Dto to National Park Model
            var obj = _mapper.Map<Trail>(trailDto);

            //Checking if record is not created successfully and returning corresponding response(Code)
            if (!_trRepo.CreateTrail(obj))
            {
                ModelState.AddModelError("", $"Something Went wrong while creating record {obj.Name}");
                return StatusCode(500, ModelState);
            }

            //Returns status Ok if everything works as expected
            return CreatedAtRoute("GetTrail", new { trId = obj.Id }, obj);
        }
        


        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        //Put/Edit/Patch a Trai
        [HttpPatch("{TrailId:int}", Name = "UpdateTrail")]
        public IActionResult UpdateTrail(int TrailId,[FromBody] TrailUpdateDto TrailDto)
        {
            //Checking for empty entry
            if (TrailDto == null || TrailId != TrailDto.Id)
            {
                return BadRequest(ModelState);
            }

            //Checking if Entry Already Exists
            if (_trRepo.TrailExists(TrailDto.Name))
            {
                ModelState.AddModelError("", "Trail Exists!");
                return StatusCode(404, ModelState);
            }

            // Checking if model State is Not valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Mapping the National Park Dto to National Park Model
            var obj = _mapper.Map<Trail>(TrailDto);

            //Checking if record is not created successfully and returning corresponding response(Code)
            if (!_trRepo.UpdateTrail(obj))
            {
                ModelState.AddModelError("", $"Something Went wrong while Updating record {obj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }


        //Delete a national Park
        [HttpDelete("{TrailId:int}", Name = "DeleteTrail")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteTrail(int TrailId)
        {
            //Checking if record Exists
            if (!_trRepo.TrailExists(TrailId))
            {
                return NotFound();
            }

            //Getting Record to be deleted fron Db
            var obj = _trRepo.GetTrail(TrailId);

            //Checking if record is not created successfully and returning corresponding response(Code)
            if (!_trRepo.DeleteTrail(obj))
            {
                ModelState.AddModelError("", $"Something Went wrong while Deleteing record {obj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }




    }
}
