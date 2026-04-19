using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCart _shoppingCartSrc;
        public ShoppingCartController(IShoppingCart shoppingCartSrc)
        {
            _shoppingCartSrc = shoppingCartSrc;
        }

        [HttpGet("GetAllProducts")]
        public Response GetAllProducts()
        {
            return _shoppingCartSrc.GetAllProducts();
        }

        [HttpPost("AddNewProduct")]
        public Response AddNewProduct(Product product)
        {
            return _shoppingCartSrc.AddNewProduct(product);
        }

        [HttpPut("UpdateProduct")]
        public Response UpdateProduct(Product product)
        {
            return _shoppingCartSrc.UpdateProduct(product);
        }

        [HttpDelete("DeleteProduct")]
        public Response DeleteProduct(Guid id)
        {
            return _shoppingCartSrc.DeleteProduct(id);
        }

        [HttpGet("GetProductById")]
        public Response GetProductById(Guid id, bool isImagePath = false)
        {
            return _shoppingCartSrc.GetProductById(id);
        }

        [HttpPost("AddToCart")]
        public Response AddToCart(CartProduct product)
        {
            return _shoppingCartSrc.AddToCart(product);
        }

        [HttpGet("GetAllCartProducts")]
        public Response GetAllCartProducts(Guid userId)
        {
            return _shoppingCartSrc.GetAllCartProducts(userId);
        }

        [HttpDelete("DeleteCartProduct")]
        public Response DeleteCartProduct(Guid id, Guid userId)
        {
            return _shoppingCartSrc.DeleteCartProduct(id, userId);
        }

        [HttpPost]
        [Route("UploadImage")]
        public Response UploadImage([FromForm] IFormFile file, [FromQuery] Guid id)
        {
            return _shoppingCartSrc.UploadImage(file, id);
        }
    }
}
