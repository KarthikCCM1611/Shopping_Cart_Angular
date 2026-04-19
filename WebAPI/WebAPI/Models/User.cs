namespace WebAPI.Models
{
    public class User: Register
    {
        public Guid id { get; set; } = Guid.NewGuid();
    }
}
