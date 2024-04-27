using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using TodoApi.Dto;
using TodoApi.Interfaces;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OwnerController : Controller
    {
        public readonly IOwnerInterface _ownerInterface;
        public readonly ICountryInterface _countryInterface;
        public IMapper _mapper;
        public OwnerController(IOwnerInterface ownerInterface,IMapper mapper, ICountryInterface countryInterface)
        {
            _mapper = mapper;
            _ownerInterface = ownerInterface;
            _countryInterface = countryInterface;
        }

        [HttpGet]
        [ProducesResponseType(200, Type=typeof(ICollection<Owner>))]

        public IActionResult GetAllOwners(){
            var res = _mapper.Map<List<OwnerDto>>(_ownerInterface.GetOwners());
            if(!ModelState.IsValid){
                return BadRequest();
            }
            return Ok(res);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type=typeof(Owner))]
        public IActionResult GetOwner(int id){
            if(!_ownerInterface.OwnerExists(id)){
                return NotFound();
            }
            var owner = _mapper.Map<OwnerDto>(_ownerInterface.GetOwner(id));
             if(!ModelState.IsValid){
                return BadRequest();
            }
            return Ok(owner);
        }

        [HttpGet("pokemon/owner/{id}")]
        [ProducesResponseType(200, Type=typeof(ICollection<Owner>))]

        public IActionResult GetOwnerOfPokemon(int id){
           
            var owners = _mapper.Map<List<OwnerDto>>(_ownerInterface.GetOwnerOfPokemon(id));

            if(!ModelState.IsValid){
                return BadRequest();
            }
            return Ok(owners);

        }

        [HttpGet("owner/pokemons/{id}")]
        [ProducesResponseType(200, Type=typeof(ICollection<Pokemon>))]

        public IActionResult GetPokemonByOwner(int id){
           
            var pokemons = _mapper.Map<List<PokemonDto>>(_ownerInterface.GetPokemonByOwner(id));

            if(!ModelState.IsValid){
                return BadRequest();
            }
            return Ok(pokemons);

        }

        [HttpPost]
        [ProducesResponseType(204)]
        public IActionResult CreateOwner([FromQuery] int countryId, [FromBody]OwnerDto createOwner){
            if(createOwner == null){
                return BadRequest(ModelState);
            }
            var cat = _ownerInterface.GetOwners().Where(c=>c.FirstName.Trim().ToUpper() == createOwner.FirstName.Trim().ToUpper()).FirstOrDefault();
            if(cat!=null){
                ModelState.AddModelError("", "Owner already exists");
                return StatusCode(422, ModelState);
            }
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            var cont = _mapper.Map<Owner>(createOwner);
            cont.Country = _countryInterface.GetCountry(countryId);
            if(!_ownerInterface.CreateOwner(cont)){
                ModelState.AddModelError("", "Soemthing went wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully Created");

        }

    }
}