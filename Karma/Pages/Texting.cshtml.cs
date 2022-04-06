using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Karma.Models;
using Karma.Services;
using Microsoft.AspNetCore.Hosting;
using Shared.Web.MvcExtensions;
using Karma.Data;

namespace Karma.Pages
{
    public class TextingModel : PageModel
    {
        
        public List<Message> Inbox { get; set; }
        public List<Message> Outbox { get; set; }
        public HttpClient HttpClient { get; }
        [BindProperty]
        public Message Message { get; set; }
        [BindProperty]
        public ItemPost Item { get; set; }

        public RequestPost Messages { get; set; }
        public IMessageRepository MessageService { get; }
        private string baseAddress = KarmaHttpContext.AppBaseUrl + "/api/";
        public IEnumerable<ItemPost> Submits { get; set; }

        private IWebHostEnvironment WebHostEnvironment { get; }
        public KarmaPointService KarmaPointService { get; }
        public KarmaDbContext Context { get; }

        public TextingModel(HttpClient client, IWebHostEnvironment webHostEnvironment, IMessageRepository messageService, 
            KarmaPointService karmaPointService, KarmaDbContext context)
        {
            HttpClient = client;
            WebHostEnvironment = webHostEnvironment;
            MessageService = messageService;
            KarmaPointService = karmaPointService;
            Context = context;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var email = User.Identity.Name;

            Inbox = await HttpClient.GetFromJsonAsync<List<Message>>($"https://localhost:44332/api/messages/to/{email}",
                new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });

            foreach (var v in Inbox.ToList())
            {
                if(v.FromEmail == "karmanotifier@karma.com")
                {
                    Inbox.Remove(v);
                }
            }

            Outbox = await HttpClient.GetFromJsonAsync<List<Message>>($"https://localhost:44332/api/messages/from/{email}",
                new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });

            return Page();
        }

        public async Task<IActionResult> OnPostMessageAsync(int itemId)
        {
            Message = await RequestMessageGetAsync(itemId);
            Message.FromEmail = User.Identity.Name;
            Message.ToEmail = Item.KarmaUser.Email;
            Message.Date = DateTime.Now;
            await HttpClient.PostAsJsonAsync<Message>($"{baseAddress}messages", Message);


            /*Message = await RequestService.GetPost(itemId);
            if (User.Identity != null) Message.FromEmail = User.Identity.Name;
            Message.ToEmail = Item.KarmaUser.Email;
            Message.Date = DateTime.Now;
            await MessageService.AddMessage(Message);*/

            return RedirectToPage("/Texting");
        }

        private async Task<Message> RequestMessageGetAsync(int id)
        {
            return Message = await HttpClient.GetFromJsonAsync<Message>($"{baseAddress}messages/{id}",
                new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });

        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            /*var userId = User.GetUserId();
            Func<int, int> Subtract = (x => x - 6);
            await KarmaPointService.ProcessKarmaBalanceAsync(userId, Subtract);

            var item = await MessageService.GetMessages();*/
            await MessageService.DeleteMsg(id);

            return RedirectToPage("/Texting");
        }
    }
}
