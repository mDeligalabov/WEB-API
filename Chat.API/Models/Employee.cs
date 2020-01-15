using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HBProducts.Chat.API.Models
{
    public class Employee : User
    {
        private List<int> ratings;
        public Employee(string name) : base(name)
        {
            
        }
    }
}