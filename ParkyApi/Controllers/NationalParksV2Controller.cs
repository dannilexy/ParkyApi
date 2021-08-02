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
    //[Route("api/[controller]")]
    [Route("api/v{version.apiVersion}/NationalParks")]
    [ApiVersion("2.0")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class NationalParksV2Controller : ControllerBase
    {
        private INationalParkRepository _npRepo;
        private readonly IMapper _mapper;
        public NationalParksV2Controller(INationalParkRepository npRepo, IMapper mapper)
        {
            _npRepo = npRepo;
            _mapper = mapper;
        }


        //Getting All Parks
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(NationalParkDto))]
        public IActionResult GetNationalParks()
        {
            var Obj = _npRepo.GetNationaParks().FirstOrDefault();
            return Ok(Obj);
        }


     


    }
}
