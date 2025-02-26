using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class TableBackground : MonoBehaviour
{
    [SerializeField] private Sprite tableSprite;
    [SerializeField] private Vector2 tableSize = new Vector2(800f, 600f);
    [SerializeField] private Vector2 tablePosition = Vector2.zero;
    
    private void Awake()
    {
        // Canvas ayarlarını yap
        Canvas canvas = GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 0; // En arkada
        
        // Masa görseli için Image oluştur
        GameObject tableObj = new GameObject("TableImage");
        tableObj.transform.SetParent(transform);
        
        Image tableImage = tableObj.AddComponent<Image>();
        tableImage.sprite = tableSprite;
        
        // Rect Transform ayarları
        RectTransform rectTransform = tableObj.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = tablePosition;
        rectTransform.sizeDelta = tableSize;
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
    }
} 