// See https://aka.ms/new-console-template for more information
using ControlStatements;

Console.WriteLine("Hello, dev!");

// if – else if – else
IfElse ifElse = new IfElse();
ifElse.IfElseTest();
ifElse.NestedIfElseTest();


// switch – case (including break & default)
SwitchCase switchCase = new SwitchCase();
switchCase.SwitchCaseTest();
switchCase.SwitchExpressionTest();
switchCase.SwitchPatternMatching();