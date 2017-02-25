using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace RentOfBikes.Models.ViewModels
{
    public class RentPointViewModel
    {
        public Employee Employee;

        public IEnumerable<Bike> Bikes;

        public Cashbox Cashbox;

        public Safe Safe;
    }
}
