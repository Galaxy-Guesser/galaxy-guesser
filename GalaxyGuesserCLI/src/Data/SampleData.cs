using System.Collections.Generic;
using GalaxyGuesserCli.Models;

namespace GalaxyGuesserCli.Data
{
    internal static class SampleData
    {
        public static List<Category> Categories { get; } = new List<Category>
        {
            new Category(1, "Galactic Trivia"),
            new Category(2, "Cosmic Science"),
            new Category(3, "Astronomy"),
            new Category(4, "Space Exploration")
        };

        public static List<Question> Questions { get; } = new List<Question>
        {
            // Galactic Trivia
            new Question(1, 1, "Which galaxy is the Milky Way on a collision course with?", 
                new[] { "Andromeda Galaxy", "Triangulum Galaxy", "Sombrero Galaxy", "Whirlpool Galaxy" }, 0),
            new Question(2, 1, "What is the most common type of galaxy in the universe?", 
                new[] { "Spiral Galaxy", "Elliptical Galaxy", "Irregular Galaxy", "Dwarf Elliptical Galaxy" }, 3),
            new Question(3, 1, "Approximately how many stars are in the Milky Way?", 
                new[] { "100 million", "1 billion", "100-400 billion", "1 trillion" }, 2),

            // Cosmic Science
            new Question(4, 2, "What is the primary gas that makes up the Sun?", 
                new[] { "Oxygen", "Helium", "Carbon Dioxide", "Hydrogen" }, 3),
            new Question(5, 2, "Which of these is NOT a state of matter found in space?", 
                new[] { "Plasma", "Bose-Einstein condensate", "Crystalline", "Neutronium" }, 2),
            new Question(6, 2, "What causes a star to go supernova?", 
                new[] { "Solar flares", "Collision with another star", "Gravitational collapse", "Hydrogen depletion" }, 2),

            // Astronomy
            new Question(7, 3, "Which planet has the Great Red Spot?", 
                new[] { "Mars", "Venus", "Jupiter", "Neptune" }, 2),
            new Question(8, 3, "What is the closest star to our Solar System?", 
                new[] { "Betelgeuse", "Proxima Centauri", "Alpha Centauri A", "Barnard's Star" }, 1),
            new Question(9, 3, "How many moons does Mars have?", 
                new[] { "0", "1", "2", "14" }, 2),

            // Space Exploration
            new Question(10, 4, "Who was the first person to walk on the Moon?", 
                new[] { "Buzz Aldrin", "Neil Armstrong", "John Glenn", "Yuri Gagarin" }, 1),
            new Question(11, 4, "Which was the first spacecraft to reach interstellar space?", 
                new[] { "Voyager 1", "Pioneer 10", "New Horizons", "Voyager 2" }, 0),
            new Question(12, 4, "What was the name of the first space station?", 
                new[] { "Mir", "ISS", "Skylab", "Salyut 1" }, 3)
        };

        public static Dictionary<string, string> UserCredentials { get; } = new Dictionary<string, string>
        {
            { "admin", "admin123" },
            { "test", "test123" }
        };
    }
}
