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
            if (!UserStoreWillReplaceInFuture.Instance.GetAll().Exists(_ => user.Name.ToLower() == _.Name.ToLower()))
            {
                UserStoreWillReplaceInFuture.Instance.Save(user);
            }

            return user;
        }

        [HttpGet]
        public List<User> GetAll()
        {
            return UserStoreWillReplaceInFuture.Instance.GetAll();
        }

        [HttpPut]
        public User Update(User user)
        {
            var foundUser = UserStoreWillReplaceInFuture.Instance.GetAll().FirstOrDefault(_ => _.Name == user.Name);
            if (foundUser != null)
            {
                foundUser.Email = user.Email;
            }

            return foundUser;
        }

        [HttpDelete]
        public User Delete(string name)
        {
            var foundUser = UserStoreWillReplaceInFuture.Instance.GetAll().FirstOrDefault(_ => _.Name == name);
            if (foundUser != null)
            {
                UserStoreWillReplaceInFuture.Instance.Delete(foundUser);
                var articles = ArticleStoreWillReplaceInFuture.Instance.GetAll()
                    .Where(article => article.UserName == foundUser.Name)
                    .ToList();
                articles.ForEach(article => ArticleStoreWillReplaceInFuture.Instance.Delete(article));
            }

            return foundUser;
        }

        [HttpGet("{name}")]
        public User GetByName(string name)
        {
            return UserStoreWillReplaceInFuture.Instance.GetAll().FirstOrDefault(_ =>
                string.Equals(_.Name, name, StringComparison.CurrentCultureIgnoreCase)) ?? throw new
                InvalidOperationException();
        }
    }
}