using System;
using System.Collections.Generic;
using System.Text;

// Mini-Project: Methods and Parameters

namespace MethodsParams
{
    internal class Person
    {
        public string Name { get; }
        public int Age { get; }

        public Person(string name, int age)
        {
            Name = name;
            Age = age;
        }

        // Method with multiple return styles
        public void GetDetails(out string fullInfo, out bool isAdult)
        {
            fullInfo = $"{Name} is {Age} year old.";
            isAdult = Age >= 18;
        }

        // Optional + named friendly method
        public void CelebrateBirthday(int extraGifts = 0, string message = "Happy Birthday!")
        {
            
            Console.WriteLine($"{message} {Name}! Now {Age} year old. Extra gifts: {extraGifts}");
        }
    }
}
