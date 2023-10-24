using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using HtmlAgilityPack;
using System.Threading;
using PollaEngendrilClientHosted.Shared.Models.Entity;
using System.Text.RegularExpressions;
using ScrappingResultadosEliminatorias.Services;
using PollaEngendrilClientHosted.Server.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using Humanizer.Configuration;


internal class Program
{
    public static IConfiguration Configuration { get; set; }

    private static async Task Main(string[] args)
    {
        try
        {

            var builder = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json");

            Configuration = builder.Build();


            // Read the connection string from the configuration
            string connectionString = Configuration.GetConnectionString("DefaultConnection");

            var serviceProvider = new ServiceCollection()
           .AddDbContext<ApplicationDbContext>(options =>
           {
               options.UseSqlServer(connectionString); 
           })
           .BuildServiceProvider();

            using (var context = serviceProvider.GetRequiredService<ApplicationDbContext>())


            using (IWebDriver driver = new ChromeDriver())
            {
                driver.Navigate().GoToUrl("https://www.google.com/search?q=conmebol+qualifiers&sca_esv=571003301&sxsrf=AM9HkKmJlo2qrTJJS8CX89-I36q7iOVkrg%3A1696528098923&source=hp&ei=4vYeZci4NICmqtsP96CGkAM&iflsig=AO6bgOgAAAAAZR8E8vB-h30eVfxphbSWdMuwAEXLZfZt&gs_ssp=eJzj4tZP1zc0MjLLzbNMMWD0Ek7Oz8tNTcrPUSgsTczJTMtMLSoGALEfC1A&oq=conmebol+q&gs_lp=Egdnd3Mtd2l6Igpjb25tZWJvbCBxKgIIATIIEAAYywEYgAQyBRAuGIAEMgUQABiABDIFEAAYgAQyBRAAGIAEMgUQABiABDIFEAAYgAQyBRAAGIAEMgUQABiABDIFEAAYgARImjxQAFjpHXABeACQAQCYAYECoAGnD6oBBjAuMTAuMbgBA8gBAPgBAcICBBAjGCfCAgcQLhiKBRgnwgILEC4YgAQYxwEY0QPCAgcQIxiKBRgnwgIMECMYigUYExiABBgnwgIIEC4YywEYgATCAgoQLhjLARiABBgKwgIHEC4YgAQYCsICBxAAGIAEGAo&sclient=gws-wiz#sie=lg;/g/11t9_xctp4;2;/g/1226mn9d;mt;fp;1;;;");

                Thread.Sleep(2400);

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

                var matchService = new MatchService(context);


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
                        string golesEquipo1 = nodeGolesEquipos != null ? nodeGolesEquipos[0].InnerText : " X ";
                        string golesEquipo2 = nodeGolesEquipos != null ? nodeGolesEquipos[1].InnerText : " X ";

                        var nodeFechaPartidoJugado = partido.SelectSingleNode(".//div[@class='imspo_mt__cmd']//span[last()]");
                        var nodeFechaPartidoNoJugado = partido.SelectSingleNode(".//div[@class='imspo_mt__ns-pm-s']");

                        var timeOnly = TimeOnly.MaxValue;

                        var timeString = nodeFechaPartidoNoJugado!=null?nodeFechaPartidoNoJugado.ChildNodes[1].InnerText:"12:00";

                        // Define a regular expression pattern for "hh:mm" format
                        var timePattern = @"(\d{1,2}:\d{2})";

                        // Use Regex to match and extract the time
                        var matchTime = Regex.Match(timeString, timePattern);

                        if (matchTime.Success)
                        {
                            string time = matchTime.Groups[1].Value;
                            if (DateTime.TryParseExact(time, "HH:mm", null, System.Globalization.DateTimeStyles.None, out DateTime parsedTime))
                            {
                                timeOnly = new TimeOnly(parsedTime.Hour, parsedTime.Minute);
                            }
                        }

                        string fechaPartido = nodeFechaPartidoJugado != null ? nodeFechaPartidoJugado.InnerText : $"{nodeFechaPartidoNoJugado.ChildNodes[0].InnerText} HORA: {timeOnly}";

                        Console.WriteLine("Bandera 1: " + bandera1);
                        Console.WriteLine("Bandera 2: " + bandera2);
                        Console.WriteLine($"Marcador: {equipo1} {golesEquipo1} - {equipo2} {golesEquipo2} ");
                        Console.WriteLine("Fecha del Partido: " + fechaPartido);
                        Console.WriteLine();
                        var match = new PollaEngendrilClientHosted.Shared.Models.Entity.Match { HomeTeam = equipo1, AwayTeam = equipo2, HomeTeamFlag = bandera1, AwayTeamFlag = bandera2 };
                        if (!golesEquipo1.Equals(" X "))
                            match.HomeTeamScore = int.Parse(golesEquipo1);
                        if (!golesEquipo2.Equals(" X "))
                            match.AwayTeamScore = int.Parse(golesEquipo2);
                        var fechaExactaPartido = DateTime.Now;

                        if (nodeFechaPartidoJugado != null)
                        {
                            string patternPartidoJugado = @"^(\d{1,2}/\d{1,2})$";
                            var fechaPartidoJugado = nodeFechaPartidoJugado.InnerText;
                            if (fechaPartidoJugado.Contains(","))
                            {
                                fechaPartidoJugado = fechaPartidoJugado.Split(',')[1].Trim();
                            }
                            var regexMatch = Regex.Match(fechaPartidoJugado, patternPartidoJugado);
                            if (regexMatch.Success)
                            {
                                string fechaCompleta = regexMatch.Groups[1].Value;
                                string[] partes = fechaCompleta.Split('/');

                                if (partes.Length == 2)
                                {
                                    int dia = int.Parse(partes[0]);
                                    int mes = int.Parse(partes[1]);
                                    int anio = 2023;
                                    fechaExactaPartido = new DateTime(anio, mes, dia);
                                }
                            }
                        }
                        else if (nodeFechaPartidoNoJugado != null)
                        {
                            string patternCurrentyear = @"^\w{3}, \d{2}/\d{2}$";
                            var regexMatch = Regex.Match(nodeFechaPartidoNoJugado.ChildNodes[0].InnerText, patternCurrentyear);
                            if (regexMatch.Success)
                            {
                                string fechaCompleta = regexMatch.Groups[1].Value;
                                string[] partes = fechaCompleta.Split('/');

                                if (partes.Length == 2)
                                {
                                    int dia = int.Parse(partes[0]);
                                    int mes = int.Parse(partes[1]);
                                    int anio = 2023;
                                    fechaExactaPartido = new DateTime(anio, mes, dia);
                                }
                            }
                            else
                            {
                                string patternNextYear = @"^(\d{1,2}/\d{1,2}/\d{2})$";
                                var matchNextYear = Regex.Match(nodeFechaPartidoNoJugado.ChildNodes[0].InnerText, patternNextYear);
                                if (matchNextYear.Success)
                                {
                                    string fechaCompleta = matchNextYear.Groups[1].Value;
                                    string[] partes = fechaCompleta.Split('/');

                                    if (partes.Length == 3)
                                    {
                                        int dia = int.Parse(partes[0]);
                                        int mes = int.Parse(partes[1]);
                                        int anho = int.Parse(partes[2]) + 2000;
                                        fechaExactaPartido = new DateTime(anho, mes, dia);
                                    }
                                }
                                else
                                {
                                    string patternPartidoJugado = @"^(\d{1,2}/\d{1,2})$";
                                    var nodeValue = nodeFechaPartidoNoJugado.ChildNodes[0].InnerText;
                                    if (nodeValue.Contains(","))
                                    {
                                        nodeValue = nodeValue.Split(',')[1].Trim();
                                    }
                                    regexMatch = Regex.Match(nodeValue, patternPartidoJugado);
                                    if (regexMatch.Success)
                                    {
                                        string fechaCompleta = regexMatch.Groups[1].Value;
                                        string[] partes = fechaCompleta.Split('/');

                                        if (partes.Length == 2)
                                        {
                                            int dia = int.Parse(partes[0]);
                                            int mes = int.Parse(partes[1]);
                                            int anio = 2023;
                                            fechaExactaPartido = new DateTime(anio, mes, dia);
                                        }
                                    }
                                }

                            }
                        }
                        match.Date = fechaExactaPartido;
                        if (!timeOnly.Equals(TimeOnly.MaxValue))
                        {
                            match.Date = match.Date.AddMinutes(timeOnly.Minute);
                            match.Date = match.Date.AddHours(timeOnly.Hour);
                        }
                        Console.WriteLine(match.Date.ToLongDateString());
                        await matchService.InsertOrUpdateMatchAsync(match);
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