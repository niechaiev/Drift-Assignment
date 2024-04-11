using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Garage
{
    public class SelectModePage : Page
    {
        [SerializeField] private Button soloButton;
        [SerializeField] private Button onlineButton;
    
        public void Setup(string selectedLevel)
        {
            gameObject.SetActive(true);
            soloButton.onClick.AddListener(() =>
            {
                LoadScene(false, selectedLevel);
            });        
            onlineButton.onClick.AddListener(() =>
            {
                LoadScene(true, selectedLevel);
            });
        }

        private void LoadScene(bool isOnline, string selectedLevel)
        {
            Player.Instance.IsOnline = isOnline;
            SceneManager.LoadScene(selectedLevel);
        }
    }
}
