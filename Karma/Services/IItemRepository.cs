﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Karma.Models;
using Microsoft.AspNetCore.Http;

namespace Karma.Services
{
    public interface IItemRepository
    {
        public Task<List<ItemPost>> GetPosts();

        public Task<List<ItemPost>> SearchPosts(string searchTerm);

        public Task<ItemPost> GetPost(int id);
        public Task<ItemPost> AddPostAsync(ItemPost post, string userId);
        public ItemPost AddPost(ItemPost post, string userId);

        public Task<ItemPost> DeletePost(int id);

        public Task<ItemPost> UpdatePost(ItemPost newPost);
    }
}
