using MiniBlog.Model;
using MiniBlog.Stores;

namespace MiniBlog.Services
{
    public class UserService
    {
        private IArticleStore articleStore;
        private IUserStore userStore;

        public UserService(IArticleStore articleStore, IUserStore userStore)
        {
            this.articleStore = articleStore;
            this.userStore = userStore;
        }

        public User Register(User user)
        {
            if (!this.userStore.GetAll().Exists(_ => user.Name.ToLower() == _.Name.ToLower()))
            {
                this.userStore.Save(user);
            }

            return user;
        }
    }
}
