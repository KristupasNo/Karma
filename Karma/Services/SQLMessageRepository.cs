﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Karma.Data;
using Karma.Models;
using Microsoft.EntityFrameworkCore;

namespace Karma.Services
{
    public class SQLMessageRepository : IMessageRepository
    {

        public SQLMessageRepository(KarmaDbContext context)
        {
            Context = context;
        }

        public KarmaDbContext Context { get; }

        public async Task<Message> AddMessage(Message message)
        {
            await Context.Messages.AddAsync(message);
            await Context.SaveChangesAsync();
            return message;
        }

        public async Task<List<Message>> GetMessages()
        {
            return await Context.Messages.ToListAsync();
        }

        public async Task<List<Message>> GetMessagesByEmail(string email)
        {
            return await Context.Messages.Where(m => m.ToEmail == email).ToListAsync();
        }
    }
}
