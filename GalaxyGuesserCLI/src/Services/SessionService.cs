using System;
using System.Threading;
using System.Threading.Tasks;
using ConsoleApp1.Models;

namespace ConsoleApp1.Services
{
    public class SessionService
    {
        public static string GenerateSessionCode()
        {
            const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 6)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}