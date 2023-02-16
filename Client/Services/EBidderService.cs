
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Web;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using Radzen;

namespace EBidderWeb.Client
{
    public partial class EBidderService
    {
        private readonly HttpClient httpClient;
        private readonly Uri baseUri;
        private readonly NavigationManager navigationManager;

        public EBidderService(NavigationManager navigationManager, HttpClient httpClient, IConfiguration configuration)
        {
            this.httpClient = httpClient;

            this.navigationManager = navigationManager;
            this.baseUri = new Uri($"{navigationManager.BaseUri}odata/EBidder/");
        }


        public async System.Threading.Tasks.Task ExportAdminsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/ebidder/admins/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/ebidder/admins/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportAdminsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/ebidder/admins/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/ebidder/admins/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetAdmins(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<EBidderWeb.Server.Models.EBidder.Admin>> GetAdmins(Query query)
        {
            return await GetAdmins(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<EBidderWeb.Server.Models.EBidder.Admin>> GetAdmins(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"Admins");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetAdmins(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<EBidderWeb.Server.Models.EBidder.Admin>>(response);
        }

        partial void OnCreateAdmin(HttpRequestMessage requestMessage);

        public async Task<EBidderWeb.Server.Models.EBidder.Admin> CreateAdmin(EBidderWeb.Server.Models.EBidder.Admin admin = default(EBidderWeb.Server.Models.EBidder.Admin))
        {
            var uri = new Uri(baseUri, $"Admins");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(admin), Encoding.UTF8, "application/json");

            OnCreateAdmin(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<EBidderWeb.Server.Models.EBidder.Admin>(response);
        }

        partial void OnDeleteAdmin(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteAdmin(int adminId = default(int))
        {
            var uri = new Uri(baseUri, $"Admins({adminId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteAdmin(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetAdminByAdminId(HttpRequestMessage requestMessage);

        public async Task<EBidderWeb.Server.Models.EBidder.Admin> GetAdminByAdminId(string expand = default(string), int adminId = default(int))
        {
            var uri = new Uri(baseUri, $"Admins({adminId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetAdminByAdminId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<EBidderWeb.Server.Models.EBidder.Admin>(response);
        }

        partial void OnUpdateAdmin(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateAdmin(int adminId = default(int), EBidderWeb.Server.Models.EBidder.Admin admin = default(EBidderWeb.Server.Models.EBidder.Admin))
        {
            var uri = new Uri(baseUri, $"Admins({adminId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);


            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(admin), Encoding.UTF8, "application/json");

            OnUpdateAdmin(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportArtworksToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/ebidder/artworks/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/ebidder/artworks/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportArtworksToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/ebidder/artworks/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/ebidder/artworks/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetArtworks(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<EBidderWeb.Server.Models.EBidder.Artwork>> GetArtworks(Query query)
        {
            return await GetArtworks(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<EBidderWeb.Server.Models.EBidder.Artwork>> GetArtworks(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"Artworks");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetArtworks(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<EBidderWeb.Server.Models.EBidder.Artwork>>(response);
        }

        partial void OnCreateArtwork(HttpRequestMessage requestMessage);

        public async Task<EBidderWeb.Server.Models.EBidder.Artwork> CreateArtwork(EBidderWeb.Server.Models.EBidder.Artwork artwork = default(EBidderWeb.Server.Models.EBidder.Artwork))
        {
            var uri = new Uri(baseUri, $"Artworks");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(artwork), Encoding.UTF8, "application/json");

            OnCreateArtwork(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<EBidderWeb.Server.Models.EBidder.Artwork>(response);
        }

        partial void OnDeleteArtwork(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteArtwork(int artId = default(int))
        {
            var uri = new Uri(baseUri, $"Artworks({artId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteArtwork(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetArtworkByArtId(HttpRequestMessage requestMessage);

        public async Task<EBidderWeb.Server.Models.EBidder.Artwork> GetArtworkByArtId(string expand = default(string), int artId = default(int))
        {
            var uri = new Uri(baseUri, $"Artworks({artId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetArtworkByArtId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<EBidderWeb.Server.Models.EBidder.Artwork>(response);
        }

        partial void OnUpdateArtwork(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateArtwork(int artId = default(int), EBidderWeb.Server.Models.EBidder.Artwork artwork = default(EBidderWeb.Server.Models.EBidder.Artwork))
        {
            var uri = new Uri(baseUri, $"Artworks({artId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);


            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(artwork), Encoding.UTF8, "application/json");

            OnUpdateArtwork(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportBuyersToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/ebidder/buyers/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/ebidder/buyers/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportBuyersToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/ebidder/buyers/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/ebidder/buyers/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetBuyers(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<EBidderWeb.Server.Models.EBidder.Buyer>> GetBuyers(Query query)
        {
            return await GetBuyers(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<EBidderWeb.Server.Models.EBidder.Buyer>> GetBuyers(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"Buyers");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetBuyers(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<EBidderWeb.Server.Models.EBidder.Buyer>>(response);
        }

        partial void OnCreateBuyer(HttpRequestMessage requestMessage);

        public async Task<EBidderWeb.Server.Models.EBidder.Buyer> CreateBuyer(EBidderWeb.Server.Models.EBidder.Buyer buyer = default(EBidderWeb.Server.Models.EBidder.Buyer))
        {
            var uri = new Uri(baseUri, $"Buyers");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(buyer), Encoding.UTF8, "application/json");

            OnCreateBuyer(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<EBidderWeb.Server.Models.EBidder.Buyer>(response);
        }

        partial void OnDeleteBuyer(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteBuyer(int ssn = default(int), int buyerId = default(int))
        {
            var uri = new Uri(baseUri, $"Buyers(SSN={ssn},BuyerID={buyerId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteBuyer(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetBuyerBySsnAndBuyerId(HttpRequestMessage requestMessage);

        public async Task<EBidderWeb.Server.Models.EBidder.Buyer> GetBuyerBySsnAndBuyerId(string expand = default(string), int ssn = default(int), int buyerId = default(int))
        {
            var uri = new Uri(baseUri, $"Buyers(SSN={ssn},BuyerID={buyerId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetBuyerBySsnAndBuyerId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<EBidderWeb.Server.Models.EBidder.Buyer>(response);
        }

        partial void OnUpdateBuyer(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateBuyer(int ssn = default(int), int buyerId = default(int), EBidderWeb.Server.Models.EBidder.Buyer buyer = default(EBidderWeb.Server.Models.EBidder.Buyer))
        {
            var uri = new Uri(baseUri, $"Buyers(SSN={ssn},BuyerID={buyerId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);


            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(buyer), Encoding.UTF8, "application/json");

            OnUpdateBuyer(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportGameItemsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/ebidder/gameitems/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/ebidder/gameitems/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportGameItemsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/ebidder/gameitems/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/ebidder/gameitems/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetGameItems(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<EBidderWeb.Server.Models.EBidder.GameItem>> GetGameItems(Query query)
        {
            return await GetGameItems(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<EBidderWeb.Server.Models.EBidder.GameItem>> GetGameItems(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"GameItems");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetGameItems(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<EBidderWeb.Server.Models.EBidder.GameItem>>(response);
        }

        partial void OnCreateGameItem(HttpRequestMessage requestMessage);

        public async Task<EBidderWeb.Server.Models.EBidder.GameItem> CreateGameItem(EBidderWeb.Server.Models.EBidder.GameItem gameItem = default(EBidderWeb.Server.Models.EBidder.GameItem))
        {
            var uri = new Uri(baseUri, $"GameItems");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(gameItem), Encoding.UTF8, "application/json");

            OnCreateGameItem(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<EBidderWeb.Server.Models.EBidder.GameItem>(response);
        }

        partial void OnDeleteGameItem(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteGameItem(int gameId = default(int))
        {
            var uri = new Uri(baseUri, $"GameItems({gameId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteGameItem(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetGameItemByGameId(HttpRequestMessage requestMessage);

        public async Task<EBidderWeb.Server.Models.EBidder.GameItem> GetGameItemByGameId(string expand = default(string), int gameId = default(int))
        {
            var uri = new Uri(baseUri, $"GameItems({gameId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetGameItemByGameId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<EBidderWeb.Server.Models.EBidder.GameItem>(response);
        }

        partial void OnUpdateGameItem(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateGameItem(int gameId = default(int), EBidderWeb.Server.Models.EBidder.GameItem gameItem = default(EBidderWeb.Server.Models.EBidder.GameItem))
        {
            var uri = new Uri(baseUri, $"GameItems({gameId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);


            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(gameItem), Encoding.UTF8, "application/json");

            OnUpdateGameItem(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportNftsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/ebidder/nfts/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/ebidder/nfts/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportNftsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/ebidder/nfts/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/ebidder/nfts/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetNfts(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<EBidderWeb.Server.Models.EBidder.Nft>> GetNfts(Query query)
        {
            return await GetNfts(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<EBidderWeb.Server.Models.EBidder.Nft>> GetNfts(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"Nfts");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetNfts(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<EBidderWeb.Server.Models.EBidder.Nft>>(response);
        }

        partial void OnCreateNft(HttpRequestMessage requestMessage);

        public async Task<EBidderWeb.Server.Models.EBidder.Nft> CreateNft(EBidderWeb.Server.Models.EBidder.Nft nft = default(EBidderWeb.Server.Models.EBidder.Nft))
        {
            var uri = new Uri(baseUri, $"Nfts");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(nft), Encoding.UTF8, "application/json");

            OnCreateNft(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<EBidderWeb.Server.Models.EBidder.Nft>(response);
        }

        partial void OnDeleteNft(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteNft(int nftId = default(int))
        {
            var uri = new Uri(baseUri, $"Nfts({nftId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteNft(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetNftByNftId(HttpRequestMessage requestMessage);

        public async Task<EBidderWeb.Server.Models.EBidder.Nft> GetNftByNftId(string expand = default(string), int nftId = default(int))
        {
            var uri = new Uri(baseUri, $"Nfts({nftId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetNftByNftId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<EBidderWeb.Server.Models.EBidder.Nft>(response);
        }

        partial void OnUpdateNft(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateNft(int nftId = default(int), EBidderWeb.Server.Models.EBidder.Nft nft = default(EBidderWeb.Server.Models.EBidder.Nft))
        {
            var uri = new Uri(baseUri, $"Nfts({nftId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);


            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(nft), Encoding.UTF8, "application/json");

            OnUpdateNft(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportSellersToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/ebidder/sellers/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/ebidder/sellers/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportSellersToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/ebidder/sellers/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/ebidder/sellers/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetSellers(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<EBidderWeb.Server.Models.EBidder.Seller>> GetSellers(Query query)
        {
            return await GetSellers(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<EBidderWeb.Server.Models.EBidder.Seller>> GetSellers(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"Sellers");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetSellers(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<EBidderWeb.Server.Models.EBidder.Seller>>(response);
        }

        partial void OnCreateSeller(HttpRequestMessage requestMessage);

        public async Task<EBidderWeb.Server.Models.EBidder.Seller> CreateSeller(EBidderWeb.Server.Models.EBidder.Seller seller = default(EBidderWeb.Server.Models.EBidder.Seller))
        {
            var uri = new Uri(baseUri, $"Sellers");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(seller), Encoding.UTF8, "application/json");

            OnCreateSeller(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<EBidderWeb.Server.Models.EBidder.Seller>(response);
        }

        partial void OnDeleteSeller(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteSeller(int ssn = default(int), int sellerId = default(int))
        {
            var uri = new Uri(baseUri, $"Sellers(SSN={ssn},SellerID={sellerId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteSeller(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetSellerBySsnAndSellerId(HttpRequestMessage requestMessage);

        public async Task<EBidderWeb.Server.Models.EBidder.Seller> GetSellerBySsnAndSellerId(string expand = default(string), int ssn = default(int), int sellerId = default(int))
        {
            var uri = new Uri(baseUri, $"Sellers(SSN={ssn},SellerID={sellerId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetSellerBySsnAndSellerId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<EBidderWeb.Server.Models.EBidder.Seller>(response);
        }

        partial void OnUpdateSeller(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateSeller(int ssn = default(int), int sellerId = default(int), EBidderWeb.Server.Models.EBidder.Seller seller = default(EBidderWeb.Server.Models.EBidder.Seller))
        {
            var uri = new Uri(baseUri, $"Sellers(SSN={ssn},SellerID={sellerId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);


            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(seller), Encoding.UTF8, "application/json");

            OnUpdateSeller(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }
    }
}