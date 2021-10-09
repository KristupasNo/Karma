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

namespace Karma.Pages
{
    public class SubmitsModel : PageModel
    {

        [BindProperty]
        public ItemPost Item { get; set; }   
        
        [BindProperty]
        public IFormFile Photo { get; set; }

        public JsonFileItemService ItemService;

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


        public IActionResult OnPost()
        {
            if (ModelState.IsValid == false)
            {
                return Page();
            }

            ItemService.AddPost(Item, Photo);
                
                
            return RedirectToPage("/Submits");
        }

        //Uploads the parsed pic into ./wwwroot/images/ 
        //Returns uniqueFileName string - a random ID + file name

    }
}
