using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TodoApi.Dto;
using TodoApi.Interfaces;
using TodoApi.Models;
using TodoApi.Repository;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryInterface _categoryRepository;
        private readonly IMapper _mapper;
        public CategoryController(ICategoryInterface categoryRepository, IMapper mapper)
        {
            _categoryRepository  = categoryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type=typeof(ICollection<Category>))]
        public IActionResult GetCategories (){
            var res = _mapper.Map<List<CategoryDto>>(_categoryRepository.GetCategories());
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            return Ok(res);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type=typeof(Category))]

        public IActionResult GetCategory(int id){
            if(!_categoryRepository.IsCategory(id)){
                return NotFound();
            }

            var category = _mapper.Map<CategoryDto>( _categoryRepository.Getcategory(id));

            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            return Ok(category);
        }

        [HttpGet("catgory/pokemon/all/{id}")]
        [ProducesResponseType(200, Type=typeof(ICollection<Category>))]

        public IActionResult GetPokemonByCategory(int id){
            if(!_categoryRepository.IsCategory(id)){
                return NotFound();
            }

            var pokemons = _mapper.Map<List<PokemonDto>>(_categoryRepository.GetPokemonByCategory(id));

            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            return Ok(pokemons);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        public IActionResult CreateCategory([FromBody] CategoryDto categoryCreate){
            if(categoryCreate==null){
                return BadRequest(ModelState);
            }
            var cat = _categoryRepository.GetCategories().Where(c=>c.Name.Trim().ToUpper() == categoryCreate.Name.Trim().ToUpper()).FirstOrDefault();
            
            if(cat!=null){
                 ModelState.AddModelError("", "Category Already Exists");
                 return StatusCode(422, ModelState);
            }
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            var category = _mapper.Map<Category>(categoryCreate);
            if(!_categoryRepository.CreateCategory(category)){
                ModelState.AddModelError("", "Soemthing went wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully Created");

        }

        [HttpPut("{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCategory(int id , [FromBody] CategoryDto updateCategory){
            if(updateCategory == null){
                return BadRequest(ModelState);
            }
            if(id != updateCategory.Id){
                return BadRequest(ModelState);
            }
            if(!_categoryRepository.IsCategory(id)){
                return BadRequest(ModelState);
            }

            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            var categoryMap = _mapper.Map<Category>(updateCategory);
            if(!_categoryRepository.UpdateCategory(categoryMap)){
                ModelState.AddModelError("","something went wrong updateung category");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}