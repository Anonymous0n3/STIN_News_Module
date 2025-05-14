using Microsoft.AspNetCore.Mvc.RazorPages;
using STIN_News_Module.Logic;
using STIN_News_Module.Logic.JsonModel;
using System;
using System.Collections.Generic;

namespace STIN_News_Module.Pages
{
    public class UserDashboardModel : PageModel
    {
        public List<DataModel> Stocks { get; set; }
        private Utils utils = new Utils();

        public void OnGet()
        {
            // Zde můžeš napojit reálná data z API, databáze nebo souboru
            Stocks = new List<DataModel>
            {
                new DataModel { Name = "Microsoft", Date = 12345678, Rating = 8, Sale = 0 },
                new DataModel { Name = "Google", Date = 12345678, Rating = -3, Sale = 1 },
                new DataModel { Name = "OpenAI", Date = 12345678, Rating = 5, Sale = 0 },
            };
        }

        public void OnPostAdd()
        {
            // Zde můžeš např. přidat novou akcii (staticky nebo dynamicky)
            List<DataModel> stocks = utils.GetFromBurzaAsync().Result;
            Stocks = utils.doAllLogic(stocks, 7).Result;

        }
    }
}
