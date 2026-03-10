# C# Learning Journey 🚀

**Day-by-Day Revision & Notes**  
A structured, beginner-to-intermediate C# tutorial series with definitions, syntax, examples, and best practices.

**Author:** Arvind Kumar  
**GitHub:** [@arvind01A](https://github.com/arvind01A)  
**Started:** March 2026  
**Goal:** Build strong fundamentals → move to OOP, collections, LINQ, async, .NET projects

![C# Badge](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![.NET Badge](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)
![Visual Studio](https://img.shields.io/badge/Visual%20Studio-2026-5C2D91?style=for-the-badge&logo=visual-studio)

---

## 📚 Table of Contents

- [Overview](#overview)
- [Progress & Completed Days](#progress--completed-days)
- [Day 1 – Fundamentals](#day-1--fundamentals)
- [Day 2 – Classes & Objects](#day-2--classes--objects)
- [Day 3 – OOPs](#day-3--OOPs)
- [Day 4 – Array, String & Tuples](#day-4--Array)
- [Day 5 – Generics](#day-5--Generic)
- [Day 6 – Collections](#day-6--Collections)
- [Upcoming Topics](#upcoming-topics)
- [How to Use This Repo](#how-to-use-this-repo)
- [Tools & Environment](#tools--environment)
- [Contributing & Feedback](#contributing--feedback)

---

## Overview

This repository documents my daily C# revision — from basics to advanced concepts.  
Each day includes:

- Clear **definitions**
- **Syntax** examples
- Practical **code snippets** (copy-paste ready)
- Key **differences** & common pitfalls
- Mini summary tables when useful

Perfect for beginners, self-learners, or anyone refreshing C# skills in 2026!

---

## Progress & Completed Days

| Day | Topic                          | Status     | Link in Repo                  |
|-----|--------------------------------|------------|-------------------------------|
| 1   | Fundamentals (Variables, Operators, Control, Loops) | ✅ Done   | [Day 1 Notes](./README.md) |
| 2   | Classes & Objects (Methods, Parameters) | ✅ Done   | [Day 2 Notes](./README.md)      |
| 3   | OOPs    | ✅ Done | [Day 3 Notes](./README.md)                             |
| 4   | Basic Data Structures (Array, String, Tuples) | ✅ Done | [Day 4 Notes](./README.md)   |
| 5   | Generics (Classes, Methods, Contraints)       | ✅ Done        | [Day 5 Notes](./README.md)   |
| 6   | Collections (Non-Generic, Generic, Speciallized)  | ✅ Done        | [Day 6 Notes](./README.md)   |
| ... | ...                            | ...        | ...                           |

> Last updated: March 09, 2026

```ansi
## Repository Structure

*csharp-learning
   ├── day1/ Fundamentals
   │     └── 01-Variables
   │     └── 02-Operators
   │     └── 03-control-statements
   │     └── 04-jump-statements
   │     └── 05-loops
   ├── day2/ Classes & Objects
   │     └── 01-Methods and Parameters            
   │                   └── 01-Return Types
   │                   └── 02-Parameter Passing
   │                   └── 03-Optional Parameters
   │                   └── 04-Named Arguments
   │      └── 02-Constructors
   │                   └── 01-Default Constructors
   │                   └── 02-Parameterized Constructors
   │                   └── 03-Constructor Overloading
   │                   └── 04-Static Constructors & Copy Constructors
   │      └── 03-Fields (data)
   ├── day3/ OOPs 4 Pillars
   │     └── 01-Encapsulation
   │                  └── 01-Access Modifiers
   │                  └── 02-Properties
   │     └── 02-Inheritance
   │                  └── 01-Types of Inheritance
   │                  └── 02-base and this keywords
   │     └── 03-Abstraction         
   │                  └── Abstract Classes & Interfaces
   │     └── 04-Polymorphism
   │                  └──01-Compile-Time
   │                  └──02-Runtime
   │     └── 05-Indexers
   ├── day4/ Basic Data Structures
   │    └── Arrays
   │            ├── Single-Dimensional    
   │            ├── Array Methods          
   │            ├── Array Class           
   │            └── Multi-Dim / Jagged     
   │    └── Strings
   │            ├── String Operations      
   │            ├── Interpolation ($)      
   │            └── String vs StringBuilder 
   │    └── Tuples
   │             └── Named Tuples          
   │
   ├── day5/ Generics
   │     ├── Generic Classes         public class MyClass<T> { }
   │     │   ├── Single type param   Box<T>
   │     │   └── Multiple params     Pair<TKey, TValue>
   │     │
   │     ├── Generic Methods         public static void Method<T>(T val) { }
   │     │   ├── Explicit call       Method<int>(42)
   │     │   └── Type inference      Method(42)   ← compiler infers T = int
   │     │
   │     └── Generic Constraints     where T : ...
   │         ├── where T : class              reference types only
   │         ├── where T : struct             value types only
   │         ├── where T : new()              can do new T()
   │         ├── where T : IComparable<T>     can compare values
   │         └── Combined: where T : class, IAnimal, new()
   ├── day6/ Collections
   │     ├── Non-Generic Collections
   │     │       ├── ArrayList
   │     │       ├── Hashtable
   │     │       └── Queue, Stack, SortedList
   │     ├── Generic Collections
   │     │       ├── List<T>
   │     │       ├── Dictionary<TKey, TValue>
   │     │       └── Queue<T>, Stack<T>, HashSet<T>, LinkedList<T>, SortedList<T>
   │     └── Specialized Collections
   │             └── Concurrent Collections
   │     
   │     
   │
   │
   │
   │
   │
   │
   │
```
