using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Karma.Areas.Identity.Data;
using Karma.Data;
using Karma.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Karma.Services
{

    public class SqlItemRepository : IItemRepository
    {
        public KarmaDbContext Context { get; }

        public SqlItemRepository(KarmaDbContext context)
        {
            Context = context;
        }

        public async Task<ItemPost> AddPostAsync(ItemPost post, string userId)
        {
            post.Date = DateTime.Now;
            post.State = Post.StateEnum.Recent;
            post.KarmaUser = await Context.Users.FindAsync(userId);
            if (post.KarmaUser == null)
                return null;
            await Context.Items.AddAsync(post);
            await Context.SaveChangesAsync();
            return post;
        }
        public ItemPost AddPost(ItemPost post, string userId)
        {
            post.Date = DateTime.Now;
            post.State = Post.StateEnum.Recent;
            post.KarmaUser = Context.Users.Find(userId);
            if (post.KarmaUser == null)
                return null;
            Context.Items.Add(post);
            Context.SaveChanges();
            return post;
        }

        public async Task<ItemPost> DeletePost(int id)
        {
            ItemPost item = await Context.Items.FindAsync(id);
            if(item != null)
            {
                Context.Items.Remove(item);
                await Context.SaveChangesAsync();
            }
            return item;
        }

        public async Task<ItemPost> GetPost(int id)
        {
            return await Context.Items.Include(i => i.KarmaUser).FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<List<ItemPost>> GetPosts()
        {
            return await Context.Items.Include(i => i.KarmaUser).ToListAsync();
        }

        public async Task<List<ItemPost>> SearchPosts(string searchTerm)
        {
            if (searchTerm == null)
                return await GetPosts();

            return  await Context.Items.Include(i => i.KarmaUser).Where(item => item.Title.Contains(searchTerm)).ToListAsync();
        }

        public async Task<ItemPost> UpdatePost(ItemPost newPost)
        {
            ItemPost post = await Context.Items.FirstOrDefaultAsync(post => post.Id == newPost.Id);
            if(post == null)
            {
                return null;
            }
            if(newPost.Picture != null) post.Picture = newPost.Picture;
            post.Title = newPost.Title;
            post.Description = newPost.Description;
            post.Category = newPost.Category;
            await Context.SaveChangesAsync();
            return newPost;
        }
    }
}
