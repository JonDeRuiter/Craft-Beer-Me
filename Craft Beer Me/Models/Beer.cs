﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Craft_Beer_Me.Models
{
    public class Beer
    {
        public int ID { get; set; }
        public string BeerName {get; set;}
        public string Picture {get; set;}
        public string Description {get; set;}

        public string CategoryName {get; set;}
        public double ABV {get; set;}
        public double IBU {get; set;}
        public double SRM  {get; set;}

        public string Food_Pair {get; set;}
        public bool Available {get; set;}
           

        public Beer()
        {

        }

    }

    public class DBBeer : DbContext
    {
        public DbSet<Beer> Beers { get; set; }
    }

}