// using System;
// using System.Collections.Generic;
// using System.Linq;
// using ConsoleApp1.Models.Category;
// using ConsoleApp1.Models.Player;
// using ConsoleApp1.Models.Question;
// using ConsoleApp1.Models.Session;
// using ConsoleApp1.Models.SessionPlayer;
// using ConsoleApp1.Models.SessionQuestion;
// using ConsoleApp1.Models.SessionScore;

// namespace ConsoleApp1.Services
// {
//     public class DataService
//     {
//         // In-memory data storage
//         private List<Player> _players = new List<Player>();
//         private List<Session> _sessions = new List<Session>();
//         private List<Category> _categories = new List<Category>();
//         private List<SessionPlayer> _sessionPlayers = new List<SessionPlayer>();
//         private List<SessionQuestion> _sessionQuestions = new List<SessionQuestion>();
//         private List<Question> _questions = new List<Question>();
//         private List<SessionScore> _sessionScores = new List<SessionScore>();
//         private Dictionary<string, string> _userCredentials = new Dictionary<string, string>();
        
//         public void InitializeData()
//         {
//             // Create default categories
//             _categories = new List<Category>
//             {
//                 new Category(1, "Galactic Trivia"),
//                 new Category(2, "Cosmic Science"),
//                 new Category(3, "Astronomy"),
//                 new Category(4, "Space Exploration")
//             };

//             // Create sample questions for each category
//             _questions = new List<Question>
//             {
//                 // Galactic Trivia
//                 new Question(1, 1, "Which galaxy is the Milky Way on a collision course with?", 
//                     new[] { "Andromeda Galaxy", "Triangulum Galaxy", "Sombrero Galaxy", "Whirlpool Galaxy" }, 0),
//                 new Question(2, 1, "What is the most common type of galaxy in the universe?", 
//                     new[] { "Spiral Galaxy", "Elliptical Galaxy", "Irregular Galaxy", "Dwarf Elliptical Galaxy" }, 3),
//                 new Question(3, 1, "Approximately how many stars are in the Milky Way?", 
//                     new[] { "100 million", "1 billion", "100-400 billion", "1 trillion" }, 2),
                
//                 // Cosmic Science
//                 new Question(4, 2, "What is the primary gas that makes up the Sun?", 
//                     new[] { "Oxygen", "Helium", "Carbon Dioxide", "Hydrogen" }, 3),
//                 new Question(5, 2, "Which of these is NOT a state of matter found in space?", 
//                     new[] { "Plasma", "Bose-Einstein condensate", "Crystalline", "Neutronium" }, 2),
//                 new Question(6, 2, "What causes a star to go supernova?", 
//                     new[] { "Solar flares", "Collision with another star", "Gravitational collapse", "Hydrogen depletion" }, 2),
                
//                 // Astronomy
//                 new Question(7, 3, "Which planet has the Great Red Spot?", 
//                     new[] { "Mars", "Venus", "Jupiter", "Neptune" }, 2),
//                 new Question(8, 3, "What is the closest star to our Solar System?", 
//                     new[] { "Betelgeuse", "Proxima Centauri", "Alpha Centauri A", "Barnard's Star" }, 1),
//                 new Question(9, 3, "How many moons does Mars have?", 
//                     new[] { "0", "1", "2", "14" }, 2),
                
//                 // Space Exploration
//                 new Question(10, 4, "Who was the first person to walk on the Moon?", 
//                     new[] { "Buzz Aldrin", "Neil Armstrong", "John Glenn", "Yuri Gagarin" }, 1),
//                 new Question(11, 4, "Which was the first spacecraft to reach interstellar space?", 
//                     new[] { "Voyager 1", "Pioneer 10", "New Horizons", "Voyager 2" }, 0),
//                 new Question(12, 4, "What was the name of the first space station?", 
//                     new[] { "Mir", "ISS", "Skylab", "Salyut 1" }, 3)
//             };

//             // Add some default users for testing
//             _userCredentials.Add("admin", "admin123");
//             _userCredentials.Add("test", "test123");
//         }
        
//         public bool ValidateCredentials(string username, string password)
//         {
//             return _userCredentials.TryGetValue(username, out string storedPassword) && password == storedPassword;
//         }
        
//         public bool UserExists(string username)
//         {
//             return _userCredentials.ContainsKey(username);
//         }
        
//         public void AddCredentials(string username, string password)
//         {
//             _userCredentials.Add(username, password);
//         }
        
//         public Player GetPlayerByUsername(string username)
//         {
//             return _players.FirstOrDefault(p => p.Username == username);
//         }
        
//         public Player CreatePlayer(string username, string name)
//         {
//             int playerId = _players.Count > 0 ? _players.Max(p => p.Id) + 1 : 1;
//             Guid playerGuid = Guid.NewGuid();

//             Player player = new Player(playerId, playerGuid, username, name);
//             _players.Add(player);
//             return player;
//         }
        
//         public string GenerateSessionCode()
//         {
//             const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
//             var random = new Random();
//             return new string(Enumerable.Repeat(chars, 6)
//                 .Select(s => s[random.Next(s.Length)]).ToArray());
//         }
        
//         public List<Category> GetCategories()
//         {
//             return _categories;
//         }
        
//         public Session CreateSession(int categoryId, int questionDuration, int questionCount, string sessionCode)
//         {
//             int sessionId = _sessions.Count > 0 ? _sessions.Max(s => s.Id) + 1 : 1;
//             Session session = new Session(sessionId, sessionCode, categoryId, questionDuration, questionCount);
//             _sessions.Add(session);
//             return session;
//         }
        
//         public Session GetSessionByCode(string code)
//         {
//             return _sessions.FirstOrDefault(s => s.Code == code);
//         }
        
//         public void AddPlayerToSession(int playerId, int sessionId)
//         {
//             // Check if player already in session
//             if (!_sessionPlayers.Any(sp => sp.PlayerId == playerId && sp.SessionId == sessionId))
//             {
//                 _sessionPlayers.Add(new SessionPlayer(sessionId, playerId));
//             }
//         }
        
//         public void AddQuestionsToSession(int sessionId, int categoryId, int questionCount)
//         {
//             // Get questions for this category and randomize them
//             var categoryQuestions = _questions
//                 .Where(q => q.CategoryId == categoryId)
//                 .OrderBy(q => Guid.NewGuid()) // Random order
//                 .Take(questionCount)  // Only take requested number of questions
//                 .ToList();
            
//             int id = _sessionQuestions.Count > 0 ? _sessionQuestions.Max(sq => sq.Id) + 1 : 1;
//             foreach (var question in categoryQuestions)
//             {
//                 _sessionQuestions.Add(new SessionQuestion(id++, sessionId, question.Id));
//             }
//         }
        
//         public List<Question> GetSessionQuestions(int sessionId)
//         {
//             return _sessionQuestions
//                 .Where(sq => sq.SessionId == sessionId)
//                 .Join(
//                     _questions, 
//                     sq => sq.QuestionId, 
//                     q => q.Id, 
//                     (sq, q) => q
//                 )
//                 .ToList();
//         }
        
//         public void SaveScore(int playerId, int sessionId, int score, int timeRemaining = 0)
//         {
//             _sessionScores.Add(new SessionScore(playerId, sessionId, score, timeRemaining));
//         }
        
//         public List<dynamic> GetSessionLeaderboard(int sessionId)
//         {
//             return _sessionScores
//                 .Where(s => s.SessionId == sessionId)
//                 .OrderByDescending(s => s.Score + s.TimeRemaining)
//                 .Select(s => new { 
//                     Name = _players.First(p => p.Id == s.PlayerId).Name, 
//                     Score = s.Score,
//                     TimeBonus = s.TimeRemaining,
//                     Total = s.Score + s.TimeRemaining
//                 })
//                 .ToList<dynamic>();
//         }
        
//         public SessionScore GetPlayerScore(int playerId, int sessionId)
//         {
//             return _sessionScores.FirstOrDefault(s => s.PlayerId == playerId && s.SessionId == sessionId);
//         }
        
//         public int GetSessionQuestionCount(int sessionId)
//         {
//             return _sessionQuestions.Count(sq => sq.SessionId == sessionId);
//         }
//     }
// }