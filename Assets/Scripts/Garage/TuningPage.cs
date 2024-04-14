using System.Collections.Generic;
using TMPro;
using Tuning;
using UnityEngine;
using UnityEngine.UI;

namespace Garage
{
    public class TuningPage : Page
    {
        [SerializeField] private Button spoilerTuningButton;
        [SerializeField] private Button wheelTuningButton;
        [SerializeField] private Button colorTuningButton;
        [SerializeField] private Transform categoryGameObject;
        [SerializeField] private Button upgradeButtonPrefab;
        [SerializeField] private Button buyButton;
        
        private readonly List<Button> _upgradeButtons = new();
        private CarTuning _selectedCarTuning;
        private CarTuningData _savedCarTuningData;

        private void Awake()
        {
            spoilerTuningButton.onClick.AddListener(() => CreateUpgradeButtons(_selectedCarTuning.Data.SpoilerTuning));
            wheelTuningButton.onClick.AddListener(() => CreateUpgradeButtons(_selectedCarTuning.Data.WheelTuning));
            colorTuningButton.onClick.AddListener(() => CreateUpgradeButtons(_selectedCarTuning.Data.ColorTuning));
        }

        private void DestroyUpgradeButtons()
        {
            foreach (var upgradeButton in _upgradeButtons)
            {
                Destroy(upgradeButton.gameObject);
            }
            _upgradeButtons.Clear();
        }

        private void CreateUpgradeButtons(object tuning)
        {
            _selectedCarTuning.Data.ApplyTuning();
            DestroyUpgradeButtons();
            buyButton.interactable = false;
            buyButton.gameObject.SetActive(true);
            
            switch (tuning)
            {
                case Tuning<GameObject> tuningGameObject:
                    for (var index = 0; index < tuningGameObject.PriceObjectPairs.Length; index++)
                    {
                        var upgradeButton = CreateUpgradeButton(tuningGameObject, index, out var pair);

                        if (pair.Thumbnail != null)
                            upgradeButton.image.sprite = pair.Thumbnail;
                    }

                    break;
                case Tuning<Material> tuningMaterial:
                    for (var index = 0; index < tuningMaterial.PriceObjectPairs.Length; index++)
                    {
                        var upgradeButton = CreateUpgradeButton(tuningMaterial, index, out var pair);
                        
                        upgradeButton.image.color = pair.Upgrade.color;
                    }
                    
                    break;
            }
        }

        private Button CreateUpgradeButton<T>(Tuning<T> tuningUpgrade, int index, out PriceUpgradePair<T> pair)
        {
            pair = tuningUpgrade.PriceObjectPairs[index];
            var savedPair = _savedCarTuningData.GetTuningOfSameType(tuningUpgrade).PriceObjectPairs[index];
            var upgradeButton = Instantiate(upgradeButtonPrefab, categoryGameObject);
            var upgradeButtonText = upgradeButton.GetComponentInChildren<TMP_Text>();
            upgradeButtonText.SetText(savedPair.Price == 0 ? "Owned" : savedPair.Price.ToString());
            _upgradeButtons.Add(upgradeButton);
            upgradeButton.onClick.AddListener(() =>
            {
                tuningUpgrade.ApplyUpgrade(index);
                buyButton.interactable = savedPair.Price <= Player.Instance.Cash;
                buyButton.onClick.RemoveAllListeners();
                buyButton.onClick.AddListener(() =>
                {
                    _savedCarTuningData.SetSelectedAndBuy(tuningUpgrade, index);
                    buyButton.interactable = false;
                    upgradeButtonText.SetText("Owned");
                    Player.Instance.SaveTuning();
                    
                });
                buyButton.GetComponentInChildren<TMP_Text>().SetText(savedPair.Price == 0 ? "Select" : "Purchase & Select");
                
            });
            return upgradeButton;
        }
        
        
        public void Setup(GameObject carInstance)
        {
            _selectedCarTuning = carInstance.GetComponent<CarTuning>();
            _savedCarTuningData = Player.Instance.CarTunings[_selectedCarTuning.Data.CarId];
            gameObject.SetActive(true);
            if (_selectedCarTuning.HasTuning(_selectedCarTuning.Data.SpoilerTuning)) spoilerTuningButton.gameObject.SetActive(true);
            if (_selectedCarTuning.HasTuning(_selectedCarTuning.Data.WheelTuning)) wheelTuningButton.gameObject.SetActive(true);
            if (_selectedCarTuning.HasTuning(_selectedCarTuning.Data.ColorTuning)) colorTuningButton.gameObject.SetActive(true);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            spoilerTuningButton.gameObject.SetActive(false);
            wheelTuningButton.gameObject.SetActive(false);
            colorTuningButton.gameObject.SetActive(false);
            DestroyUpgradeButtons();
            buyButton.interactable = false;
            buyButton.gameObject.SetActive(false);
        }
    }
}