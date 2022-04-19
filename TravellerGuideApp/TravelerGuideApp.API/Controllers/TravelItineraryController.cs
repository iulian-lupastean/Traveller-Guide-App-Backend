﻿using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelerGuideApp.API.DTOs;
using TravelerGuideApp.Application.Commands;
using TravelerGuideApp.Application.Queries;

namespace TravelerGuideApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TravelItineraryController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public TravelItineraryController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTravelItinerary([FromBody] TravelItineraryPutPostDto travelItinerary)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var created = await _mediator.Send(_mapper.Map<CreateTravelItineraryCommand>(travelItinerary));
            var mappedResult = _mapper.Map<TravelItineraryGetDto>(created);
            return CreatedAtAction(nameof(GetById), new { travelItineraryId = mappedResult.TravelId }, mappedResult);
        }

        [HttpGet]
        [Route("user/{userId}")]
        public async Task<IActionResult> GetAllForUser(int userId)
        {
            var result = await _mediator.Send(new GetTravelItinerariesQuery { userId = userId });
            if (result == null)
                return NotFound();
            var mappedResult = _mapper.Map<List<TravelItineraryGetDto>>(result);
            return Ok(mappedResult);
        }

        [HttpGet]
        [Route("{travelItineraryId}")]
        public async Task<IActionResult> GetById(int travelItineraryId)
        {
            var result = await _mediator.Send(new GetTravelItineraryByIdQuery { Id = travelItineraryId });
            if (result == null)
                return NotFound();
            var mappedResult = _mapper.Map<TravelItineraryGetDto>(result);
            return Ok(mappedResult);
        }

        [HttpPut]
        [Route("{travelItineraryId}")]
        public async Task<IActionResult> UpdateTravelItinerary(int travelItineraryId,
            [FromBody] TravelItineraryPutPostDto updatedTravelItinerary)
        {
            var command = new UpdateTravelItineraryCommand
            {
                Id = travelItineraryId,
                Name = updatedTravelItinerary.Name,
                Status = updatedTravelItinerary.Status,
                TravelDate = updatedTravelItinerary.TravelDate,

            };
            var result = await _mediator.Send(command);
            if (result == null)
                return NotFound();
            return NoContent();
        }

        [HttpDelete]
        [Route("{travelItineraryId}")]
        public async Task<IActionResult> DeleteTravelItinerary(int travelItineraryId)
        {
            var result = await _mediator.Send(new DeleteTravelItineraryCommand { Id = travelItineraryId });
            if (result == null)
                return NotFound();
            return NoContent();
        }
    }

}
