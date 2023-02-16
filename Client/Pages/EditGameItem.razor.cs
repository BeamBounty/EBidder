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
    public partial class EditGameItem
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

        [Parameter]
        public int GameID { get; set; }

        protected override async Task OnInitializedAsync()
        {
            gameItem = await EBidderService.GetGameItemByGameId(gameId:GameID);
        }
        protected bool errorVisible;
        protected EBidderWeb.Server.Models.EBidder.GameItem gameItem;

        protected async Task FormSubmit()
        {
            try
            {
                await EBidderService.UpdateGameItem(gameId:GameID, gameItem);
                DialogService.Close(gameItem);
            }
            catch (Exception ex)
            {
                errorVisible = true;
            }
        }

        protected async Task CancelButtonClick(MouseEventArgs args)
        {
            DialogService.Close(null);
        }
    }
}