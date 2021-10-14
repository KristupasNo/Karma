using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Karma.Models;
using Karma.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Text.RegularExpressions;

namespace Karma.Pages
{
    public class SubmitsModel : PageModel
    {

        [BindProperty]
        public ItemPost Item { get; set; }   
        
        [BindProperty]
        public IFormFile Photo { get; set; }

        private JsonFileItemService ItemService { get; }

        private IWebHostEnvironment WebHostEnvironment { get; }

        public IEnumerable<ItemPost> Submits { get; private set; }

        public SubmitsModel(
	    JsonFileItemService itemService,
            IWebHostEnvironment webHostEnvironment)
        {
            ItemService = itemService;
            WebHostEnvironment = webHostEnvironment;    
        }

        public void OnGet()
        {
            Submits = ItemService.GetPosts();
        }

        // Deletes Post on button trigger, refreshes posts afterwards : )
        public IActionResult OnPostDelete(string id)
        {
            ItemService.DeletePost(id);

            return RedirectToPage("/Submits");
        }
        // TO-DO implement filter by Category, get info from checkbox
        // public List<string> Categories = new List<string>(Post.SCategories);
        [BindProperty]
        public string SelectedCategory { get; set; }
        public IActionResult OnPostFilter(string SCategory)
        {
            //Submits = Submits.Where(x => x.Category == SelectedCategory);
            return RedirectToPage("/Submits");
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid == false)
            {
                return Page();
            }

            //if (!Regex.Match(, "^[A-Z][a-zA-Z]*$").Success) { RedirectToPage("/Accounts"); }

            ItemService.AddPost(Item, Photo);
                
            return RedirectToPage("/Submits");
        }
    }
}
