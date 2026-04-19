using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Win32;
using Newtonsoft.Json;
using System.Text.Json;
using WebAPI.Models;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace WebAPI.Services
{
    public interface IMaster
    {
        List<User> LoadExistingUsers();
        string Register(Register register);
        string Login(Login login);
        void SaveUser();
    }

    public class MasterService: IMaster
    {
        private readonly string _userPath;
        private List<User> _users;

        private readonly IWebHostEnvironment _hostEnvironment;
        public MasterService(IWebHostEnvironment hostEnvironment) {
            _hostEnvironment = hostEnvironment;
            var dir = Path.Combine(_hostEnvironment.ContentRootPath, "data/user-data");
            Directory.CreateDirectory(dir);
            _userPath = Path.Combine(dir, "users.json");
            if (!File.Exists(_userPath)) File.Create(_userPath);
            _users = LoadExistingUsers();
        }

        public List<User> LoadExistingUsers()
        {
            try
            {
                var json = File.ReadAllText(_userPath);
                return JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
            }
            catch
            {
                return new List<User>();
            }
        }

        public string Login(Login login)
        {
            try
            {
                User? existingUser = _users.FirstOrDefault(usr => usr.email == login.email);
                if (existingUser == null)
                {
                    return JsonConvert.SerializeObject(new { message = "User doesn't exist", statusCode = 404 });
                    //return "User doesn't exist";
                }
                if (login.password != existingUser.password)
                {
                    return JsonConvert.SerializeObject(new { message = "Pasword is incorrect, Please check the entered password", statusCode = 401 });
                }
                return JsonConvert.SerializeObject(new { message = "User Login Success", statusCode = 200, user = existingUser });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { message = ex.Message, statusCode =  400 });
            }
        }

        public string Register(Register register)
        {
            try
            {
                User? existingUser = _users.FirstOrDefault(usr => usr.email == register.email);
                if (existingUser != null)
                {
                    return JsonConvert.SerializeObject(new { message= "Email already exist", statusCode = 409  });
                }
                User user = new User();
                user.userName = register.userName;
                user.email = register.email;
                user.password = register.password;
                user.phone = register.phone;
                user.city = register.city;
                _users.Add(user);
                SaveUser();
                return JsonConvert.SerializeObject(new { message = "User Registration Success", statusCode = 200, user = user });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { message = ex.Message, statusCode = 400 });
            }
        }

        public void SaveUser()
        {
            var json = JsonSerializer.Serialize(_users, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_userPath, json);
        }
    }
}
