canread-sharp is an implementation of the excellent readability.js, written in C#.

NOTE: Still in early development, not yet production-ready.

Article article = Article.GrabArticle("http://somedomain/somearticle");
Console.WriteLine(article.Title);
Console.WriteLine(article.ToString());