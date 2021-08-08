using LiteDB;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using WebUrlShort.Models;

namespace WebUrlShort.Delegates
{
    public static class RequestDelegates
    {
        static ILiteCollection<ShortUrl> GetDbCollection(HttpContext context)
        {
            var db = context.RequestServices.GetService<ILiteDatabase>();
            var collection = db.GetCollection<ShortUrl>();
            return collection;
        }

        /// <summary>
        /// { url: "https://google.com" } 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static async Task CreateUrl(HttpContext context)
        {
            ShortUrl jsonRequest = await context.Request.ReadFromJsonAsync<ShortUrl>();

            var shortModel = new ShortUrl(new Uri(jsonRequest.Url, UriKind.Absolute));
            ILiteCollection<ShortUrl> collection = GetDbCollection(context);

            collection.Insert(shortModel);

            var rawShortUrl = $"https://localhost:5001/{shortModel.Chunk}";

            //context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(new { url = rawShortUrl });
        }

        internal static async Task GetHomePage(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            await context.Response.SendFileAsync("wwwroot/index.html");
        }


        /// <summary>
        /// https://localhost:5001/{Chunk}
        /// https://localhost:5001/abc123
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static Task RedirectToUrl(HttpContext context)
        {
            var chunk = context.Request.Path.Value.TrimStart('/');

            var dbColl = GetDbCollection(context);

            var shortModel = dbColl.FindOne(x => x.Id == chunk);

            if (shortModel != null) context.Response.Redirect(shortModel.Url);
            else context.Response.Redirect("/");

            return Task.CompletedTask;
        }
    }
}
