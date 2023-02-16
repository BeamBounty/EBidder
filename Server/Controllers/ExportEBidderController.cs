using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

using EBidderWeb.Server.Data;

namespace EBidderWeb.Server.Controllers
{
    public partial class ExportEBidderController : ExportController
    {
        private readonly EBidderContext context;
        private readonly EBidderService service;

        public ExportEBidderController(EBidderContext context, EBidderService service)
        {
            this.service = service;
            this.context = context;
        }

        [HttpGet("/export/EBidder/admins/csv")]
        [HttpGet("/export/EBidder/admins/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAdminsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetAdmins(), Request.Query), fileName);
        }

        [HttpGet("/export/EBidder/admins/excel")]
        [HttpGet("/export/EBidder/admins/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAdminsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetAdmins(), Request.Query), fileName);
        }

        [HttpGet("/export/EBidder/artworks/csv")]
        [HttpGet("/export/EBidder/artworks/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportArtworksToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetArtworks(), Request.Query), fileName);
        }

        [HttpGet("/export/EBidder/artworks/excel")]
        [HttpGet("/export/EBidder/artworks/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportArtworksToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetArtworks(), Request.Query), fileName);
        }

        [HttpGet("/export/EBidder/buyers/csv")]
        [HttpGet("/export/EBidder/buyers/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportBuyersToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetBuyers(), Request.Query), fileName);
        }

        [HttpGet("/export/EBidder/buyers/excel")]
        [HttpGet("/export/EBidder/buyers/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportBuyersToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetBuyers(), Request.Query), fileName);
        }

        [HttpGet("/export/EBidder/gameitems/csv")]
        [HttpGet("/export/EBidder/gameitems/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportGameItemsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetGameItems(), Request.Query), fileName);
        }

        [HttpGet("/export/EBidder/gameitems/excel")]
        [HttpGet("/export/EBidder/gameitems/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportGameItemsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetGameItems(), Request.Query), fileName);
        }

        [HttpGet("/export/EBidder/nfts/csv")]
        [HttpGet("/export/EBidder/nfts/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportNftsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetNfts(), Request.Query), fileName);
        }

        [HttpGet("/export/EBidder/nfts/excel")]
        [HttpGet("/export/EBidder/nfts/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportNftsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetNfts(), Request.Query), fileName);
        }

        [HttpGet("/export/EBidder/sellers/csv")]
        [HttpGet("/export/EBidder/sellers/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportSellersToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetSellers(), Request.Query), fileName);
        }

        [HttpGet("/export/EBidder/sellers/excel")]
        [HttpGet("/export/EBidder/sellers/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportSellersToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetSellers(), Request.Query), fileName);
        }
    }
}
