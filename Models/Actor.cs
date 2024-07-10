namespace BasicEfCoreDemo.Models
{
    public class Actor
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<Movie> Movies { get; set; } = new List<Movie>();
    }
}
