
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using GalaxyGuesserCLI.DTO;
using GalaxyGuesserCLI.Services;

namespace GalaxyGuesserCLI
{
    class Program
    {
        public static void Main()
        {
             GalaxyQuiz.Start().GetAwaiter().GetResult();
        }
    }
}