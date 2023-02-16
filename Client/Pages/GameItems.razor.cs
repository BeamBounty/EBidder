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
    public partial class GameItems
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

        protected IEnumerable<EBidderWeb.Server.Models.EBidder.GameItem> gameItems;

        protected RadzenDataGrid<EBidderWeb.Server.Models.EBidder.GameItem> grid0;
        protected int count;

        protected async Task Grid0LoadData(LoadDataArgs args)
        {
            try
            {
                var result = await EBidderService.GetGameItems(filter: $"{args.Filter}", orderby: $"{args.OrderBy}", top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null);
                gameItems = result.Value.AsODataEnumerable();
                count = result.Count;
            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load GameItems" });
            }
        }    

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddGameItem>("Add GameItem", null);
            await grid0.Reload();
        }

        protected async Task EditRow(EBidderWeb.Server.Models.EBidder.GameItem args)
        {
            await DialogService.OpenAsync<EditGameItem>("Edit GameItem", new Dictionary<string, object> { {"GameID", args.GameID} });
            await grid0.Reload();
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, EBidderWeb.Server.Models.EBidder.GameItem gameItem)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await EBidderService.DeleteGameItem(gameId:gameItem.GameID);

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
                    Detail = $"Unable to delete GameItem" 
                });
            }
        }
    }
}