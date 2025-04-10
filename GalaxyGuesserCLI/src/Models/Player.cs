 namespace ConsoleApp1.Models
{
    class Player
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }

        public Player(int id, Guid guid, string username, string name)
        {
            Id = id;
            Guid = guid;
            Username = username;
            Name = name;
        }
    }
}
