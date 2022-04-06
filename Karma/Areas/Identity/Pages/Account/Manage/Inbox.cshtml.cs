using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Karma.Models;
using Karma.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Karma.Areas.Identity.Pages.Account
{
    public class InboxModel : PageModel
    {
        public InboxModel(HttpClient client)
        {
            HttpClient = client;
        }
        public List<Message> Inbox { get; set; }
        public HttpClient HttpClient { get; }

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
                if (v.FromEmail != "karmanotifier@karma.com")
                {
                    Inbox.Remove(v);
                }
            }

            return Page();
        }
    }
}
