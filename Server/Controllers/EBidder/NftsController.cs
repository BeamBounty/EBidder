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
    [Route("odata/EBidder/Nfts")]
    public partial class NftsController : ODataController
    {
        private EBidderWeb.Server.Data.EBidderContext context;

        public NftsController(EBidderWeb.Server.Data.EBidderContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<EBidderWeb.Server.Models.EBidder.Nft> GetNfts()
        {
            var items = this.context.Nfts.AsQueryable<EBidderWeb.Server.Models.EBidder.Nft>();
            this.OnNftsRead(ref items);

            return items;
        }

        partial void OnNftsRead(ref IQueryable<EBidderWeb.Server.Models.EBidder.Nft> items);

        partial void OnNftGet(ref SingleResult<EBidderWeb.Server.Models.EBidder.Nft> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/EBidder/Nfts(NftID={NftID})")]
        public SingleResult<EBidderWeb.Server.Models.EBidder.Nft> GetNft(int key)
        {
            var items = this.context.Nfts.Where(i => i.NftID == key);
            var result = SingleResult.Create(items);

            OnNftGet(ref result);

            return result;
        }
        partial void OnNftDeleted(EBidderWeb.Server.Models.EBidder.Nft item);
        partial void OnAfterNftDeleted(EBidderWeb.Server.Models.EBidder.Nft item);

        [HttpDelete("/odata/EBidder/Nfts(NftID={NftID})")]
        public IActionResult DeleteNft(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.Nfts
                    .Where(i => i.NftID == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnNftDeleted(item);
                this.context.Nfts.Remove(item);
                this.context.SaveChanges();
                this.OnAfterNftDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnNftUpdated(EBidderWeb.Server.Models.EBidder.Nft item);
        partial void OnAfterNftUpdated(EBidderWeb.Server.Models.EBidder.Nft item);

        [HttpPut("/odata/EBidder/Nfts(NftID={NftID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutNft(int key, [FromBody]EBidderWeb.Server.Models.EBidder.Nft item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null || (item.NftID != key))
                {
                    return BadRequest();
                }
                this.OnNftUpdated(item);
                this.context.Nfts.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Nfts.Where(i => i.NftID == key);
                ;
                this.OnAfterNftUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/EBidder/Nfts(NftID={NftID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchNft(int key, [FromBody]Delta<EBidderWeb.Server.Models.EBidder.Nft> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.Nfts.Where(i => i.NftID == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnNftUpdated(item);
                this.context.Nfts.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Nfts.Where(i => i.NftID == key);
                ;
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnNftCreated(EBidderWeb.Server.Models.EBidder.Nft item);
        partial void OnAfterNftCreated(EBidderWeb.Server.Models.EBidder.Nft item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] EBidderWeb.Server.Models.EBidder.Nft item)
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

                this.OnNftCreated(item);
                this.context.Nfts.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Nfts.Where(i => i.NftID == item.NftID);

                ;

                this.OnAfterNftCreated(item);

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
