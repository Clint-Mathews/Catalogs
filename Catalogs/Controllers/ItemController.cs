using Catalogs.Dtos;
using Catalogs.Entities;
using Catalogs.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Catalogs.Controllers
{
    [ApiController]
    // [Route("[controller]")]
    [Route("items")]
    public class ItemController : ControllerBase
    {
        private readonly IInMemItemsRepository repository;
        private readonly ILogger<ItemController> logger;
        public ItemController(IInMemItemsRepository repository, ILogger<ItemController> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        // GET /items
        [HttpGet]
        public async Task<IEnumerable<ItemDto>> GetItemsAsync()
        {
            var items = (await repository.GetItemsAsync())
                        .Select(item => item.AsItemDto());
            logger.LogInformation($"{DateTime.UtcNow.ToString("hh:mm:ss")} : Retrived {items.Count()} items");
            return items;
        }

        // GET /items/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto?>> GetItemAsync(Guid id)
        {
            var item = await repository.GetItemAsync(id);

            if(item is null)
            {
                return NotFound();
            }
            return item.AsItemDto();
        }

        // POST /items
        [HttpPost]
        public async Task<ActionResult<ItemDto>> CreateItemAsync(CreateItemDto createItem)
        {
            Item item = new()
            {
                Id = Guid.NewGuid(),
                Name = createItem.Name,
                Price = createItem.Price,
                CreatedDate = DateTime.UtcNow,
            };
            await repository.CreateItemAsync(item);
            return CreatedAtAction(nameof(GetItemAsync), new { id = item.Id }, item.AsItemDto());
        }

        // PUT /items/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateItemAsync(Guid id, UpdateItemDto item)
        {
            var existingItem = await repository.GetItemAsync(id);
            if(existingItem is null)
            {
                return NotFound();
            }
            Item updatedItem = existingItem with
            {
                Name =item.Name,
                Price= item.Price
            };
            await repository.UpdateItemAsync(updatedItem);
            return NoContent();
        }

        // DELETE /items/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteItemAsync(Guid id)
        {
            var existingItem = await repository.GetItemAsync(id);
            if(existingItem is null)
            {
                return NotFound();
            }
            await repository.DeleteItemAsync(id);
            return NoContent();
        }

    }
}
