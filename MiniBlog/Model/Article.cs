namespace MiniBlog.Model
{
    public class Article
    {
        public Article()
        {
        }

        public Article(string userName, string title, string content)
        {
            Id = Guid.NewGuid();
            UserName = userName;
            Title = title;
            Content = content;
        }

        public Guid Id { get; set; }

        public string UserName { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }
    }
}