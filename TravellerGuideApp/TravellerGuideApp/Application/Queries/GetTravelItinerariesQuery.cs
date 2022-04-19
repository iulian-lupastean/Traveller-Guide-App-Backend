﻿using MediatR;
using TravelerGuideApp.Domain.Entities;

namespace TravelerGuideApp.Application.Queries
{
    public class GetTravelItinerariesQuery : IRequest<IEnumerable<TravelItinerary>>
    {
        public int userId { get; set; }
    }
}
