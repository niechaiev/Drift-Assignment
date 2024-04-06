using UnityEngine;

public class Page : MonoBehaviour
{
    [SerializeField] private Header header; 
    
    protected void OnEnable()
    {
        header.ShowButtonBack(true);
        header.BackButton.onClick.AddListener(Close);
    }

    protected void OnDisable()
    {
        header.ShowButtonBack(false);
        header.BackButton.onClick.RemoveListener(Close);
    }
    
    private void Close()
    {
        gameObject.SetActive(false);
    }
}