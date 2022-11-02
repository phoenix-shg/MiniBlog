using MiniBlog.Model;
using MiniBlog.Stores;
using Microsoft.AspNetCore.Mvc;
using MiniBlog.Services;

namespace MiniBlog.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private IArticleStore _articleStore;
        private IUserStore _userStore;
        private UserService _userService;
        public UserController(IArticleStore articleStore, IUserStore userStore,UserService userService)
        {
            _articleStore = articleStore;
            _userStore = userStore;
            _userService = userService;
        }

        [HttpPost]
        public ActionResult<User> Register(User user)
        {
            return Created("/user", _userService.Register(user));
        }

        [HttpGet]
        public List<User> GetAll()
        {
            return _userService.GetAll();
        }

        [HttpPut]
        public User Update(User user)
        {
            return _userService.Update(user);
        }

        [HttpDelete]
        public User Delete(string name)
        {
            return _userService.Delete(name);
        }

        [HttpGet("{name}")]
        public User GetByName(string name)
        {
            return _userService.GetByName(name);
        }
    }
}