using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HtmlAgilityPack;

namespace datapullread
{
    class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Sahibinden Vitrin Listeleme \n");
            Console.WriteLine("!!! UYARI !!! \nİlanlar 30 saniye aralıklarla listelenmektedir!\nSahibinden.com site güvenliği için süre sınırı konulmuştur!");
            Sahibinden();
            Save();
        }
        private static void Sahibinden()
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument document = web.Load("https://www.sahibinden.com/");

            String detailLink = "";
            String sahibindenLink = "https://www.sahibinden.com/";

            for (int j = 1; j < 57; j++)
            {
                System.Threading.Thread.Sleep(30000);
                var link = document.DocumentNode.SelectNodes("//*[@id=\"container\"]/div[3]/div/div[3]/div[3]/ul/li[" + j + "]/a");
                int last = 0;
                link.ToList().ForEach(i => last = (i.OuterHtml.IndexOf(" title=")));

                if (last != -1)
                {
                    link.ToList().ForEach(i => detailLink = (i.OuterHtml.Substring(9, last - 10)));
                    Console.WriteLine(j + ". İlan");
                    Details(sahibindenLink + detailLink);
                }
                else
                {
                    continue;
                }
            }
        }
        static List<string> titles = new List<string>();
        static List<string> prices = new List<string>();
        private static void Details(String url)
        {
            try
            {
                HtmlWeb web = new HtmlWeb();
                HtmlDocument document = web.Load(url);

                var title = document.DocumentNode.SelectNodes("//*[@id=\"classifiedDetail\"]/div/div[1]/h1/text()").First().InnerText;
                var price = document.DocumentNode.SelectNodes("//*[@id=\"classifiedDetail\"]/div/div[2]/div[2]/h3/text()").First().InnerText;

                titles.Add(title);
                prices.Add(price.Trim());

                Console.WriteLine(title);
                Console.WriteLine(price.Trim());
            }
            catch (Exception)
            {
                Console.WriteLine("İlan Getirilemedi!");
                System.Threading.Thread.Sleep(10000);
            }
        }

        private static void Save()
        {
            StreamWriter file = new StreamWriter("C:/Users/Furkan/Desktop/adverts.txt");
            titles.ForEach(titles => file.WriteLine(titles));
            prices.ForEach(prices => file.WriteLine(prices));
            file.Close();
            Console.WriteLine("Dosya başarıyla kaydedildi!");
        }
    }
}
