using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;

namespace STIN_News_Module.Pages
{
    public class UserDashboardModel : PageModel
    {
        public List<StockItem> Stocks { get; set; }

        public void OnGet()
        {
            // Zde můžeš napojit reálná data z API, databáze nebo souboru
            Stocks = new List<StockItem>
            {
                new StockItem { Name = "Microsoft", Date = DateTime.Now.AddDays(-1), Rating = 8, Sell = 0 },
                new StockItem { Name = "Google", Date = DateTime.Now.AddDays(-2), Rating = -3, Sell = 1 },
                new StockItem { Name = "OpenAI", Date = DateTime.Now, Rating = 5, Sell = 0 },
            };
        }

        public void OnPostAdd()
        {
            // Zde můžeš např. přidat novou akcii (staticky nebo dynamicky)
            Stocks = new List<StockItem>
            {
                new StockItem { Name = "Microsoft", Date = DateTime.Now.AddDays(-1), Rating = 8, Sell = 0 },
                new StockItem { Name = "Google", Date = DateTime.Now.AddDays(-2), Rating = -3, Sell = 1 },
                new StockItem { Name = "OpenAI", Date = DateTime.Now, Rating = 5, Sell = 0 },
                new StockItem { Name = "Nová akcie", Date = DateTime.Now, Rating = 1, Sell = 0 }
            };
        }


        public class StockItem
        {
            public string Name { get; set; }
            public DateTime Date { get; set; }
            public int Rating { get; set; }
            public int Sell { get; set; }
        }
    }
}
