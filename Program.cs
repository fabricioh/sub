using System;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using InquirerCS;

namespace reddit_sharp {
  class Program {

    static void Main(string[] args) {
      switch (args.Length) {
        case 0:
          Selector();
          return;
        case 1:
          GetSub(args[0]);
          return;
        case 3:
          GetSub(args[0], args[1], Int32.Parse(args[2]));
          return;
        default:
          Color(
            "sub@v0.1 - fabricio h\n\n" +
            "Digitando apenas 'sub' na linha de comando, o programa te pergunta algumas informações e exibe os posts de acordo.\n\n" +
            "Outra opção é passar diretamente alguns argumentos:\n\n" +
            "\tsub [subreddit] [filtro] [quantidade]\n\n" +
            "- subreddit: De qual subreddit puxar os posts\n" +
            "- filtro: Pode ser hot, new ou controversial\n" +
            "- quantidade: Quantos posts mostrar (máximo: 26)\n",
            ConsoleColor.Yellow
          );
          break;
      }
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

    static void GetSub(string name, string filter = "hot", int amount = 26) {
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
          Color($"Post URL: http://reddit.com{post["permalink"]}", ConsoleColor.Blue);
          Color($"Link: {post["url"]}", ConsoleColor.Blue);
          Color($"Score: {post["score"]}", ConsoleColor.Blue);
          Color($"Comments: {post["num_comments"]}\n", ConsoleColor.Blue);
          // Post
          Console.WriteLine(post["selftext"] + "\n");

          Console.WriteLine("---------------------------------------------");
        }
      }
    }

    static void Color(dynamic msg, ConsoleColor color) {
      Console.ForegroundColor = color;
      Console.WriteLine(msg.ToString());
      Console.ResetColor();
    }
  }
}
