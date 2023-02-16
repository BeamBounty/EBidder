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
    [Route("odata/EBidder/Buyers")]
    public partial class BuyersController : ODataController
    {
        private EBidderWeb.Server.Data.EBidderContext context;

        public BuyersController(EBidderWeb.Server.Data.EBidderContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<EBidderWeb.Server.Models.EBidder.Buyer> GetBuyers()
        {
            var items = this.context.Buyers.AsQueryable<EBidderWeb.Server.Models.EBidder.Buyer>();
            this.OnBuyersRead(ref items);

            return items;
        }

        partial void OnBuyersRead(ref IQueryable<EBidderWeb.Server.Models.EBidder.Buyer> items);

        partial void OnBuyerGet(ref SingleResult<EBidderWeb.Server.Models.EBidder.Buyer> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/EBidder/Buyers(SSN={keySSN},BuyerID={keyBuyerID})")]
        public SingleResult<EBidderWeb.Server.Models.EBidder.Buyer> GetBuyer([FromODataUri] int keySSN, [FromODataUri] int keyBuyerID)
        {
            var items = this.context.Buyers.Where(i => i.SSN == keySSN && i.BuyerID == keyBuyerID);
            var result = SingleResult.Create(items);

            OnBuyerGet(ref result);

            return result;
        }
        partial void OnBuyerDeleted(EBidderWeb.Server.Models.EBidder.Buyer item);
        partial void OnAfterBuyerDeleted(EBidderWeb.Server.Models.EBidder.Buyer item);

        [HttpDelete("/odata/EBidder/Buyers(SSN={keySSN},BuyerID={keyBuyerID})")]
        public IActionResult DeleteBuyer([FromODataUri] int keySSN, [FromODataUri] int keyBuyerID)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.Buyers
                    .Where(i => i.SSN == keySSN && i.BuyerID == keyBuyerID)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnBuyerDeleted(item);
                this.context.Buyers.Remove(item);
                this.context.SaveChanges();
                this.OnAfterBuyerDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnBuyerUpdated(EBidderWeb.Server.Models.EBidder.Buyer item);
        partial void OnAfterBuyerUpdated(EBidderWeb.Server.Models.EBidder.Buyer item);

        [HttpPut("/odata/EBidder/Buyers(SSN={keySSN},BuyerID={keyBuyerID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutBuyer([FromODataUri] int keySSN, [FromODataUri] int keyBuyerID, [FromBody]EBidderWeb.Server.Models.EBidder.Buyer item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null || (item.SSN != keySSN && item.BuyerID != keyBuyerID))
                {
                    return BadRequest();
                }
                this.OnBuyerUpdated(item);
                this.context.Buyers.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Buyers.Where(i => i.SSN == keySSN && i.BuyerID == keyBuyerID);
                ;
                this.OnAfterBuyerUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/EBidder/Buyers(SSN={keySSN},BuyerID={keyBuyerID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchBuyer([FromODataUri] int keySSN, [FromODataUri] int keyBuyerID, [FromBody]Delta<EBidderWeb.Server.Models.EBidder.Buyer> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.Buyers.Where(i => i.SSN == keySSN && i.BuyerID == keyBuyerID).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnBuyerUpdated(item);
                this.context.Buyers.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Buyers.Where(i => i.SSN == keySSN && i.BuyerID == keyBuyerID);
                ;
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnBuyerCreated(EBidderWeb.Server.Models.EBidder.Buyer item);
        partial void OnAfterBuyerCreated(EBidderWeb.Server.Models.EBidder.Buyer item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] EBidderWeb.Server.Models.EBidder.Buyer item)
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

                this.OnBuyerCreated(item);
                this.context.Buyers.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Buyers.Where(i => i.SSN == item.SSN && i.BuyerID == item.BuyerID);

                ;

                this.OnAfterBuyerCreated(item);

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
