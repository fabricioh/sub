using System;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using InquirerCS;

using static Util;

// https://www.reddit.com/r/programmingcirclejerk.json

namespace reddit_sharp {
  class Program {
    static void Main(string[] args) {
      if (args.Length == 0) {
        Selector();
        return;
      }

      GetSub(args[0], args[1], Int32.Parse(args[2]));
    }

    static void Selector() {
      var sub = "all";
      var filter = "hot";
      var amount = "20";

      sub = Question.Input("Qual subreddit você quer ler?").Prompt();
      filter = Question.List("Qual?", new List<string>{"hot", "new", "controversial"}).Prompt();
      amount = Question.Input("Quantos posts?").Prompt();

      Console.Clear();
      GetSub(sub, filter, Int32.Parse(amount));
    }

    static void GetSub(string name, string filter, int amount) {
      using (var web = new WebClient()) {
        var data = web.DownloadString($"https://www.reddit.com/r/{name}/{filter}.json");

        var obj = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(data);
        var posts = new List<JObject>();

        foreach (var elem in obj["data"]["children"]) {
          posts.Add(elem["data"]);
        }

        posts.Reverse();

        foreach (var post in posts.Take(amount)) {
          // Título & Autor
          Color($"\n{post["title"]}", ConsoleColor.Yellow);
          Color($"Author: {post["author"]}", ConsoleColor.Yellow);
          // Info
          Color($"URL: {post["url"]}", ConsoleColor.Blue);
          Color($"Score: {post["score"]}", ConsoleColor.Blue);
          Color($"Comments: {post["num_comments"]}\n", ConsoleColor.Blue);
          // Post
          Console.WriteLine(post["selftext"] + "\n");

          Console.WriteLine("---------------------------------------------");
        }
      }
    }
  }
}
