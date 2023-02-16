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
    [Route("odata/EBidder/Admins")]
    public partial class AdminsController : ODataController
    {
        private EBidderWeb.Server.Data.EBidderContext context;

        public AdminsController(EBidderWeb.Server.Data.EBidderContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<EBidderWeb.Server.Models.EBidder.Admin> GetAdmins()
        {
            var items = this.context.Admins.AsQueryable<EBidderWeb.Server.Models.EBidder.Admin>();
            this.OnAdminsRead(ref items);

            return items;
        }

        partial void OnAdminsRead(ref IQueryable<EBidderWeb.Server.Models.EBidder.Admin> items);

        partial void OnAdminGet(ref SingleResult<EBidderWeb.Server.Models.EBidder.Admin> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/EBidder/Admins(AdminID={AdminID})")]
        public SingleResult<EBidderWeb.Server.Models.EBidder.Admin> GetAdmin(int key)
        {
            var items = this.context.Admins.Where(i => i.AdminID == key);
            var result = SingleResult.Create(items);

            OnAdminGet(ref result);

            return result;
        }
        partial void OnAdminDeleted(EBidderWeb.Server.Models.EBidder.Admin item);
        partial void OnAfterAdminDeleted(EBidderWeb.Server.Models.EBidder.Admin item);

        [HttpDelete("/odata/EBidder/Admins(AdminID={AdminID})")]
        public IActionResult DeleteAdmin(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.Admins
                    .Where(i => i.AdminID == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnAdminDeleted(item);
                this.context.Admins.Remove(item);
                this.context.SaveChanges();
                this.OnAfterAdminDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnAdminUpdated(EBidderWeb.Server.Models.EBidder.Admin item);
        partial void OnAfterAdminUpdated(EBidderWeb.Server.Models.EBidder.Admin item);

        [HttpPut("/odata/EBidder/Admins(AdminID={AdminID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutAdmin(int key, [FromBody]EBidderWeb.Server.Models.EBidder.Admin item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null || (item.AdminID != key))
                {
                    return BadRequest();
                }
                this.OnAdminUpdated(item);
                this.context.Admins.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Admins.Where(i => i.AdminID == key);
                ;
                this.OnAfterAdminUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/EBidder/Admins(AdminID={AdminID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchAdmin(int key, [FromBody]Delta<EBidderWeb.Server.Models.EBidder.Admin> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.Admins.Where(i => i.AdminID == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnAdminUpdated(item);
                this.context.Admins.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Admins.Where(i => i.AdminID == key);
                ;
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnAdminCreated(EBidderWeb.Server.Models.EBidder.Admin item);
        partial void OnAfterAdminCreated(EBidderWeb.Server.Models.EBidder.Admin item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] EBidderWeb.Server.Models.EBidder.Admin item)
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

                this.OnAdminCreated(item);
                this.context.Admins.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Admins.Where(i => i.AdminID == item.AdminID);

                ;

                this.OnAfterAdminCreated(item);

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
