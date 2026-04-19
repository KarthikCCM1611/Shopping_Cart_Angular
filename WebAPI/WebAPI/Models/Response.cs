namespace WebAPI.Models
{
    public class Response
    {
        public int statusCode { get; set; }
        public string statusMessage { get; set; } = string.Empty;
        public List<Product> listOfProducts { get; set; }
        public List<CartProduct> listOfCartProducts { get; set; }
        public Product product { get; set; }
        public string imageUrl { get; set; } = string.Empty;
    }
}
