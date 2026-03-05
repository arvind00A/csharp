// See https://aka.ms/new-console-template for more information

using Fields;

Employee emp1 = new Employee("Alice", 30, "E001");
Employee emp2 = new Employee("Bob", 25, "E002");

Console.WriteLine($"{emp1.Name} works at {Employee.CompanyName}");  // TechCorp
Console.WriteLine($"Total employees: {Employee.TotalEmployees}");   // 2