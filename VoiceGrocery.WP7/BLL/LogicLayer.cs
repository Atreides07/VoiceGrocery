using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VoiceGrocery.WP7.Model;
using VoiceGrocery.WP7.ServiceReference1;

namespace VoiceGrocery.WP7.BLL
{
    public class LogicLayer
    {
        private ShopList shopList = new ShopList();
        public LogicLayer()
        {
            shopList.ShopItems = new System.Collections.ObjectModel.ObservableCollection<ShopItem>();
        }

        public List<ShopItem>  GetShopItems()
        {
            return shopList.ShopItems.ToList();
        }

        public void Parse(string voiceText)
        {
            var words = voiceText.Split(new[]{' '}, StringSplitOptions.RemoveEmptyEntries);
            if(words.Length>1)
            {
                var command = words.First();
                if(command.Equals("купить"))
                {
                    Add(words.Skip(1));
                    IncrementUpdate();
                }
                if(command.StartsWith("купил"))
                {
                    SetBoughtStatusTrue(words.Skip(1));
                    IncrementUpdate();
                }
                if (command.Equals("удалить") || command.Equals("очистить"))
                {
                    if (words[1].Equals("список"))
                    {
                        shopList.ShopItems.Clear();
                    }
                    else
                    {
                        RemoveShopListItems(words.Skip(1));
                    }
                    IncrementUpdate();
                }

            }

        }

        private void RemoveShopListItems(IEnumerable<string> products)
        {
            var shopItems = shopList.ShopItems.Where(i => products.Contains(i.Name));
            foreach (var item in shopItems)
            {
                shopList.ShopItems.Remove(item);
            }
        }

        private void IncrementUpdate()
        {
            var shopListItem = new ShopList()
            {
                Version = shopList.Version + 1,
                ShopItems = shopList.ShopItems
            };
            var client = new ServiceReference1.GroceryServiceSoapClient();
            client.UploadVersionAsync(shopListItem);
            
        }

        private void SetBoughtStatusTrue(IEnumerable<string> products)
        {
            foreach (var product in products)
            {
                if (product.Length > 2)
                {
                    SetBoughtStatusTrue(product);
                }
            }
        }

        private void SetBoughtStatusTrue(string product)
        {
            var shopListItem = shopList.ShopItems.FirstOrDefault(i => i.Name.Equals(product));
            shopListItem.IsBought = true;
        }

        private void Add(IEnumerable<string> products)
        {
            foreach (var product in products)
            {
                if(product.Length>2)
                {
                    Add(product);
                }
            }
        }

        private void Add(string product)
        {
            var shopListItem = shopList.ShopItems.FirstOrDefault(i => i.Name.Equals(product));
            if (shopListItem == null)
            {
                shopListItem = new ShopItem();
                shopListItem.Name = product;
                shopList.ShopItems.Add(shopListItem);
            }
            else
            {
                shopListItem.IsBought = false;
            }
        }

        public int GetVersion()
        {
            return shopList.Version;
        }

        internal void UpdateShopList(ShopList shopList)
        {
            this.shopList = shopList;
        }
    }
}
