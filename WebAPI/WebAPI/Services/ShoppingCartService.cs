using Microsoft.Extensions.Hosting;
using System.Data;
using System.Text.Json;
using WebAPI.Models;
using static System.Net.Mime.MediaTypeNames;

namespace WebAPI.Services
{
    public interface IShoppingCart
    {
        List<Product> LoadExisingProduct();
        List<CartProduct> LoadExisingCart();
        void SaveProduct();
        void SaveCartProduct();
        Response GetAllProducts();
        Response AddNewProduct(Product product);
        Response UploadImage(IFormFile file, Guid id);
        Response UpdateProduct(Product product);
        Response DeleteProduct(Guid id);
        Response GetProductById(Guid id, bool isImagePath = false);
        Response AddToCart(CartProduct product);
        Response GetAllCartProducts(Guid userId, bool convertBase64 = true);
        Response DeleteCartProduct(Guid Id, Guid userId);
    }

    public class ShoppingCartService : IShoppingCart
    {
        private readonly string _productPath;
        private readonly string _cartPath;
        private readonly object _lock = new();
        private readonly IWebHostEnvironment _hostEnvironment;

        private List<Product> _productItems;
        private List<CartProduct> _cartItems;

        public ShoppingCartService(IWebHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
            var dir = Path.Combine(_hostEnvironment.ContentRootPath, "data/shopping-cart/database");
            Directory.CreateDirectory(dir);
            _productPath = Path.Combine(dir, "product.json");
            _cartPath = Path.Combine(dir, "cart.json");
            if (!File.Exists(_productPath)) File.Create(_productPath);
            if (!File.Exists(_cartPath)) File.Create(_cartPath);
            _productItems = LoadExisingProduct();
            _cartItems = LoadExisingCart();
        }
        public Response AddNewProduct(Product product)
        {
            Response response = new Response();
            try
            {
                lock (_lock)
                {
                    if (product.Id.ToString() == "00000000-0000-0000-0000-000000000000")
                    {
                        product.Id = Guid.NewGuid();
                    }
                    _productItems.Add(product);
                    SaveProduct();
                    response.product = product;
                    response.statusCode = 200;
                    response.statusMessage = "Product Added Successfully";
                }
            }
            catch (Exception ex)
            {
                response.statusCode = 100;
                response.statusMessage = $"Error adding the new product. Message: {ex.Message}";
            }
            return response;
        }

        public Response AddToCart(CartProduct product)
        {
            Response response = new Response();
            try
            {
                CartProduct cartProduct = GetAllCartProducts(product.UserId, false)?.listOfCartProducts?.FirstOrDefault(u => u.Id == product.Id && u.UserId == product.UserId);
                if (cartProduct != null)
                {
                    response.statusCode = 100;
                    response.statusMessage = $"Product already exists in cart";
                    return response;
                }
                var image = GetProductById(product.Id, true)?.product?.Image;
                if (product.Id == null)
                {
                    response.statusCode = 100;
                    response.statusMessage = $"{product.Id} is invalid";
                    return response;
                }
                product.Image = image;
                _cartItems.Add(product);
                SaveCartProduct();
                response.statusCode = 200;
                response.statusMessage = "Product Added to cart successfully";
                return response;
            }
            catch(Exception ex)
            {
                response.statusCode = 100;
                response.statusMessage = $"{product.Id} is invalid";
                return response;
            }
        }

        public Response DeleteCartProduct(Guid id, Guid userId)
        {
            Response response = new Response();
            try
            {
                CartProduct cartProduct = _cartItems.FirstOrDefault(x => x.Id == id && x.UserId == userId);
                if (cartProduct == null)
                {
                    response.statusCode = 100;
                    response.statusMessage = $"{id} doesn't exist in the cart";
                    return response;
                }
                _cartItems.Remove(cartProduct);
                SaveCartProduct();
                response.statusCode = 200;
                response.statusMessage = "Cart item Deleted Successfully";
                response.listOfCartProducts = _cartItems;
                return response;
            }
            catch(Exception ex)
            {
                response.statusCode = 100;
                response.statusMessage = $"Error when deleting the cart product. Messsage: {ex.Message}";
                return response;
            }
        }

        public Response DeleteProduct(Guid id)
        {
            Response response = new Response();
            try
            {
                lock (_lock)
                {
                    Product product = _productItems.FirstOrDefault(x => x.Id == id);
                    if (product == null)
                    {
                        response.statusCode = 404;
                        response.statusMessage = "Product doesn't exist";
                        return response;
                    }
                    var imagePath = product.Image;
                    var noImagePath = Path.Combine(_hostEnvironment.ContentRootPath, "data/shopping-cart/images/NoImage.jpg");
                    if (System.IO.File.Exists(imagePath) && imagePath != noImagePath)
                    {
                        System.IO.File.Delete(imagePath);
                    }
                    _productItems.Remove(product);
                    SaveProduct();
                    response.statusCode = 200;
                    response.statusMessage = "Product Deleted Successfully";
                }
            }
            catch (Exception ex)
            {
                response.statusCode = 100;
                response.statusMessage = $"Error deleting the product. Message: {ex.Message}";
            }
            return response;
        }

        public Response GetProductById(Guid id, bool isImagePath = false)
        {
            Response response = new Response();
            Product product = _productItems.FirstOrDefault(x => x.Id == id);
            if (product == null)
            {
                response.statusCode = 404;
                response.statusMessage = $"{id} data found";
                response.product = null;
                return response;
            }
            var image = product.Image;
            if (!isImagePath)
            {
                if (System.IO.File.Exists(image))
                {
                    var extension = Path.GetExtension(image).Split(".")[1];
                    image = Convert.ToBase64String(System.IO.File.ReadAllBytes(image));
                    image = $"data:image/{extension};base64,{image}";
                }
                else
                {
                    image = Convert.ToBase64String(System.IO.File.ReadAllBytes(Path.Combine(_hostEnvironment.ContentRootPath, "data/shopping-cart/images/", "NoImage.jpg")));
                    image = $"data:image/jpg;base64,{image}";
                }
            }
            product.Image = image;
            response.statusCode = 200;
            response.statusMessage = "Product found";
            response.product = product;
            return response;
        }

        public Response GetAllCartProducts(Guid userId, bool convertBase64 = true)
        {
            Response response = new Response();
            List<CartProduct> productCartsList = _cartItems.FindAll(p => p.UserId == userId);
            if (productCartsList.Count == 0)
            {
                response.statusCode = 100;
                response.statusMessage = "No data found";
                response.listOfCartProducts = new List<CartProduct>();
                return response;
            }
            response.statusCode = 200;
            response.statusMessage = "Cart Data found";
            response.listOfCartProducts = productCartsList;
            if (convertBase64)
            {
                List<CartProduct> products = productCartsList;
                ConvertImageToBase64(ref products);
                response.listOfCartProducts = products;
            }
            return response;
        }

        public Response GetAllProducts()
        {
            Response response = new Response();
            if (_productItems.Count > 0)
            {
                response.statusCode = 200;
                response.statusMessage = "Data found";
                List<Product> products = _productItems;
                ConvertImageToBase64(ref products);
                response.listOfProducts = products;
            }
            else
            {
                response.statusCode = 100;
                response.statusMessage = "No data found";
                response.listOfProducts = null;
            }
            return response;
        }

        public Response UpdateProduct(Product product)
        {
            Response response = new Response();
            try
            {
                lock (_lock)
                {
                    var idx = _productItems.FindIndex(x => x.Id == product.Id);
                    if (idx == -1)
                    {
                        response.statusCode = 400;
                        response.statusMessage = $"Product doesn't exist";
                        return response;
                    }
                    _productItems[idx] = new Product
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Image = product.Image,                        
                        Price = product.Price,                        
                    };
                    SaveProduct();
                    response.product = product;
                    response.statusCode = 200;
                    response.statusMessage = "Product Updated Successfully";
                }
            }
            catch (Exception ex)
            {
                response.statusCode = 100;
                response.statusMessage = $"Error updating the product. Message: {ex.Message}";
            }
            return response;
        }

        public Response UploadImage(IFormFile file, Guid id)
        {
            Response response = new Response();
            try
            {
                if (id.ToString() != "00000000-0000-0000-0000-000000000000")
                {
                    Product product = _productItems.FirstOrDefault(x => x.Id == id);
                    if (product == null)
                    {
                        response.statusCode = 404;
                        response.statusMessage = "Product doesn't exist";
                        return response;
                    }
                    var oldImagePath = product.Image;
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }
                var fileName = Path.GetFileName(file.FileName);
                var extension = Path.GetExtension(fileName);
                string fileGuid = id.ToString() != "00000000-0000-0000-0000-000000000000" ? id.ToString() : Guid.NewGuid().ToString();                
                var path = Path.Combine(_hostEnvironment.ContentRootPath, "data/shopping-cart/images/", fileGuid + extension);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
                response.imageUrl = path;
                response.statusCode = 200;
                response.statusMessage = $"Image {(id == null ? "Added" : "Updated")} Successfully";
            }
            catch (Exception ex)
            {
                response.statusCode = 100;
                response.statusMessage = $"Error adding the image. Message: {ex.Message}";
            }
            return response;
        }
        public List<Product> LoadExisingProduct()
        {
            try
            {
                var json = File.ReadAllText(_productPath);
                return JsonSerializer.Deserialize<List<Product>>(json) ?? new List<Product>();
            }
            catch
            {
                return new List<Product>();
            }
        }
        public List<CartProduct> LoadExisingCart()
        {
            try
            {
                var json = File.ReadAllText(_cartPath);
                return JsonSerializer.Deserialize<List<CartProduct>>(json) ?? new List<CartProduct>();
            }
            catch
            {
                return new List<CartProduct>();
            }
        }

        public void SaveProduct()
        {
            var json = JsonSerializer.Serialize(_productItems, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_productPath, json);
        }

        public void SaveCartProduct()
        {
            var json = JsonSerializer.Serialize(_cartItems, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_cartPath, json);
        }

        private void ConvertImageToBase64(ref List<Product> products)
        {
            foreach (var product in products)
            {
                if (System.IO.File.Exists(product.Image))
                {
                    var extension = Path.GetExtension(product.Image).Split(".")[1];
                    string image = Convert.ToBase64String(System.IO.File.ReadAllBytes(product.Image));
                    product.Image = $"data:image/{extension};base64,{image}";
                }
                else
                {
                    var path = Path.Combine(_hostEnvironment.ContentRootPath, "data/shopping-cart/images/", "NoImage.jpg");
                    string image = Convert.ToBase64String(System.IO.File.ReadAllBytes(path));
                    product.Image = $"data:image/jpg;base64,{image}";
                }
            }
        }


        private void ConvertImageToBase64(ref List<CartProduct>? cartProducts)
        {
            foreach (var product in cartProducts)
            {
                if (System.IO.File.Exists(product.Image))
                {
                    var extension = Path.GetExtension(product.Image).Split(".")[1];
                    string image = Convert.ToBase64String(System.IO.File.ReadAllBytes(product.Image));
                    product.Image = $"data:image/{extension};base64,{image}";
                }
                else
                {
                    var path = Path.Combine(_hostEnvironment.ContentRootPath, "data/shopping-cart/images/", "NoImage.jpg");
                    string image = Convert.ToBase64String(System.IO.File.ReadAllBytes(path));
                    product.Image = $"data:image/jpg;base64,{image}";
                }
            }
        }


        private void DeleteImage(string imagePath)
        {

        }
    }
}
