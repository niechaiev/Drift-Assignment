using UnityEngine;
using UnityEngine.UI;

namespace Garage
{
    public class LevelSelectPage : Page
    {
        [SerializeField] private Button[] levelButtons;
        [SerializeField] private SelectModePage selectModePage;
        
        private void Awake()
        {
            for (var i = 0; i < levelButtons.Length; i++)
            {
                var sceneNumber = i;
                levelButtons[i].onClick.AddListener(() =>
                {
                    gameObject.SetActive(false);
                    selectModePage.Setup("Level " + sceneNumber);
                });
            }
        }
    }
}