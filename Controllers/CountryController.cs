using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Dto;
using TodoApi.Interfaces;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CountryController : ControllerBase
    {
        private readonly ICountryInterface _countryInterface;
        private readonly IMapper _mapper;
        public CountryController(ICountryInterface countryInterface, IMapper mapper)
        {
            _countryInterface = countryInterface;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type=typeof(ICollection<Country>))]
        public IActionResult Getcountries(){
            var countries = _mapper.Map<List<CountryDto>>(_countryInterface.GetCountries());
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            return Ok(countries);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type=typeof(Country))]
        public IActionResult Getcountry(int id){
            Console.WriteLine(id);
            if(!_countryInterface.CountryExists(id)){
                return NotFound();
            }
            var country = _mapper.Map<CountryDto>(_countryInterface.GetCountry(id));
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            return Ok(country);
        }

        [HttpGet("owner/{id}")]
        [ProducesResponseType(200, Type=typeof(Country))]
        public IActionResult GetCountryByOwner(int id){
            var country = _mapper.Map<CountryDto>(_countryInterface.GetCountryByOwner(id));
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            return Ok(country);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        public IActionResult CreateCountry([FromBody] CountryDto country){
            if(country==null){
                return BadRequest(ModelState);
            }
            var cat = _countryInterface.GetCountries().Where(c=>c.Name.Trim().ToUpper() == country.Name.Trim().ToUpper()).FirstOrDefault();
            if(cat!=null){
                 ModelState.AddModelError("", "Country Already Exists");
                 return StatusCode(422, ModelState);
            }
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            var cont = _mapper.Map<Country>(country);
            if(!_countryInterface.CreateCountry(cont)){
                ModelState.AddModelError("", "Soemthing went wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully Created");

        }

        [HttpPut("{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCategory(int id , [FromBody] CountryDto updateCategory){
            if(updateCategory == null){
                return BadRequest(ModelState);
            }
            if(id != updateCategory.Id){
                return BadRequest(ModelState);
            }
            if(!_countryInterface.CountryExists(id)){
                return BadRequest(ModelState);
            }

            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            var categoryMap = _mapper.Map<Country>(updateCategory);
            if(!_countryInterface.UpdateCountry(categoryMap)){
                ModelState.AddModelError("","something went wrong updateung category");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }




    }
}