using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace WebApplication3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private IRepository _repository;
        public ArticlesController(IRepository repository)
        {
            _repository = repository;

        }

        [HttpGet]
        [Route("/get")]
        public ActionResult Get(Guid guid)
        {
            var article = _repository.Get(guid);

            if (article == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Article not found");
            }

            return StatusCode(StatusCodes.Status201Created, JsonSerializer.Serialize(article));
        }

        [Route("/create")]
        [HttpPost]
        public ActionResult Create(string title, string text)
        {
            if (string.IsNullOrEmpty(title))
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            var obj = new Article() { Title = title, Id = Guid.NewGuid(), Text = text };
            _repository.Create(obj);
            return StatusCode(StatusCodes.Status201Created, JsonSerializer.Serialize(obj));
        }

        [Route("/delete")]
        [HttpDelete]
        public ActionResult Delete(Guid guid)
        {
            bool isdeleted = _repository.Delete(guid);
            if (!isdeleted)
                return StatusCode(StatusCodes.Status400BadRequest, "Article not found");

            return StatusCode(StatusCodes.Status200OK);
        }

        [Route("/update")]
        [HttpPut]
        public ActionResult Update(Guid guid)
        {
            var article = _repository.Get(guid);

            if (article == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Article not found");

            }
            _repository.Update(article);
            return StatusCode(StatusCodes.Status200OK);
        }
    }

    public class Article
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
    }



    public class Repository : IRepository
    {
        private static List<Article> articles = new List<Article>();
        public Article Get(Guid id)
        {
            if (!articles.Any(i => i.Id == id))
                return null;
            var article = articles.Where(i => i.Id == id).FirstOrDefault();
            return article;
        }
        // Returns the identifier of a created article.
        // Throws an exception if an article is null.
        // Throws an exception if a title is null or empty.
        public Guid Create(Article article)
        {
            articles.Add(article);
            return article.Id;
        }

        // Returns true if an article was deleted or false if it was not possible to find it.
        public bool Delete(Guid id)
        {
            if (!articles.Any(i => i.Id == id))
            {
                return false;

            }
            else
            {
                articles.Remove(articles.Where(i => i.Id == id).FirstOrDefault());
                return true;
            }

        }
        // Returns true if an article was updated or false if it was not possible to find it.
        // Throws an exception if an articleToUpdate is null.
        // Throws an exception if a title is null or empty.
        public bool Update(Article articleToUpdate)
        {

            return true;
        }
    }

    public interface IRepository
    {
        // Returns a found article or null.
        Article Get(Guid id);
        // Returns the identifier of a created article.
        // Throws an exception if an article is null.
        // Throws an exception if a title is null or empty.
        Guid Create(Article article);
        // Returns true if an article was deleted or false if it was not possible to find it.
        bool Delete(Guid id);
        // Returns true if an article was updated or false if it was not possible to find it.
        // Throws an exception if an articleToUpdate is null.
        // Throws an exception if a title is null or empty.
        bool Update(Article articleToUpdate);
    }
}
