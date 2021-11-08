﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Karma.Models;
using Karma.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Karma.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        public ItemsController(IItemRepository itemService)
        {
            ItemService = itemService;
        }

        public IItemRepository ItemService { get; }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            try
            {
                return Ok(await ItemService.GetPosts());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ItemPost>> Get(int id)
        {
            try
            {
                var result = await ItemService.GetPost(id);

                if(result == null)
                {
                    return NotFound();
                }

                return result;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }

        [HttpPost]
        public async Task<ActionResult<ItemPost>> Post(ItemPost newItem, string userId)
        {
            try
            {
                if (newItem == null)
                    return BadRequest();

                var createdItem = await ItemService.AddPost(newItem, userId);

                if (createdItem == null)
                    return NotFound();

                return CreatedAtAction(nameof(Get), new { id = createdItem.Id }, createdItem);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }

        [HttpPut]
        public async Task<ActionResult<ItemPost>> Put(ItemPost newItem)
        {
            try
            {
                if (newItem == null)
                    return BadRequest();

                var updatedItem = await ItemService.UpdatePost(newItem);

                if (updatedItem == null)
                    return NotFound();

                return CreatedAtAction(nameof(Get), new { id = updatedItem.Id }, updatedItem);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }
    }
}
