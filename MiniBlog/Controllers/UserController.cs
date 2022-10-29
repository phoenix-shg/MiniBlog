using MiniBlog.Model;
using MiniBlog.Stores;
using Microsoft.AspNetCore.Mvc;

namespace MiniBlog.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        [HttpPost]
        public User Register(User user)
        {
            if (!UserStoreWillReplaceInFuture.instance.Users.Exists(_ => user.Name.ToLower() == _.Name.ToLower()))
            {
                UserStoreWillReplaceInFuture.instance.Users.Add(user);
            }

            return user;
        }

        [HttpGet]
        public List<User> GetAll()
        {
            return UserStoreWillReplaceInFuture.instance.Users;
        }

        [HttpPut]
        public User Update(User user)
        {
            var foundUser = UserStoreWillReplaceInFuture.instance.Users.FirstOrDefault(_ => _.Name == user.Name);
            if (foundUser != null)
            {
                foundUser.Email = user.Email;
            }

            return foundUser;
        }

        [HttpDelete]
        public User Delete(string name)
        {
            var foundUser = UserStoreWillReplaceInFuture.instance.Users.FirstOrDefault(_ => _.Name == name);
            if (foundUser != null)
            {
                UserStoreWillReplaceInFuture.instance.Users.Remove(foundUser);
                ArticleStoreWillReplaceInFuture.instance.Articles.RemoveAll(a => a.UserName == foundUser.Name);
            }

            return foundUser;
        }

        [HttpGet("{name}")]
        public User GetByName(string name)
        {
            return UserStoreWillReplaceInFuture.instance.Users.FirstOrDefault(_ => _.Name.ToLower() == name.ToLower());
        }
    }
}