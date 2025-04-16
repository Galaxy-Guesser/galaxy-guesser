
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using GalaxyGuesserCli.Models;
using GalaxyGuesserCli.Services;
using GalaxyGuesserCli.Data;

namespace GalaxyGuesserCli
{
    class Program
    {
        public static void Main()
        {
             GalaxyQuiz.Start().GetAwaiter().GetResult();
        }
    }
}