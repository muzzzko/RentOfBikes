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
    public class RentPointsController : Controller
    {
        private readonly IQueryBuilder _queryBuilder;




        public RentPointsController(IQueryBuilder queryBuilder)
        {
            _queryBuilder = queryBuilder;
        }



        public IActionResult RentPoints()
        {
            IEnumerable<RentPoint> list = _queryBuilder
                                         .For<IEnumerable<RentPoint>>()
                                         .With(new GetAllEntitiesCriterion());


            Mapper.Initialize(cfg => cfg.CreateMap<RentPoint, RentPointViewModel>());
            var rentPoint = Mapper.Map<IEnumerable<RentPoint>, IEnumerable<RentPointViewModel>>(list);

            return View(rentPoint);
        }
    }
}
