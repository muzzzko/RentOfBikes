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
    public class RentsController : Controller
    {
        private readonly IQueryBuilder _queryBuilder;



        public RentsController(IQueryBuilder queryBuilder)
        {
            _queryBuilder = queryBuilder;
        }



        public IActionResult List()
        {
            IEnumerable<Rent> list = _queryBuilder.For<IEnumerable<Rent>>().With(new GetAllEntitiesCriterion());

            Mapper.Initialize(cfg => cfg.CreateMap<Rent, RentViewModel>());
            var rents = Mapper.Map<IEnumerable<Rent>, IEnumerable<RentViewModel>>(list);
            return View(rents);
        }
    }
}
