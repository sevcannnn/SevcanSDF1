using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevcanSDF1
{
    public class Employee
    {
        public string Code { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }

        
        public BindingList<Payment> Payments { get; set; } = new BindingList<Payment>();
    }
}
