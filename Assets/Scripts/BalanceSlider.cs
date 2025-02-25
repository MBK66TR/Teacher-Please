using UnityEngine;
using UnityEngine.UI;

public class BalanceSlider : MonoBehaviour
{
    [SerializeField] private Image fillBar; // Dolum görseli
    [SerializeField] private Image backgroundBar; // Bar arkası
    
    private GameRules gameRules;
    
    void Start()
    {
        gameRules = FindObjectOfType<GameRules>();
        
        // Fill bar'ın ayarlarını yap
        fillBar.type = Image.Type.Filled;
        fillBar.fillMethod = Image.FillMethod.Horizontal;
        fillBar.fillOrigin = (int)Image.OriginHorizontal.Left;
    }

    void Update()
    {
        float studentValue = gameRules.GetStudentSatisfaction();
        float adminValue = gameRules.GetAdministrationTrust();
        
        // Bar'ın doluluk oranını hesapla
        float normalizedBalance = (studentValue - adminValue + 100f) / 200f;
        fillBar.fillAmount = normalizedBalance;
    }
} 