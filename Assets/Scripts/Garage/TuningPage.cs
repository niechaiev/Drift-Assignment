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
        [SerializeField] private Button addonButtonPrefab;
        
        private List<Button> addonButtons = new();
        private CarTuning selectedCarTuning;

        private void Awake()
        {
            spoilerTuningButton.onClick.AddListener(() => CreateAddonButtons(selectedCarTuning.SpoilerTuning));
            wheelTuningButton.onClick.AddListener(() => CreateAddonButtons(selectedCarTuning.WheelTuning));
            colorTuningButton.onClick.AddListener(() => CreateAddonButtons(selectedCarTuning.ColorTuning));
        }

        private void DestroyAddonButtons()
        {
            foreach (var addonButton in addonButtons)
            {
                Destroy(addonButton.gameObject);
            }
            addonButtons.Clear();
        }

        private void CreateAddonButtons(object tuning)
        {
            DestroyAddonButtons();
            
            switch (tuning)
            {
                case Tuning<GameObject> tuningGameObject:
                    foreach (var pair in tuningGameObject.PriceObjectPairs)
                    {
                        var addonButton = Instantiate(addonButtonPrefab, categoryGameObject);
                        addonButton.GetComponentInChildren<TMP_Text>().SetText(pair.Price.ToString());
                        addonButton.gameObject.SetActive(true);
                        addonButtons.Add(addonButton);
                    }
                    break;
                case Tuning<Material> tuningMaterial:
                    foreach (var pair in tuningMaterial.PriceObjectPairs)
                    {
                        var addonButton = Instantiate(addonButtonPrefab, categoryGameObject);
                        addonButton.GetComponentInChildren<TMP_Text>().SetText(pair.Price.ToString());
                        addonButton.image.color = pair.Addon.color;
                        addonButton.gameObject.SetActive(true);
                        addonButtons.Add(addonButton);
                    }
                    break;
            }
        }
        
        

        public void Setup(CarTuning carTuning)
        {
            gameObject.SetActive(true);
            if (carTuning.HasTuning(carTuning.SpoilerTuning)) spoilerTuningButton.gameObject.SetActive(true);
            if (carTuning.HasTuning(carTuning.WheelTuning)) wheelTuningButton.gameObject.SetActive(true);
            if (carTuning.HasTuning(carTuning.ColorTuning)) colorTuningButton.gameObject.SetActive(true);
            selectedCarTuning = carTuning;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            spoilerTuningButton.gameObject.SetActive(false);
            wheelTuningButton.gameObject.SetActive(false);
            colorTuningButton.gameObject.SetActive(false);
            DestroyAddonButtons();
        }
    }
}