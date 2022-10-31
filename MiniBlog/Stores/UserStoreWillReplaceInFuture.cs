namespace MiniBlog.Stores
{
    using Model;

    public class UserStoreWillReplaceInFuture
    {
        private List<User> users;

        public static readonly UserStoreWillReplaceInFuture Instance = new UserStoreWillReplaceInFuture();

        private UserStoreWillReplaceInFuture()
        {
            Init();
        }

        public List<User> GetAll()
        {
            return this.users;
        }

        public User Save(User user)
        {
            this.users.Add(user);
            return user;
        }

        public bool Delete(User user)
        {
            return this.users.Remove(user);
        }

        public void Init()
        {
            users = new List<User>();
        }
    }
}