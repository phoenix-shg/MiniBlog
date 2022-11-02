using MiniBlog.Model;

namespace MiniBlog.Stores
{
    public interface IUserStore
    {
        public List<User> GetAll();
        public User Save(User user);
        public bool Delete(User user);
    }
}
