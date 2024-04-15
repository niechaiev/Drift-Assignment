using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

namespace Garage
{
    public class IAPPage : Page, IDetailedStoreListener
    {
        [SerializeField] private IAPItem iapItem;
        [SerializeField] private Transform parent;
        private readonly List<IAPItem> _iapItems = new();
        private IStoreController _storeController;
        
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
            SetupBuilder();
        }

        private void SetupBuilder()
        {
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
            foreach (var item in _iapItems)
            {
                builder.AddProduct(item.Id, ProductType.Consumable);
            }

            UnityPurchasing.Initialize(this, builder);
        }

        private void CreateIAPItem(bool isGold, int amount, string price)
        {
            var item = Instantiate(iapItem.gameObject, parent).GetComponent<IAPItem>();
            item.Setup(isGold, amount, price);
            item.OnClick = id =>
            {
#if PLATFORM_STANDALONE_WIN || UNITY_EDITOR
                _storeController.InitiatePurchase(id);
#else
                Player.Instance.AddBalance(isGold, amount);
#endif
            };
            _iapItems.Add(item);
        }

        private void Clear()
        {
            foreach (var item in _iapItems)
            {
                Destroy(item.gameObject);
            }
            _iapItems.Clear();
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();
            Clear();
        }
        
        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            _storeController = controller;
        }
        
        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
        {
            var product = purchaseEvent.purchasedProduct;
            var item = _iapItems.Find(item => item.Id == product.definition.id);
            Player.Instance.AddBalance(item.IsGold, item.Amount);
            return PurchaseProcessingResult.Complete;
        }
        
        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            throw new System.NotImplementedException();
        }
        
        public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
        {
            throw new System.NotImplementedException();
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            throw new System.NotImplementedException();
        }

        public void OnInitializeFailed(InitializationFailureReason error, string message)
        {
            Debug.LogError(error);
            Debug.LogError(message);
        }
    }
}