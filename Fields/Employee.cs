using System;
using System.Collections.Generic;
using System.Text;

namespace Fields
{
    internal class Employee
    {
        // Instance fields
        public string Name;
        public int Age;

        // Readonly instance field
        public readonly string EmployeeId;

        // Static field
        public static string CompanyName = "Tech Solutions Inc.";
        public static int TotalEmployees = 0;

        public Employee(string name, int age, string id)
        {
            Name = name;
            Age = age;
            EmployeeId = id;    // readonly set here
            TotalEmployees++;   // update static counter when a new employee is created
        }
    }
}
