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
    [Route("odata/EBidder/Artworks")]
    public partial class ArtworksController : ODataController
    {
        private EBidderWeb.Server.Data.EBidderContext context;

        public ArtworksController(EBidderWeb.Server.Data.EBidderContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<EBidderWeb.Server.Models.EBidder.Artwork> GetArtworks()
        {
            var items = this.context.Artworks.AsQueryable<EBidderWeb.Server.Models.EBidder.Artwork>();
            this.OnArtworksRead(ref items);

            return items;
        }

        partial void OnArtworksRead(ref IQueryable<EBidderWeb.Server.Models.EBidder.Artwork> items);

        partial void OnArtworkGet(ref SingleResult<EBidderWeb.Server.Models.EBidder.Artwork> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/EBidder/Artworks(ArtID={ArtID})")]
        public SingleResult<EBidderWeb.Server.Models.EBidder.Artwork> GetArtwork(int key)
        {
            var items = this.context.Artworks.Where(i => i.ArtID == key);
            var result = SingleResult.Create(items);

            OnArtworkGet(ref result);

            return result;
        }
        partial void OnArtworkDeleted(EBidderWeb.Server.Models.EBidder.Artwork item);
        partial void OnAfterArtworkDeleted(EBidderWeb.Server.Models.EBidder.Artwork item);

        [HttpDelete("/odata/EBidder/Artworks(ArtID={ArtID})")]
        public IActionResult DeleteArtwork(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.Artworks
                    .Where(i => i.ArtID == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnArtworkDeleted(item);
                this.context.Artworks.Remove(item);
                this.context.SaveChanges();
                this.OnAfterArtworkDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnArtworkUpdated(EBidderWeb.Server.Models.EBidder.Artwork item);
        partial void OnAfterArtworkUpdated(EBidderWeb.Server.Models.EBidder.Artwork item);

        [HttpPut("/odata/EBidder/Artworks(ArtID={ArtID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutArtwork(int key, [FromBody]EBidderWeb.Server.Models.EBidder.Artwork item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null || (item.ArtID != key))
                {
                    return BadRequest();
                }
                this.OnArtworkUpdated(item);
                this.context.Artworks.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Artworks.Where(i => i.ArtID == key);
                ;
                this.OnAfterArtworkUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/EBidder/Artworks(ArtID={ArtID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchArtwork(int key, [FromBody]Delta<EBidderWeb.Server.Models.EBidder.Artwork> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.Artworks.Where(i => i.ArtID == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnArtworkUpdated(item);
                this.context.Artworks.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Artworks.Where(i => i.ArtID == key);
                ;
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnArtworkCreated(EBidderWeb.Server.Models.EBidder.Artwork item);
        partial void OnAfterArtworkCreated(EBidderWeb.Server.Models.EBidder.Artwork item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] EBidderWeb.Server.Models.EBidder.Artwork item)
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

                this.OnArtworkCreated(item);
                this.context.Artworks.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Artworks.Where(i => i.ArtID == item.ArtID);

                ;

                this.OnAfterArtworkCreated(item);

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
