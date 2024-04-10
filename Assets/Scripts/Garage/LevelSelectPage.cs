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
            for (var i = 0; i < levelButtons.Length; i++)
            {
                var sceneNumber = i;
                levelButtons[i].onClick.AddListener(() =>
                {
                    LoadScene(sceneNumber);
                });
            }
        }

        private void LoadScene(int number)
        {
            SceneManager.LoadScene("Level " + number);
        }
    
    }
}