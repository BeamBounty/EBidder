using System;
using System.Net;
using System.Data;
using System.Linq;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Formatter;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace EBidderWeb.Server.Controllers.EBidder
{
    [Route("odata/EBidder/GameItems")]
    public partial class GameItemsController : ODataController
    {
        private EBidderWeb.Server.Data.EBidderContext context;

        public GameItemsController(EBidderWeb.Server.Data.EBidderContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<EBidderWeb.Server.Models.EBidder.GameItem> GetGameItems()
        {
            var items = this.context.GameItems.AsQueryable<EBidderWeb.Server.Models.EBidder.GameItem>();
            this.OnGameItemsRead(ref items);

            return items;
        }

        partial void OnGameItemsRead(ref IQueryable<EBidderWeb.Server.Models.EBidder.GameItem> items);

        partial void OnGameItemGet(ref SingleResult<EBidderWeb.Server.Models.EBidder.GameItem> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/EBidder/GameItems(GameID={GameID})")]
        public SingleResult<EBidderWeb.Server.Models.EBidder.GameItem> GetGameItem(int key)
        {
            var items = this.context.GameItems.Where(i => i.GameID == key);
            var result = SingleResult.Create(items);

            OnGameItemGet(ref result);

            return result;
        }
        partial void OnGameItemDeleted(EBidderWeb.Server.Models.EBidder.GameItem item);
        partial void OnAfterGameItemDeleted(EBidderWeb.Server.Models.EBidder.GameItem item);

        [HttpDelete("/odata/EBidder/GameItems(GameID={GameID})")]
        public IActionResult DeleteGameItem(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.GameItems
                    .Where(i => i.GameID == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnGameItemDeleted(item);
                this.context.GameItems.Remove(item);
                this.context.SaveChanges();
                this.OnAfterGameItemDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnGameItemUpdated(EBidderWeb.Server.Models.EBidder.GameItem item);
        partial void OnAfterGameItemUpdated(EBidderWeb.Server.Models.EBidder.GameItem item);

        [HttpPut("/odata/EBidder/GameItems(GameID={GameID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutGameItem(int key, [FromBody]EBidderWeb.Server.Models.EBidder.GameItem item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null || (item.GameID != key))
                {
                    return BadRequest();
                }
                this.OnGameItemUpdated(item);
                this.context.GameItems.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.GameItems.Where(i => i.GameID == key);
                ;
                this.OnAfterGameItemUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/EBidder/GameItems(GameID={GameID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchGameItem(int key, [FromBody]Delta<EBidderWeb.Server.Models.EBidder.GameItem> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.GameItems.Where(i => i.GameID == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnGameItemUpdated(item);
                this.context.GameItems.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.GameItems.Where(i => i.GameID == key);
                ;
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnGameItemCreated(EBidderWeb.Server.Models.EBidder.GameItem item);
        partial void OnAfterGameItemCreated(EBidderWeb.Server.Models.EBidder.GameItem item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] EBidderWeb.Server.Models.EBidder.GameItem item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null)
                {
                    return BadRequest();
                }

                this.OnGameItemCreated(item);
                this.context.GameItems.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.GameItems.Where(i => i.GameID == item.GameID);

                ;

                this.OnAfterGameItemCreated(item);

                return new ObjectResult(SingleResult.Create(itemToReturn))
                {
                    StatusCode = 201
                };
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }
    }
}
