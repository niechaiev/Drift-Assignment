using UnityEngine;

namespace Garage
{
    public abstract class Page : MonoBehaviour
    {
        [SerializeField] protected Header header; 
    
        protected virtual void OnEnable()
        {
            header.ShowButtonBack(true);
            header.BackButton.onClick.AddListener(Close);
        }

        protected virtual void OnDisable()
        {
            header.ShowButtonBack(false);
            header.BackButton.onClick.RemoveListener(Close);
        }
    
        private void Close()
        {
            gameObject.SetActive(false);
        }
    }
}