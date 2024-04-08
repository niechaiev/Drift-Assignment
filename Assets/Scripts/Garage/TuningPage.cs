using System.Collections.Generic;
using TMPro;
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
        
        
        private List<Button> upgradeButtons = new();
        private CarTuning selectedCarTuning;
        private Renderer selectedCarBodyRenderer;

        private void Awake()
        {
            spoilerTuningButton.onClick.AddListener(() => CreateUpgradeButtons(selectedCarTuning.SpoilerTuning));
            wheelTuningButton.onClick.AddListener(() => CreateUpgradeButtons(selectedCarTuning.WheelTuning));
            colorTuningButton.onClick.AddListener(() => CreateUpgradeButtons(selectedCarTuning.ColorTuning));
        }

        private void DestroyUpgradeButtons()
        {
            foreach (var upgradeButton in upgradeButtons)
            {
                Destroy(upgradeButton.gameObject);
            }
            upgradeButtons.Clear();
        }

        private void CreateUpgradeButtons(object tuning)
        {
            DestroyUpgradeButtons();
            buyButton.interactable = false;
            buyButton.gameObject.SetActive(true);
            
            switch (tuning)
            {
                case Tuning<GameObject> tuningGameObject:
                    for (var index = 0; index < tuningGameObject.PriceObjectPairs.Length; index++)
                    {
                        var pair = tuningGameObject.PriceObjectPairs[index];
                        var upgradeButton = CreateUpgradeButton(pair);

                        if (pair.Thumbnail != null)
                            upgradeButton.image.sprite = pair.Thumbnail;

                        var upgradeIndex = index;
                        upgradeButton.onClick.AddListener(() =>
                        {
                            var upgrade = selectedCarTuning.SpoilerTuning.SelectedGameObject;
                            if (upgrade != null)
                                upgrade.SetActive(false);
                            
                            upgrade = tuningGameObject.PriceObjectPairs[upgradeIndex].Upgrade;
                            if (upgrade != null)
                                upgrade.SetActive(true);
                            
                            selectedCarTuning.SpoilerTuning.Selected = upgradeIndex;

                            buyButton.interactable = pair.Price != 0 && pair.Price <= Player.Cash;
                        });
                    }

                    break;
                case Tuning<Material> tuningMaterial:
                    for (var index = 0; index < tuningMaterial.PriceObjectPairs.Length; index++)
                    {
                        var pair = tuningMaterial.PriceObjectPairs[index];
                        var upgradeButton = CreateUpgradeButton(pair);

                        upgradeButton.image.color = pair.Upgrade.color;

                        var upgradeIndex = index;
                        upgradeButton.onClick.AddListener(() =>
                        {
                            var materials = selectedCarBodyRenderer.materials;
                            if (materials.Length > 1) //TODO::Rearrange materials
                                materials[1] = pair.Upgrade;
                            else
                                materials[0] = pair.Upgrade;

                            selectedCarBodyRenderer.materials = materials;
                            
                            selectedCarTuning.ColorTuning.Selected = upgradeIndex;
                            
                            buyButton.interactable = pair.Price != 0 && pair.Price <= Player.Cash;
                        });
                    }
                    
                    break;
            }
        }

        private Button CreateUpgradeButton<T>(PriceUpgradePair<T> pair)
        {
            var upgradeButton = Instantiate(upgradeButtonPrefab, categoryGameObject);
            upgradeButton.GetComponentInChildren<TMP_Text>()
                .SetText(pair.Price == 0 ? "Owned" : pair.Price.ToString());
            upgradeButton.gameObject.SetActive(true);
            upgradeButtons.Add(upgradeButton);
            return upgradeButton;
        }


        public void Setup(GameObject carInstance)
        {
            selectedCarTuning = carInstance.GetComponent<CarTuning>();
            selectedCarBodyRenderer = selectedCarTuning.ColorTuning.CarBody.GetComponent<Renderer>();
            gameObject.SetActive(true);
            if (selectedCarTuning.HasTuning(selectedCarTuning.SpoilerTuning)) spoilerTuningButton.gameObject.SetActive(true);
            if (selectedCarTuning.HasTuning(selectedCarTuning.WheelTuning)) wheelTuningButton.gameObject.SetActive(true);
            if (selectedCarTuning.HasTuning(selectedCarTuning.ColorTuning)) colorTuningButton.gameObject.SetActive(true);
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