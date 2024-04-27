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

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IReviewInterface _reviewInterface;
        public ReviewController(IMapper mapper,IReviewInterface reviewInterface)
        {
            _mapper = mapper;
            _reviewInterface = reviewInterface;
        }
        [HttpGet]
        [ProducesResponseType(200, Type=typeof(ICollection<Review>))]
        public IActionResult GetReview(){
            var rev = _mapper.Map<List<ReviewDto>>(_reviewInterface.GetReviews());
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            return Ok(rev);

        }
        
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type=typeof(Review))]

        public IActionResult GetReview(int id){
            if(!_reviewInterface.ReviewExists(id)){
                return NotFound();
            }
            var review = _mapper.Map<ReviewDto>(_reviewInterface.GetReview(id));
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            return Ok(review);
        }

        [HttpGet("pokemon/{id}")]
        [ProducesResponseType(200, Type=typeof(ICollection<Review>))]
        public IActionResult GetReviewsOfPokemon(int id){
            var revs = _mapper.Map<List<ReviewDto>>(_reviewInterface.GetReviewsOfPokemon(id));
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            return Ok(revs);
        }
    }
}