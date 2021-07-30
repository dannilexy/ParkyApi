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
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class NationalParksController : ControllerBase
    {
        private INationalParkRepository _npRepo;
        private readonly IMapper _mapper;
        public NationalParksController(INationalParkRepository npRepo, IMapper mapper)
        {
            _npRepo = npRepo;
            _mapper = mapper;
        }


        //Getting All Parks
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<NationalParkDto>))]
        public IActionResult GetNationalParks()
        {
            var ObjList = _npRepo.GetNationaParks();
            var ObjDto = new List<NationalParkDto>();
            foreach (var Obj in ObjList)
            {
                ObjDto.Add(_mapper.Map<NationalParkDto>(Obj));
            }

            return Ok(ObjDto);
        }


        //Getting Individual Park
        [HttpGet("{nationalParkId:int}", Name = "GetNationalPark")]
        [ProducesResponseType(200, Type = typeof(NationalParkDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetNationalPark(int nationalParkId)
        {

         var obj =   _npRepo.GetNationalPark(nationalParkId);
            if (obj == null)
            {
                return NotFound();
            }
            else
            {
                var ObjDto = _mapper.Map<NationalParkDto>(obj);
                return Ok(ObjDto);
            }
        }


        //Creating a Park
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(NationalParkDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public IActionResult CreateNationalPark([FromBody] NationalParkDto nationalParkDto)
        {
            //Checking for empty entry
            if (nationalParkDto == null)
            {
                return BadRequest(ModelState);
            }

            //Checking if Entry Already Exists
            if (_npRepo.NationalParkExists(nationalParkDto.Name))
            {
                ModelState.AddModelError("", "National Park Exists!");
                return StatusCode(404, ModelState);
            }

            // Checking if model State is Not valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Mapping the National Park Dto to National Park Model
            var obj = _mapper.Map<NationalPark>(nationalParkDto);

            //Checking if record is not created successfully and returning corresponding response(Code)
            if (!_npRepo.CreateNationalPark(obj))
            {
                ModelState.AddModelError("", $"Something Went wrong while creating record {obj.Name}");
                return StatusCode(500, ModelState);
            }

            //Returns status Ok if everything works as expected
            return CreatedAtRoute("GetNationalPark", new {nationalParkId = obj.Id }, obj);
        }



        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        //Put/Edit/Patch a national Park
        [HttpPatch("{nationalParkId:int}", Name = "UpdateNaionalPark")]
        public IActionResult UpdateNaionalPark(int nationalParkId,[FromBody] NationalParkDto nationalParkDto)
        {
            //Checking for empty entry
            if (nationalParkDto == null || nationalParkId != nationalParkDto.Id)
            {
                return BadRequest(ModelState);
            }

            //Checking if Entry Already Exists
            if (_npRepo.NationalParkExists(nationalParkDto.Name))
            {
                ModelState.AddModelError("", "National Park Exists!");
                return StatusCode(404, ModelState);
            }

            // Checking if model State is Not valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Mapping the National Park Dto to National Park Model
            var obj = _mapper.Map<NationalPark>(nationalParkDto);

            //Checking if record is not created successfully and returning corresponding response(Code)
            if (!_npRepo.UpdateNationalPark(obj))
            {
                ModelState.AddModelError("", $"Something Went wrong while Updating record {obj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }


        //Delete a national Park
        [HttpDelete("{nationalParkId:int}", Name = "DeleteNaionalPark")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteNaionalPark(int nationalParkId)
        {
            //Checking if record Exists
            if (!_npRepo.NationalParkExists(nationalParkId))
            {
                return NotFound();
            }

            //Getting Record to be deleted fron Db
            var obj = _npRepo.GetNationalPark(nationalParkId);

            //Checking if record is not created successfully and returning corresponding response(Code)
            if (!_npRepo.DeleteNationalPark(obj))
            {
                ModelState.AddModelError("", $"Something Went wrong while Deleteing record {obj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }




    }
}
