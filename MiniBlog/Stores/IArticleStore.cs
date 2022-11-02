using MiniBlog.Model;

namespace MiniBlog.Stores
{
    public interface IArticleStore
    {
        Article Save(Article article);
        List<Article> GetAll();
        bool Delete(Article article);
    }
}
