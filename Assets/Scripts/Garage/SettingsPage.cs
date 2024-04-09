using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Garage
{
    public class SettingsPage : Page
    {
        [SerializeField] private TMP_InputField nameText;
        [SerializeField] private Button eraseDataButton;
        [SerializeField] private Button saveButton;

        private void Awake()
        {
            saveButton.onClick.AddListener(Save);
            eraseDataButton.onClick.AddListener(() =>
            {
                Player.Instance.EraseData();
                Close();
            });
        }

        private void Save()
        {
            Player.Instance.Name = nameText.text;
            Close();
        }

        private void Close()
        {
            gameObject.SetActive(false);
            header.BackButton.onClick?.Invoke();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            nameText.text = Player.Instance.Name;
        }
    }
}
