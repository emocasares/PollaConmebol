﻿using System;
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

                Thread.Sleep(500);

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
                        var nodesEquipos = partido.SelectNodes(".//td[contains(@class, 'tns-c')]/div/div[contains(@class, 'liveresults-sports-immersive__hide-element')]");
                        string equipo1 = nodesEquipos[0].InnerText;
                        string equipo2 = nodesEquipos[1].InnerText;


                        var nodesBanderas = partido.SelectNodes(".//td[contains(@class, 'imspo_mt__lgc')]");
                        const string protocolo = "https";
                        string bandera1 = $"{protocolo}:{nodesBanderas[0].ChildNodes[0].Attributes["src"].Value}";
                        string bandera2 = $"{protocolo}:{nodesBanderas[1].ChildNodes[0].Attributes["src"].Value}";

                        var nodeGolesEquipos = partido.SelectNodes(".//td[contains(@class, 'tns-c')]/div/div[contains(@class, 'imspo_mt__tt-w')]"); 
                        string golesEquipo1 = nodeGolesEquipos !=null ? nodeGolesEquipos[0].InnerText:" X ";
                        string golesEquipo2 = nodeGolesEquipos != null ? nodeGolesEquipos[1].InnerText : " X ";

                        var nodeFechaPartidoJugado = partido.SelectSingleNode(".//div[@class='imspo_mt__cmd']//span[last()]");
                        var nodeFechaPartidoNoJugado = partido.SelectSingleNode(".//div[@class='imspo_mt__ns-pm-s']");
                        string fechaPartido = nodeFechaPartidoJugado != null ? nodeFechaPartidoJugado.InnerText : $"{nodeFechaPartidoNoJugado.ChildNodes[0].InnerText} HORA: {nodeFechaPartidoNoJugado.ChildNodes[1].InnerText}";

                        Console.WriteLine("Bandera 1: " + bandera1);
                        Console.WriteLine("Bandera 2: " + bandera2);
                        Console.WriteLine($"Marcador: {equipo1} {golesEquipo1} - {equipo2} {golesEquipo2} ");
                        Console.WriteLine("Fecha del Partido: " + fechaPartido);
                        Console.WriteLine();
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
