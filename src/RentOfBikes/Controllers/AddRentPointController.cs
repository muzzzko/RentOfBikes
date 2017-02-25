using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Domain.Entities.CQRS;
using RentOfBikes.Models.ViewModels;
using Infrastructure.CommandContext;
using Domain.Entities;

namespace RentOfBikes.Controllers
{
    public class AddRentPointController : Controller
    {
        private readonly ICommandBuilder _commandBuilder;



        public AddRentPointController(ICommandBuilder commandBuilder)
        {
            _commandBuilder = commandBuilder;
        }



        [HttpGet]
        public IActionResult AddRentPoint()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddRentPoint(string  secodName,string firstName, string patronymic, decimal money)
        {
            _commandBuilder.Execute<AddNewRentPointCommandContext>(
                new AddNewRentPointCommandContext()
                    {
                        Employee = new Employee(secodName, firstName, patronymic),
                        Money = money
                }
                );
            return RedirectPermanent("~/RentPoints/RentPoints");
        }
    }
}
