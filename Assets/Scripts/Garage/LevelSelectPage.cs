using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Garage
{
    public class LevelSelectPage : Page
    {
        [SerializeField] private Button[] levelButtons;

        private void Awake()
        {
            levelButtons[0].onClick.AddListener(LoadScene);
        }

        private void LoadScene()
        {
            SceneManager.LoadScene("Level 1");
        }
    
    }
}