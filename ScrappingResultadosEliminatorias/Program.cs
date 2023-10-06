using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using HtmlAgilityPack;
using System.Threading;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                driver.Navigate().GoToUrl("https://www.google.com/search?q=conmebol+qualifiers&sca_esv=571003301&sxsrf=AM9HkKmJlo2qrTJJS8CX89-I36q7iOVkrg%3A1696528098923&source=hp&ei=4vYeZci4NICmqtsP96CGkAM&iflsig=AO6bgOgAAAAAZR8E8vB-h30eVfxphbSWdMuwAEXLZfZt&gs_ssp=eJzj4tZP1zc0MjLLzbNMMWD0Ek7Oz8tNTcrPUSgsTczJTMtMLSoGALEfC1A&oq=conmebol+q&gs_lp=Egdnd3Mtd2l6Igpjb25tZWJvbCBxKgIIATIIEAAYywEYgAQyBRAuGIAEMgUQABiABDIFEAAYgAQyBRAAGIAEMgUQABiABDIFEAAYgAQyBRAAGIAEMgUQABiABDIFEAAYgARImjxQAFjpHXABeACQAQCYAYECoAGnD6oBBjAuMTAuMbgBA8gBAPgBAcICBBAjGCfCAgcQLhiKBRgnwgILEC4YgAQYxwEY0QPCAgcQIxiKBRgnwgIMECMYigUYExiABBgnwgIIEC4YywEYgATCAgoQLhjLARiABBgKwgIHEC4YgAQYCsICBxAAGIAEGAo&sclient=gws-wiz#sie=lg;/g/11t9_xctp4;2;/g/1226mn9d;mt;fp;1;;;");

                Thread.Sleep(5000);

                IWebElement visibleElement = driver.FindElement(By.XPath("//div[@id='liveresults-sports-immersive__updatable-league-matches']"));

                int scrollCount = 8;

                Actions actions = new(driver);

                actions.Click(visibleElement).Build().Perform();

                Thread.Sleep(700);

                for (int i = 0; i < scrollCount; i++)
                {
                    actions.SendKeys(Keys.PageDown).Perform();
                    Thread.Sleep(1000);
                }

                string html = driver.PageSource;

                driver.Quit();

                HtmlDocument doc = new();
                doc.LoadHtml(html);

                HtmlNodeCollection jornadas = doc.DocumentNode.SelectNodes("//div[@class='OcbAbf']");

                foreach (HtmlNode jornada in jornadas)
                {
                    string jornadaTitle = jornada.SelectSingleNode(".//div[@data-title]").InnerText;

                    HtmlNodeCollection partidos = jornada.SelectNodes(".//div[@jscontroller='ThULI']");

                    foreach (HtmlNode partido in partidos)
                    {
                        string equipo1 = partido.SelectSingleNode(".//div[@data-df-team-mid]:nth-child(2)").InnerText;
                        string equipo2 = partido.SelectSingleNode(".//div[@data-df-team-mid]:nth-child(3)").InnerText;

                        string golesEquipo1 = partido.SelectSingleNode(".//td[@class='imspo_mt__tt-w'][1]/div").InnerText;
                        string golesEquipo2 = partido.SelectSingleNode(".//td[@class='imspo_mt__tt-w'][2]/div").InnerText;

                        string fechaPartido = partido.SelectSingleNode(".//div[@class='imspo_mt__cmd']").InnerText;

                        Console.WriteLine("Jornada: " + jornadaTitle);
                        Console.WriteLine("Equipo 1: " + equipo1);
                        Console.WriteLine("Equipo 2: " + equipo2);
                        Console.WriteLine("Goles Equipo 1: " + golesEquipo1);
                        Console.WriteLine("Goles Equipo 2: " + golesEquipo2);
                        Console.WriteLine("Fecha del Partido: " + fechaPartido);
                        Console.WriteLine("-----------------------------------------");
                    }
                }
                
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }
}
