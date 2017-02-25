using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Domain.Entities.CQRS;
using Infrastructure.CommandContext;
using RentOfBikes.Models.ViewModels;
using Domain.Entities.Deposits;
using Domain.Entities;

namespace RentOfBikes.Controllers
{
    public class RentServiceController : Controller
    {
        private readonly ICommandBuilder _commandBuilder;



        public RentServiceController(ICommandBuilder commandBuilder)
        {
            _commandBuilder = commandBuilder;
        }


        [HttpGet]
        public IActionResult TakeBike()
        {
            return View();
        }

        [HttpPost]
        public IActionResult TakeBike(string Surname, string firstName,
            string patronymic, string Name, string deposit)
        {
            Deposit deposiT;
            if (deposit.All(x => x != ' '))
            {
                deposiT = new MoneyDeposit(Int32.Parse(deposit));
            }
            else
            {
                deposiT = new PassportDeposit(deposit.Substring(0, 4), deposit.Substring(5));
            }
            _commandBuilder.Execute<TakeBikeCommandContext>(
                new TakeBikeCommandContext()
                {
                    Client = new Client(Surname, firstName, patronymic),
                    BikeName = Name,
                    Deposit = deposiT
                }
                );
            return RedirectPermanent("~/Rents/List");
        }

        [HttpGet]
        public IActionResult ReturnBike()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ReturnBike(string Surname, string firstName,
            string patronymic, string Name)
        {
            _commandBuilder.Execute<ReturnBikeCommandContext>(
                new ReturnBikeCommandContext()
                {
                    Employee = new Employee(Surname, firstName, patronymic),
                    BikeName = Name,
                }
                );
            return RedirectPermanent("~/Rents/List");
        }
    }
}
