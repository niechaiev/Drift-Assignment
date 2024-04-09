using System.Collections.Generic;
using UnityEngine;

namespace Garage
{
    public class IAPPage : Page
    {
        [SerializeField] private IAPItem iapItem;
        [SerializeField] private Transform parent;
        private List<IAPItem> iapItems = new();
        
        public void Setup()
        {
            Clear();
            gameObject.SetActive(true);
            CreateIAPItem(false, 100, "₴ 0.99");
            CreateIAPItem(false, 500, "₴ 1.99");
            CreateIAPItem(false, 1000, "₴ 2.99");
            CreateIAPItem(false, 2000, "₴ 3.99");
            CreateIAPItem(false, 5000, "₴ 4.99");
            CreateIAPItem(true, 50, "₴ 0.99");
            CreateIAPItem(true, 100, "₴ 1.99");
            CreateIAPItem(true, 250, "₴ 2.99");
            CreateIAPItem(true, 500, "₴ 3.99");
            CreateIAPItem(true, 1000, "₴ 4.99");
        }

        private void CreateIAPItem(bool isGold, int amount, string price)
        {
            var item = Instantiate(iapItem.gameObject, parent).GetComponent<IAPItem>();
            item.Setup(isGold, amount, price);
            item.OnClick = Player.Instance.AddBalance;
            iapItems.Add(item);
        }

        private void Clear()
        {
            foreach (var item in iapItems)
            {
                Destroy(item.gameObject);
            }
            iapItems.Clear();
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();
            Clear();
        }
    }
}