using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Domain.Entities.CQRS;
using RentOfBikes.Models.ViewModels;
using Infrastructure.CommandContext;
using Domain.Entities;
using Infrastructure.Criterion;

namespace RentOfBikes.Controllers
{
    public class AddBikeController : Controller
    {
        private readonly ICommandBuilder _commandBuilder;
        private readonly IQueryBuilder _queryBuilder;


        public AddBikeController(ICommandBuilder commandBuilder,
            IQueryBuilder queryBuilder)
        {
            _commandBuilder = commandBuilder;
            _queryBuilder = queryBuilder;
        }


        [HttpGet]
        public IActionResult AddBike()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddBike(BikeViewModel bike)
        {
            _commandBuilder.Execute<AddNewBikeCommandContext>(
                new AddNewBikeCommandContext()
                {
                    Name = bike.Name,
                    HourCost = bike.HourCost
                });
            return RedirectPermanent("~/Bikes/List");
        }

        [HttpGet]
        public IActionResult AddBikeToRentPoint()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddBikeToRentPoint(string name, string Surname,string FirstName, string Patronymic)
        {

            _commandBuilder.Execute<AddBikeToRentPointCommandContext>(
                new AddBikeToRentPointCommandContext
                {
                    BikeName = name,
                    Employee = new Employee(Surname,FirstName,Patronymic)
                });
            return RedirectPermanent("~/RentPoints/RentPoints");
        }
    }
}
