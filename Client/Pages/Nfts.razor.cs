using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace EBidderWeb.Client.Pages
{
    public partial class Nfts
    {
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected TooltipService TooltipService { get; set; }

        [Inject]
        protected ContextMenuService ContextMenuService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        [Inject]
        public EBidderService EBidderService { get; set; }

        protected IEnumerable<EBidderWeb.Server.Models.EBidder.Nft> nfts;

        protected RadzenDataGrid<EBidderWeb.Server.Models.EBidder.Nft> grid0;
        protected int count;

        protected async Task Grid0LoadData(LoadDataArgs args)
        {
            try
            {
                var result = await EBidderService.GetNfts(filter: $"{args.Filter}", orderby: $"{args.OrderBy}", top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null);
                nfts = result.Value.AsODataEnumerable();
                count = result.Count;
            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Nfts" });
            }
        }    

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddNft>("Add Nft", null);
            await grid0.Reload();
        }

        protected async Task EditRow(EBidderWeb.Server.Models.EBidder.Nft args)
        {
            await DialogService.OpenAsync<EditNft>("Edit Nft", new Dictionary<string, object> { {"NftID", args.NftID} });
            await grid0.Reload();
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, EBidderWeb.Server.Models.EBidder.Nft nft)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await EBidderService.DeleteNft(nftId:nft.NftID);

                    if (deleteResult != null)
                    {
                        await grid0.Reload();
                    }
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage
                { 
                    Severity = NotificationSeverity.Error,
                    Summary = $"Error", 
                    Detail = $"Unable to delete Nft" 
                });
            }
        }
    }
}