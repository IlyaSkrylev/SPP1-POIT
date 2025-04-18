using System;
using System.Collections.Generic;

namespace MyApp
{
    // 1. Класс Calculator с методами Add и Subtract
    public class Calculator
    {
        public Calculator() { }

        public int Add(int a, int b)
        {
            return a + b;
        }

        public int Subtract(int a, int b)
        {
            return a - b;
        }
    }

    // 2. Класс Logger с методом Log
    public class Logger
    {
        public Logger() { }

        public void Log(string message)
        {
            Console.WriteLine("Log: " + message);
        }
    }
}