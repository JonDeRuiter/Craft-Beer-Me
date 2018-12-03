using Craft_Beer_Me.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace Craft_Beer_Me.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Recommended(double ABV, double IBU, double SRM)
        {
            List<Brewery> breweries = GetBreweries(ABV, IBU, SRM);
            
            ViewBag.Breweries = breweries;
            return View();
            
        }

        public List<Brewery> GetBreweries(double ABV, double IBU, double SRM)
        {
            List<Brewery> breweries = new List<Brewery>();
            
            //gets results for each of out 14 craft brewries
            for (int i = 1; i < 15; i++)
            {
                //string urlString = "https://sandbox-api.brewerydb.com/v2/" + "brewery/" + BreweryId(i) +  "/beers?key=5049b9309015a193f513d52c4d9c0003";

                //test url
                string urlString = "https://sandbox-api.brewerydb.com/v2/" + "brewery/" + "AqEUBQ" + "/beers?key=5049b9309015a193f513d52c4d9c0003";

                HttpWebRequest request = WebRequest.CreateHttp(urlString);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader rd = new StreamReader(response.GetResponseStream());
                string beerData = rd.ReadToEnd();

                JObject beerJson = JObject.Parse(beerData);
                //Valid beers get added



                //api limits to 10 requests a second, this *should* solve that
                Thread.Sleep(150);

            }
            return breweries;
        }
        
        //designed to be used by a for loop to return each of our 14 breweries
        public string BreweryId(int id)
        {
            switch (id)
            {
                case 1:
                    //founders
                    return "Idm5Y5";
                    break;
                case 2:
                    //hopcat
                    return "HizvxH";
                    break;
                case 3:
                    //jolly pumpkin
                    return "pzWq1r";
                    break;
                case 4:
                    //the mitten
                    return "bdFoir";
                    break;
                case 5:
                    //harmony
                    return "P0oEwB";
                    break;
                case 6:
                    //elk brewing
                    return "sjblac";
                    break;
                case 7:
                    //perrin
                    return "Boa6td";
                    break;
                case 8:
                    //rockford brewing
                    return "U92Ctx";
                    break;
                case 9:
                    //brewery vivant
                    return "LFkVMc";
                    break;
                case 10:
                    //peoples cider
                    return "iebYze";
                    break;
                case 11:
                    //Schmohz
                    return "AVEsqU";
                    break;
                case 12:
                    //hideout
                    return "35YJeP";
                    break;
                case 13:
                    //Atwater
                    return "boTIWO";
                    break;
                case 14:
                    //new holland
                    return "AqEUBQ";
                    break;
                default:
                    break;
            }
            return null;
        }

        //makes each new brewery object from JSON
        public Brewery MakeABrewery(JObject beerJson)
        {
            Brewery GrandCircus = new Brewery();

            GrandCircus.Name = beerJson[].ToString();
            GrandCircus.Url = beerJson[];
            GrandCircus.PictureUrl = beerJson[];
            GrandCircus.BreweryID = beerJson[];


            List<Beer> menu = new List<Beer>();
            //foreach (var item in collection)
            //{
            //    Beer newBeer = new Beer();
            //    newBeer = MakeAMenu(beerJson);
            //    menu.Add(newBeer);

            //}

            Beer newBeer = new Beer();
            newBeer = MakeAMenu(beerJson);
            menu.Add(newBeer);

            GrandCircus.Menu = menu;
            return GrandCircus;
        }

        //fills the menu with valid beers based on user parameters
        public Beer MakeAMenu(JObject beerJson)
        {
            Beer craftBeer = new Beer();

            craftBeer.BeerName = beerJson["data"][0]["name"].ToString();
            craftBeer.Description = beerJson["data"][0]["style"]["description"].ToString();
            craftBeer.ABV = (double)beerJson["data"][0]["abv"];
            craftBeer.IBU = (double)beerJson["data"][0]["ibu"];
            craftBeer.SRM = (double)beerJson["data"][0]["style"]["srmMin"];
            craftBeer.CategoryName = beerJson["data"][0]["style"]["shortName"].ToString();

            return craftBeer;
        }
    }
}