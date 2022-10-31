namespace MiniBlog.Stores
{
    using MiniBlog.Model;

    public class ArticleStoreWillReplaceInFuture
    {
        private List<Article> articles;

        private ArticleStoreWillReplaceInFuture()
        {
            this.Init();
        }

        public static readonly ArticleStoreWillReplaceInFuture Instance = new ArticleStoreWillReplaceInFuture();

        public Article Save(Article article)
        {
            this.articles.Add(article);
            return article;
        }

        public List<Article> GetAll()
        {
            return this.articles;
        }

        public bool Delete(Article articles)
        {
            return this.articles.Remove(articles);
        }
        
        /// <summary>
        /// This is for test only, please help resolve!
        /// </summary>
        public void Init()
        {
            articles = new List<Article>();
            articles.Add(new Article(null, "Happy new year", "Happy 2021 new year"));
            articles.Add(new Article(null, "Happy Halloween", "Halloween is coming"));
        }
    }
}