using System;
using System.Collections.Generic;
using MiniBlog.Model;

namespace MiniBlog.Stores
{
    public class ArticleStoreWillReplaceInFuture
    {
        private ArticleStoreWillReplaceInFuture()
        {
            this.Init();
        }

        public static readonly ArticleStoreWillReplaceInFuture instance = new();

        public List<Article> Articles { get;  set; }

        public void Init()
        {
            Articles = new List<Article>();
            Articles.Add(new Article(null, "Happy new year", "Happy 2021 new year"));
            Articles.Add(new Article(null, "Happy Halloween", "Halloween is coming"));
        }
    }
}