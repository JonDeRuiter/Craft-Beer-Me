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
        private DBBeer db = new DBBeer();

        public ActionResult Index()
        {
            
            //Beer testBeer = db.Beers.Last();
            //if (testBeer == null)
            //{
            //    GetBeer();


            //    //Session["FirstView"] = true;
            //}
            //else
            //{
            //    //draw from the db
            //}
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

        public ActionResult Recommended(string ABV, string IBU, string SRM)
        {
            //List<Beer> sixPack = GetBeer(ABV, IBU, SRM);
            
            //ViewBag.SixPack = sixPack;
            return View();
            
        }

        public List<Beer> GetBeer()
        {
            List<Beer> sixPack = new List<Beer>();

            //This bool is to quickly switch between live db and local data
            bool isdbDown = false;

            //gets results for each of our 23 pages of booze
            if (isdbDown)
            {
               
                    //sixPack = LocalBrewery();
                    string urlString = "https://api.brewerydb.com/v2/beers?key=fab6e885ba69e791a22d0143d832e493&p=" + "1";

                    HttpWebRequest request = WebRequest.CreateHttp(urlString);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    StreamReader rd = new StreamReader(response.GetResponseStream());
                    string beerData = rd.ReadToEnd();

                    JObject beerJson = JObject.Parse(beerData);
                sixPack = MakeABeerList(beerJson);


            }
            else
            {

                for (int i = 1; i < 24; i++)
                {
                    
                    string urlString = "https://api.brewerydb.com/v2/beers?key=fab6e885ba69e791a22d0143d832e493&p=" + i;

                    HttpWebRequest request = WebRequest.CreateHttp(urlString);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    StreamReader rd = new StreamReader(response.GetResponseStream());
                    string beerData = rd.ReadToEnd();

                    JObject beerJson = JObject.Parse(beerData);

                    sixPack = MakeABeerList(beerJson);
                    //api limits to 10 requests a second, this *should* solve that
                    Thread.Sleep(150);
                }
            }
            
            return sixPack;
        }
        

        //Searches the Beer List
        public List<Beer> SearchedList(JObject beerJson)
        {
            List<Beer> firstPass = new List<Beer>();
            
            //this line should do some searching
            //
            

            return firstPass;
        }

        //makes each new brewery object from each JSON page
        public List<Beer> MakeABeerList(JObject beerJson)
        {
                        
            List<Beer> menu = new List<Beer>();

            Array beerArray = beerJson["data"].ToArray();

            //beerArray.Length
            for (int i = 0; i < beerArray.Length; i++)
            {
                //Evaluate here
                Beer newBeer = new Beer();
                newBeer = MakeABeer(beerJson, i);
                menu.Add(newBeer);

            }
            
            return menu;
        }

        //fills the menu with valid beers based on user parameters
        public Beer MakeABeer(JObject beerJson, int x)
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

                       
            db.Beers.Add(craftBeer);
            db.SaveChanges();
            

            return craftBeer;
        }
    }
}