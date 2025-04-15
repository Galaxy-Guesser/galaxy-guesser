 namespace ConsoleApp1.Models
{
     public class Category
    {
        public int categoryId { get; set; }
        public string category { get; set; }

        public Category(int id, string name)
        {
            categoryId = id;
            category = name;
        }
    }

    public class Categories
    {
        public int categoryId { get; set; }
        public string category { get; set; }

    }
}