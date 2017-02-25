using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Entities.Deposits;

namespace RentOfBikes.Models.ViewModels
{
    public class RentViewModel
    {
        public DateTime StartedAt;

        public Client Client;

        public Bike Bike;

        public Deposit Deposit;
    }
}
