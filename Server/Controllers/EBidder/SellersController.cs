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
    [Route("odata/EBidder/Sellers")]
    public partial class SellersController : ODataController
    {
        private EBidderWeb.Server.Data.EBidderContext context;

        public SellersController(EBidderWeb.Server.Data.EBidderContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<EBidderWeb.Server.Models.EBidder.Seller> GetSellers()
        {
            var items = this.context.Sellers.AsQueryable<EBidderWeb.Server.Models.EBidder.Seller>();
            this.OnSellersRead(ref items);

            return items;
        }

        partial void OnSellersRead(ref IQueryable<EBidderWeb.Server.Models.EBidder.Seller> items);

        partial void OnSellerGet(ref SingleResult<EBidderWeb.Server.Models.EBidder.Seller> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/EBidder/Sellers(SSN={keySSN},SellerID={keySellerID})")]
        public SingleResult<EBidderWeb.Server.Models.EBidder.Seller> GetSeller([FromODataUri] int keySSN, [FromODataUri] int keySellerID)
        {
            var items = this.context.Sellers.Where(i => i.SSN == keySSN && i.SellerID == keySellerID);
            var result = SingleResult.Create(items);

            OnSellerGet(ref result);

            return result;
        }
        partial void OnSellerDeleted(EBidderWeb.Server.Models.EBidder.Seller item);
        partial void OnAfterSellerDeleted(EBidderWeb.Server.Models.EBidder.Seller item);

        [HttpDelete("/odata/EBidder/Sellers(SSN={keySSN},SellerID={keySellerID})")]
        public IActionResult DeleteSeller([FromODataUri] int keySSN, [FromODataUri] int keySellerID)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.Sellers
                    .Where(i => i.SSN == keySSN && i.SellerID == keySellerID)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnSellerDeleted(item);
                this.context.Sellers.Remove(item);
                this.context.SaveChanges();
                this.OnAfterSellerDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnSellerUpdated(EBidderWeb.Server.Models.EBidder.Seller item);
        partial void OnAfterSellerUpdated(EBidderWeb.Server.Models.EBidder.Seller item);

        [HttpPut("/odata/EBidder/Sellers(SSN={keySSN},SellerID={keySellerID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutSeller([FromODataUri] int keySSN, [FromODataUri] int keySellerID, [FromBody]EBidderWeb.Server.Models.EBidder.Seller item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null || (item.SSN != keySSN && item.SellerID != keySellerID))
                {
                    return BadRequest();
                }
                this.OnSellerUpdated(item);
                this.context.Sellers.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Sellers.Where(i => i.SSN == keySSN && i.SellerID == keySellerID);
                ;
                this.OnAfterSellerUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/EBidder/Sellers(SSN={keySSN},SellerID={keySellerID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchSeller([FromODataUri] int keySSN, [FromODataUri] int keySellerID, [FromBody]Delta<EBidderWeb.Server.Models.EBidder.Seller> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.Sellers.Where(i => i.SSN == keySSN && i.SellerID == keySellerID).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnSellerUpdated(item);
                this.context.Sellers.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Sellers.Where(i => i.SSN == keySSN && i.SellerID == keySellerID);
                ;
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnSellerCreated(EBidderWeb.Server.Models.EBidder.Seller item);
        partial void OnAfterSellerCreated(EBidderWeb.Server.Models.EBidder.Seller item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] EBidderWeb.Server.Models.EBidder.Seller item)
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

                this.OnSellerCreated(item);
                this.context.Sellers.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Sellers.Where(i => i.SSN == item.SSN && i.SellerID == item.SellerID);

                ;

                this.OnAfterSellerCreated(item);

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
