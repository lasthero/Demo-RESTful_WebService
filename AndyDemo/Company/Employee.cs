using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Company
{
    public class Employee
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int EmpCode { get; set; }
        public string Designation { get; set; }

        public string GetEmployeeName()
        {
            string fullName = FirstName + " " + LastName;
            return fullName;
        }
    }
}
