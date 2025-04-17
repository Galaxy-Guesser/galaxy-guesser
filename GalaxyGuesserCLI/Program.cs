
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using ConsoleApp1.Models;
using ConsoleApp1.Services;

namespace ConsoleApp1
{
    class Program
    {
        public static void Main()
        {
             GalaxyQuiz.Start().GetAwaiter().GetResult();
        }
    }
}