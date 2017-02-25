using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Domain.Entities.CQRS;
using Domain.Entities;
using Infrastructure.Criterion;
using AutoMapper;
using RentOfBikes.Models.ViewModels;

namespace RentOfBikes.Controllers
{
    public class BikesController : Controller
    {
        private readonly IQueryBuilder _queryBuilder;



        public BikesController(IQueryBuilder queryBuilder)
        {
            _queryBuilder = queryBuilder;
        }



        public IActionResult List()
        {
            IEnumerable<Bike> list = _queryBuilder.For<IEnumerable<Bike>>().With(new GetAllEntitiesCriterion());

            Mapper.Initialize(cfg => cfg.CreateMap<Bike, BikeViewModel>());
            var bikes = Mapper.Map<IEnumerable<Bike>, IEnumerable<BikeViewModel>>(list);
            return View(bikes);
        }
    }
}
