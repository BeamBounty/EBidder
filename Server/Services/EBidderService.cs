using System;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Radzen;

using EBidderWeb.Server.Data;

namespace EBidderWeb.Server
{
    public partial class EBidderService
    {
        EBidderContext Context
        {
           get
           {
             return this.context;
           }
        }

        private readonly EBidderContext context;
        private readonly NavigationManager navigationManager;

        public EBidderService(EBidderContext context, NavigationManager navigationManager)
        {
            this.context = context;
            this.navigationManager = navigationManager;
        }

        public void Reset() => Context.ChangeTracker.Entries().Where(e => e.Entity != null).ToList().ForEach(e => e.State = EntityState.Detached);


        public async Task ExportAdminsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/ebidder/admins/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/ebidder/admins/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportAdminsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/ebidder/admins/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/ebidder/admins/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnAdminsRead(ref IQueryable<EBidderWeb.Server.Models.EBidder.Admin> items);

        public async Task<IQueryable<EBidderWeb.Server.Models.EBidder.Admin>> GetAdmins(Query query = null)
        {
            var items = Context.Admins.AsQueryable();

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnAdminsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnAdminGet(EBidderWeb.Server.Models.EBidder.Admin item);

        public async Task<EBidderWeb.Server.Models.EBidder.Admin> GetAdminByAdminId(int adminid)
        {
            var items = Context.Admins
                              .AsNoTracking()
                              .Where(i => i.AdminID == adminid);

  
            var itemToReturn = items.FirstOrDefault();

            OnAdminGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnAdminCreated(EBidderWeb.Server.Models.EBidder.Admin item);
        partial void OnAfterAdminCreated(EBidderWeb.Server.Models.EBidder.Admin item);

        public async Task<EBidderWeb.Server.Models.EBidder.Admin> CreateAdmin(EBidderWeb.Server.Models.EBidder.Admin admin)
        {
            OnAdminCreated(admin);

            var existingItem = Context.Admins
                              .Where(i => i.AdminID == admin.AdminID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Admins.Add(admin);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(admin).State = EntityState.Detached;
                throw;
            }

            OnAfterAdminCreated(admin);

            return admin;
        }

        public async Task<EBidderWeb.Server.Models.EBidder.Admin> CancelAdminChanges(EBidderWeb.Server.Models.EBidder.Admin item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnAdminUpdated(EBidderWeb.Server.Models.EBidder.Admin item);
        partial void OnAfterAdminUpdated(EBidderWeb.Server.Models.EBidder.Admin item);

        public async Task<EBidderWeb.Server.Models.EBidder.Admin> UpdateAdmin(int adminid, EBidderWeb.Server.Models.EBidder.Admin admin)
        {
            OnAdminUpdated(admin);

            var itemToUpdate = Context.Admins
                              .Where(i => i.AdminID == admin.AdminID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(admin);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterAdminUpdated(admin);

            return admin;
        }

        partial void OnAdminDeleted(EBidderWeb.Server.Models.EBidder.Admin item);
        partial void OnAfterAdminDeleted(EBidderWeb.Server.Models.EBidder.Admin item);

        public async Task<EBidderWeb.Server.Models.EBidder.Admin> DeleteAdmin(int adminid)
        {
            var itemToDelete = Context.Admins
                              .Where(i => i.AdminID == adminid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnAdminDeleted(itemToDelete);


            Context.Admins.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterAdminDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportArtworksToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/ebidder/artworks/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/ebidder/artworks/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportArtworksToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/ebidder/artworks/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/ebidder/artworks/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnArtworksRead(ref IQueryable<EBidderWeb.Server.Models.EBidder.Artwork> items);

        public async Task<IQueryable<EBidderWeb.Server.Models.EBidder.Artwork>> GetArtworks(Query query = null)
        {
            var items = Context.Artworks.AsQueryable();

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnArtworksRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnArtworkGet(EBidderWeb.Server.Models.EBidder.Artwork item);

        public async Task<EBidderWeb.Server.Models.EBidder.Artwork> GetArtworkByArtId(int artid)
        {
            var items = Context.Artworks
                              .AsNoTracking()
                              .Where(i => i.ArtID == artid);

  
            var itemToReturn = items.FirstOrDefault();

            OnArtworkGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnArtworkCreated(EBidderWeb.Server.Models.EBidder.Artwork item);
        partial void OnAfterArtworkCreated(EBidderWeb.Server.Models.EBidder.Artwork item);

        public async Task<EBidderWeb.Server.Models.EBidder.Artwork> CreateArtwork(EBidderWeb.Server.Models.EBidder.Artwork artwork)
        {
            OnArtworkCreated(artwork);

            var existingItem = Context.Artworks
                              .Where(i => i.ArtID == artwork.ArtID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Artworks.Add(artwork);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(artwork).State = EntityState.Detached;
                throw;
            }

            OnAfterArtworkCreated(artwork);

            return artwork;
        }

        public async Task<EBidderWeb.Server.Models.EBidder.Artwork> CancelArtworkChanges(EBidderWeb.Server.Models.EBidder.Artwork item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnArtworkUpdated(EBidderWeb.Server.Models.EBidder.Artwork item);
        partial void OnAfterArtworkUpdated(EBidderWeb.Server.Models.EBidder.Artwork item);

        public async Task<EBidderWeb.Server.Models.EBidder.Artwork> UpdateArtwork(int artid, EBidderWeb.Server.Models.EBidder.Artwork artwork)
        {
            OnArtworkUpdated(artwork);

            var itemToUpdate = Context.Artworks
                              .Where(i => i.ArtID == artwork.ArtID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(artwork);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterArtworkUpdated(artwork);

            return artwork;
        }

        partial void OnArtworkDeleted(EBidderWeb.Server.Models.EBidder.Artwork item);
        partial void OnAfterArtworkDeleted(EBidderWeb.Server.Models.EBidder.Artwork item);

        public async Task<EBidderWeb.Server.Models.EBidder.Artwork> DeleteArtwork(int artid)
        {
            var itemToDelete = Context.Artworks
                              .Where(i => i.ArtID == artid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnArtworkDeleted(itemToDelete);


            Context.Artworks.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterArtworkDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportBuyersToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/ebidder/buyers/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/ebidder/buyers/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportBuyersToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/ebidder/buyers/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/ebidder/buyers/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnBuyersRead(ref IQueryable<EBidderWeb.Server.Models.EBidder.Buyer> items);

        public async Task<IQueryable<EBidderWeb.Server.Models.EBidder.Buyer>> GetBuyers(Query query = null)
        {
            var items = Context.Buyers.AsQueryable();

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnBuyersRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnBuyerGet(EBidderWeb.Server.Models.EBidder.Buyer item);

        public async Task<EBidderWeb.Server.Models.EBidder.Buyer> GetBuyerBySsnAndBuyerId(int ssn, int buyerid)
        {
            var items = Context.Buyers
                              .AsNoTracking()
                              .Where(i => i.SSN == ssn && i.BuyerID == buyerid);

  
            var itemToReturn = items.FirstOrDefault();

            OnBuyerGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnBuyerCreated(EBidderWeb.Server.Models.EBidder.Buyer item);
        partial void OnAfterBuyerCreated(EBidderWeb.Server.Models.EBidder.Buyer item);

        public async Task<EBidderWeb.Server.Models.EBidder.Buyer> CreateBuyer(EBidderWeb.Server.Models.EBidder.Buyer buyer)
        {
            OnBuyerCreated(buyer);

            var existingItem = Context.Buyers
                              .Where(i => i.SSN == buyer.SSN && i.BuyerID == buyer.BuyerID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Buyers.Add(buyer);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(buyer).State = EntityState.Detached;
                throw;
            }

            OnAfterBuyerCreated(buyer);

            return buyer;
        }

        public async Task<EBidderWeb.Server.Models.EBidder.Buyer> CancelBuyerChanges(EBidderWeb.Server.Models.EBidder.Buyer item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnBuyerUpdated(EBidderWeb.Server.Models.EBidder.Buyer item);
        partial void OnAfterBuyerUpdated(EBidderWeb.Server.Models.EBidder.Buyer item);

        public async Task<EBidderWeb.Server.Models.EBidder.Buyer> UpdateBuyer(int ssn, int buyerid, EBidderWeb.Server.Models.EBidder.Buyer buyer)
        {
            OnBuyerUpdated(buyer);

            var itemToUpdate = Context.Buyers
                              .Where(i => i.SSN == buyer.SSN && i.BuyerID == buyer.BuyerID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(buyer);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterBuyerUpdated(buyer);

            return buyer;
        }

        partial void OnBuyerDeleted(EBidderWeb.Server.Models.EBidder.Buyer item);
        partial void OnAfterBuyerDeleted(EBidderWeb.Server.Models.EBidder.Buyer item);

        public async Task<EBidderWeb.Server.Models.EBidder.Buyer> DeleteBuyer(int ssn, int buyerid)
        {
            var itemToDelete = Context.Buyers
                              .Where(i => i.SSN == ssn && i.BuyerID == buyerid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnBuyerDeleted(itemToDelete);


            Context.Buyers.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterBuyerDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportGameItemsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/ebidder/gameitems/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/ebidder/gameitems/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportGameItemsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/ebidder/gameitems/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/ebidder/gameitems/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGameItemsRead(ref IQueryable<EBidderWeb.Server.Models.EBidder.GameItem> items);

        public async Task<IQueryable<EBidderWeb.Server.Models.EBidder.GameItem>> GetGameItems(Query query = null)
        {
            var items = Context.GameItems.AsQueryable();

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnGameItemsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnGameItemGet(EBidderWeb.Server.Models.EBidder.GameItem item);

        public async Task<EBidderWeb.Server.Models.EBidder.GameItem> GetGameItemByGameId(int gameid)
        {
            var items = Context.GameItems
                              .AsNoTracking()
                              .Where(i => i.GameID == gameid);

  
            var itemToReturn = items.FirstOrDefault();

            OnGameItemGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnGameItemCreated(EBidderWeb.Server.Models.EBidder.GameItem item);
        partial void OnAfterGameItemCreated(EBidderWeb.Server.Models.EBidder.GameItem item);

        public async Task<EBidderWeb.Server.Models.EBidder.GameItem> CreateGameItem(EBidderWeb.Server.Models.EBidder.GameItem gameitem)
        {
            OnGameItemCreated(gameitem);

            var existingItem = Context.GameItems
                              .Where(i => i.GameID == gameitem.GameID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.GameItems.Add(gameitem);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(gameitem).State = EntityState.Detached;
                throw;
            }

            OnAfterGameItemCreated(gameitem);

            return gameitem;
        }

        public async Task<EBidderWeb.Server.Models.EBidder.GameItem> CancelGameItemChanges(EBidderWeb.Server.Models.EBidder.GameItem item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnGameItemUpdated(EBidderWeb.Server.Models.EBidder.GameItem item);
        partial void OnAfterGameItemUpdated(EBidderWeb.Server.Models.EBidder.GameItem item);

        public async Task<EBidderWeb.Server.Models.EBidder.GameItem> UpdateGameItem(int gameid, EBidderWeb.Server.Models.EBidder.GameItem gameitem)
        {
            OnGameItemUpdated(gameitem);

            var itemToUpdate = Context.GameItems
                              .Where(i => i.GameID == gameitem.GameID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(gameitem);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterGameItemUpdated(gameitem);

            return gameitem;
        }

        partial void OnGameItemDeleted(EBidderWeb.Server.Models.EBidder.GameItem item);
        partial void OnAfterGameItemDeleted(EBidderWeb.Server.Models.EBidder.GameItem item);

        public async Task<EBidderWeb.Server.Models.EBidder.GameItem> DeleteGameItem(int gameid)
        {
            var itemToDelete = Context.GameItems
                              .Where(i => i.GameID == gameid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnGameItemDeleted(itemToDelete);


            Context.GameItems.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterGameItemDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportNftsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/ebidder/nfts/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/ebidder/nfts/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportNftsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/ebidder/nfts/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/ebidder/nfts/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnNftsRead(ref IQueryable<EBidderWeb.Server.Models.EBidder.Nft> items);

        public async Task<IQueryable<EBidderWeb.Server.Models.EBidder.Nft>> GetNfts(Query query = null)
        {
            var items = Context.Nfts.AsQueryable();

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnNftsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnNftGet(EBidderWeb.Server.Models.EBidder.Nft item);

        public async Task<EBidderWeb.Server.Models.EBidder.Nft> GetNftByNftId(int nftid)
        {
            var items = Context.Nfts
                              .AsNoTracking()
                              .Where(i => i.NftID == nftid);

  
            var itemToReturn = items.FirstOrDefault();

            OnNftGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnNftCreated(EBidderWeb.Server.Models.EBidder.Nft item);
        partial void OnAfterNftCreated(EBidderWeb.Server.Models.EBidder.Nft item);

        public async Task<EBidderWeb.Server.Models.EBidder.Nft> CreateNft(EBidderWeb.Server.Models.EBidder.Nft nft)
        {
            OnNftCreated(nft);

            var existingItem = Context.Nfts
                              .Where(i => i.NftID == nft.NftID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Nfts.Add(nft);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(nft).State = EntityState.Detached;
                throw;
            }

            OnAfterNftCreated(nft);

            return nft;
        }

        public async Task<EBidderWeb.Server.Models.EBidder.Nft> CancelNftChanges(EBidderWeb.Server.Models.EBidder.Nft item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnNftUpdated(EBidderWeb.Server.Models.EBidder.Nft item);
        partial void OnAfterNftUpdated(EBidderWeb.Server.Models.EBidder.Nft item);

        public async Task<EBidderWeb.Server.Models.EBidder.Nft> UpdateNft(int nftid, EBidderWeb.Server.Models.EBidder.Nft nft)
        {
            OnNftUpdated(nft);

            var itemToUpdate = Context.Nfts
                              .Where(i => i.NftID == nft.NftID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(nft);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterNftUpdated(nft);

            return nft;
        }

        partial void OnNftDeleted(EBidderWeb.Server.Models.EBidder.Nft item);
        partial void OnAfterNftDeleted(EBidderWeb.Server.Models.EBidder.Nft item);

        public async Task<EBidderWeb.Server.Models.EBidder.Nft> DeleteNft(int nftid)
        {
            var itemToDelete = Context.Nfts
                              .Where(i => i.NftID == nftid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnNftDeleted(itemToDelete);


            Context.Nfts.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterNftDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportSellersToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/ebidder/sellers/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/ebidder/sellers/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportSellersToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/ebidder/sellers/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/ebidder/sellers/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnSellersRead(ref IQueryable<EBidderWeb.Server.Models.EBidder.Seller> items);

        public async Task<IQueryable<EBidderWeb.Server.Models.EBidder.Seller>> GetSellers(Query query = null)
        {
            var items = Context.Sellers.AsQueryable();

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnSellersRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnSellerGet(EBidderWeb.Server.Models.EBidder.Seller item);

        public async Task<EBidderWeb.Server.Models.EBidder.Seller> GetSellerBySsnAndSellerId(int ssn, int sellerid)
        {
            var items = Context.Sellers
                              .AsNoTracking()
                              .Where(i => i.SSN == ssn && i.SellerID == sellerid);

  
            var itemToReturn = items.FirstOrDefault();

            OnSellerGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnSellerCreated(EBidderWeb.Server.Models.EBidder.Seller item);
        partial void OnAfterSellerCreated(EBidderWeb.Server.Models.EBidder.Seller item);

        public async Task<EBidderWeb.Server.Models.EBidder.Seller> CreateSeller(EBidderWeb.Server.Models.EBidder.Seller seller)
        {
            OnSellerCreated(seller);

            var existingItem = Context.Sellers
                              .Where(i => i.SSN == seller.SSN && i.SellerID == seller.SellerID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Sellers.Add(seller);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(seller).State = EntityState.Detached;
                throw;
            }

            OnAfterSellerCreated(seller);

            return seller;
        }

        public async Task<EBidderWeb.Server.Models.EBidder.Seller> CancelSellerChanges(EBidderWeb.Server.Models.EBidder.Seller item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnSellerUpdated(EBidderWeb.Server.Models.EBidder.Seller item);
        partial void OnAfterSellerUpdated(EBidderWeb.Server.Models.EBidder.Seller item);

        public async Task<EBidderWeb.Server.Models.EBidder.Seller> UpdateSeller(int ssn, int sellerid, EBidderWeb.Server.Models.EBidder.Seller seller)
        {
            OnSellerUpdated(seller);

            var itemToUpdate = Context.Sellers
                              .Where(i => i.SSN == seller.SSN && i.SellerID == seller.SellerID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(seller);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterSellerUpdated(seller);

            return seller;
        }

        partial void OnSellerDeleted(EBidderWeb.Server.Models.EBidder.Seller item);
        partial void OnAfterSellerDeleted(EBidderWeb.Server.Models.EBidder.Seller item);

        public async Task<EBidderWeb.Server.Models.EBidder.Seller> DeleteSeller(int ssn, int sellerid)
        {
            var itemToDelete = Context.Sellers
                              .Where(i => i.SSN == ssn && i.SellerID == sellerid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnSellerDeleted(itemToDelete);


            Context.Sellers.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterSellerDeleted(itemToDelete);

            return itemToDelete;
        }
        }
}