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

        public ActionResult results(string ABV, string IBU, string SRM)
        {
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

            //This bool is to quickly switch between live db and local data
            bool isdbDown = true;

            //gets results for each of out 14 craft brewries
            if (isdbDown)
            {
                for (int i = 0; i < 5; i++)
                {
                    breweries = LocalBrewery();
                }
            }
            else
            {
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

        //builds brewery objects using local json data
        public List<Brewery> LocalBrewery()
        {
            List<Brewery> localBrews = new List<Brewery>();

            string SchmozPath = @"C:\Users\GC Student\Source\Repos\Craft Beer Me\Craft Beer Me\Controllers\Schmoz.json";
            StreamReader rd = new StreamReader(SchmozPath);
            string beerData = rd.ReadToEnd();
            JObject SchmozJson = JObject.Parse(beerData);

            localBrews.Add(MakeABrewery(SchmozJson, 1));

            string JollyPath = @"C:\Users\GC Student\Source\Repos\Craft Beer Me\Craft Beer Me\Controllers\JollyPumpkin.json";
            StreamReader rd2 = new StreamReader(JollyPath);
            string JollyData = rd2.ReadToEnd();
            JObject JollyJson = JObject.Parse(JollyData);

            localBrews.Add(MakeABrewery(JollyJson, 2));

            string AtwaterPath =  @"C:\Users\GC Student\Source\Repos\Craft Beer Me\Craft Beer Me\Controllers\Atwater.json";
            StreamReader rd3 = new StreamReader(AtwaterPath);
            string AtwaterData = rd3.ReadToEnd();
            JObject AtwaterJson = JObject.Parse(AtwaterData);

            localBrews.Add(MakeABrewery(AtwaterJson, 3));

            string NewPath = @"C:\Users\GC Student\Source\Repos\Craft Beer Me\Craft Beer Me\Controllers\NewHolland.json";
            StreamReader rd4 = new StreamReader(NewPath);
            string NewData = rd4.ReadToEnd();
            JObject NewJson = JObject.Parse(NewData);

            localBrews.Add(MakeABrewery(NewJson, 4));

            return localBrews;
        }

        //makes each new brewery object from JSON
        public Brewery MakeABrewery(JObject beerJson, int x)
        {
            Brewery GrandCircus = new Brewery();

            
            //GrandCircus.BreweryID = beerJson[];

            switch (x)
            {
                case 1:
                    GrandCircus.Name = "Schmoz";
                    GrandCircus.Url = "http://www.schmoz.com";
                    GrandCircus.PictureUrl = "https://brewerydb-images.s3.amazonaws.com/brewery/AVEsqU/upload_uRmLOu-squareLarge.png";
                    break;
                case 2:
                    GrandCircus.Name = "Jolly Pumpkin";
                    GrandCircus.Url = "http://brewery.jollypumpkin.com/";
                    GrandCircus.PictureUrl = "https://brewerydb-images.s3.amazonaws.com/brewery/pzWq1r/upload_2YHJS9-squareLarge.png";
                    break;
                case 3:
                    GrandCircus.Name = "Atwater";
                    GrandCircus.Url = "https://www.atwaterbeer.com/";
                    GrandCircus.PictureUrl = "https://brewerydb-images.s3.amazonaws.com/brewery/boTIWO/upload_qHbhaE-squareLarge.png";
                    break;
                case 4:
                    GrandCircus.Name = "New Holland";
                    GrandCircus.Url = "http://newhollandbrew.com/";
                    GrandCircus.PictureUrl = "https://brewerydb-images.s3.amazonaws.com/brewery/AqEUBQ/upload_0xEGxj-squareLarge.png";
                    break;
                default:
                    break;
            }



            List<Beer> menu = new List<Beer>();

            Array beerArray = beerJson["data"].ToArray();

            //beerArray.Length
            for (int i = 0; i < beerArray.Length; i++)
            {
                //Evaluate here
                Beer newBeer = new Beer();
                newBeer = MakeAMenu(beerJson, i);
                menu.Add(newBeer);

            }

            //Beer newBeer = new Beer();
            //newBeer = MakeAMenu(beerJson);
            //menu.Add(newBeer);

            GrandCircus.Menu = menu;

            return GrandCircus;
        }

        //fills the menu with valid beers based on user parameters
        public Beer MakeAMenu(JObject beerJson, int x)
        {
            Beer craftBeer = new Beer();

            //Note: Not all JSON beers have the category 'Available' hmmm...

            
            craftBeer.BeerName = beerJson["data"][x]["name"].ToString();
            
            //Description
            if (beerJson["data"][x]["style"] != null && beerJson["data"][x]["style"]["description"] != null)
            {
                craftBeer.Description = beerJson["data"][x]["style"]["description"].ToString();
            }
            else
            {
                craftBeer.Description = null;
            }

            //ABV
            if (beerJson["data"][x]["style"] != null && beerJson["data"][x]["style"]["abvMin"] != null)
            {
                craftBeer.ABV = (double)beerJson["data"][x]["style"]["abvMin"];
            }
            else if (beerJson["data"][x]["style"] != null && beerJson["data"][x]["style"]["abvMax"] != null)
            {
                craftBeer.ABV = (double)beerJson["data"][x]["style"]["abvMax"];
            }
            else if (beerJson["data"][x]["style"] != null && beerJson["data"][x]["abv"] != null)
            {
                craftBeer.ABV = (double)beerJson["data"][x]["abv"];
            }
            else
            {
                craftBeer.ABV = 0;
            }
            
            //IBU
            if (beerJson["data"][x]["ibu"] != null)
            {
                craftBeer.IBU = (double)beerJson["data"][x]["ibu"];
            }
            else if (beerJson["data"][x]["style"] != null && beerJson["data"][x]["style"]["ibuMin"] != null)
            {
                craftBeer.IBU = (double)beerJson["data"][x]["style"]["ibuMin"];
            }
            else if (beerJson["data"][x]["style"] != null && beerJson["data"][x]["style"]["ibuMax"] != null)
            {
                craftBeer.IBU = (double)beerJson["data"][x]["style"]["ibuMax"];
            }
            else
            {
                craftBeer.IBU = 0;
            }
           
            //SRM
            if (beerJson["data"][x]["style"] != null && beerJson["data"][x]["style"]["srmMin"] != null)
            {
                craftBeer.SRM = (double)beerJson["data"][x]["style"]["srmMin"];
            }
            else
            {
                craftBeer.SRM = 0;
            }

            if (beerJson["data"][x]["style"] != null && beerJson["data"][x]["style"]["shortName"] != null)
            {
                craftBeer.CategoryName = beerJson["data"][x]["style"]["shortName"].ToString();
            }
            else
            {
                craftBeer.CategoryName = null;
            }
            

            if (beerJson["data"][x]["labels"] != null && beerJson["data"][x]["labels"]["medium"] != null)
            {
                craftBeer.Picture = beerJson["data"][x]["labels"]["medium"].ToString();
            }
            else
            {
                craftBeer.Picture = null;
            }
            

            return craftBeer;
        }
    }
}