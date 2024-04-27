using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

// using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Dto;
using TodoApi.Interfaces;
using TodoApi.Models;
using TodoApi.Repository;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class PokemonController : Controller
    {
        private readonly IPokemonInterface _pokemonRepository;
        private readonly IMapper _mapper;
        private readonly IOwnerInterface _ownerInterface;
        public IReviewInterface _reviewInterface;
        public PokemonController(IPokemonInterface pokemonRepository, IMapper mapper, IOwnerInterface ownerInterface, IReviewInterface reviewInterface)
        {
            _reviewInterface = reviewInterface;
            _ownerInterface = ownerInterface;
            _pokemonRepository = pokemonRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type=typeof(IEnumerable<Pokemon>))]

        public IActionResult GetPokemons(){
            var pokemons = _mapper.Map<List<PokemonDto>>( _pokemonRepository.GetPokemons());

            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            return Ok(pokemons);
        }
        [HttpGet("{pokeId}")]
        [ProducesResponseType(200, Type=typeof(Pokemon))]
        public IActionResult GetPokemon(int pokeId){

            if(!_pokemonRepository.PokemonExist(pokeId)){
                return NotFound();
            }
            var pk = _mapper.Map<PokemonDto>(_pokemonRepository.GetPokemon(pokeId));
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            return Ok(pk);
        }

        [HttpGet("rating/{pokeId}")]
        [ProducesResponseType(200, Type=typeof(decimal))]
        public IActionResult GetRating(int pokeId){ //IActionresult is used to implement polymorphism.
            if(!_pokemonRepository.PokemonExist(pokeId)){
                return NotFound();
            }
            var r = _mapper.Map<decimal>(_pokemonRepository.GetPokemonRating(pokeId));
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            return Ok(r);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        public IActionResult CreatePokemon([FromQuery] int ownerId, [FromQuery] int categoryId, [FromBody]PokemonDto createPokemon){
            if(createPokemon == null){
                return BadRequest(ModelState);
            }
            var pokemons = _pokemonRepository.GetPokemons().Where(c=>c.Name.Trim().ToUpper() == createPokemon.Name.Trim().ToUpper()).FirstOrDefault();
            if(pokemons!=null){
                ModelState.AddModelError("", "Owner already exists");
                return StatusCode(422, ModelState);
            }
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            var poke = _mapper.Map<Pokemon>(createPokemon);



            if(!_pokemonRepository.CreatePokemon(ownerId, categoryId, poke)){
                ModelState.AddModelError("", "Soemthing went wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully Created");

        }

        [HttpPut("{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdatePokemon(int id ,[FromQuery] int ownerId, [FromQuery] int categoryId, [FromBody] PokemonDto updateCategory){
            if(updateCategory == null){
                return BadRequest(ModelState);
            }
            if(id != updateCategory.Id){
                return BadRequest(ModelState);
            }
            if(!_pokemonRepository.PokemonExist(id)){
                return BadRequest(ModelState);
            }

            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            var categoryMap = _mapper.Map<Pokemon>(updateCategory);
            if(!_pokemonRepository.UpdatePokemon(ownerId, categoryId,categoryMap)){
                ModelState.AddModelError("","something went wrong updateung category");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }


        

    }
}