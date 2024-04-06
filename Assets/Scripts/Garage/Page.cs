using UnityEngine;

public class Page : MonoBehaviour
{
    [SerializeField] private Header header; 
    
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