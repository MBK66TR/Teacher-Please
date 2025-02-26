using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class DailyExpenseManager : MonoBehaviour
{
    [SerializeField] private GameObject expensePanel;
    [SerializeField] private TextMeshProUGUI currentMoneyText;
    [SerializeField] private TextMeshProUGUI salaryText;
    [SerializeField] private TextMeshProUGUI remainingMoneyText;
    [SerializeField] private Button confirmButton;
    [SerializeField] private GameObject mainPanel;
    
    [Header("Expense Items")]
    [SerializeField] private ExpenseItem[] expenseItems;
    
    private GameRules gameRules;
    private float remainingMoney;
    private Dictionary<ExpenseType, int> missedExpenseCounts;
    private const int MAX_MISSED_EXPENSES = 3;
    
    [System.Serializable]
    public class ExpenseItem
    {
        public ExpenseType type;
        public Toggle toggle;
        public TextMeshProUGUI costText;
        public TextMeshProUGUI descriptionText;
    }
    
    void Start()
    {
        gameRules = FindObjectOfType<GameRules>();
        
        // Sözlüğü başlat
        missedExpenseCounts = new Dictionary<ExpenseType, int>();
        foreach (ExpenseType type in System.Enum.GetValues(typeof(ExpenseType)))
        {
            missedExpenseCounts[type] = 0;
        }
        
        InitializeExpenses();
        confirmButton.onClick.AddListener(ConfirmExpenses);
    }
    
    void InitializeExpenses()
    {
        var costs = gameRules.GetExpenseCosts();
        
        foreach (var item in expenseItems)
        {
            item.toggle.onValueChanged.AddListener((isOn) => UpdateExpenses());
            item.costText.text = $"-{costs[item.type]:F0} TL";
            UpdateExpenseItemUI(item);
        }
        
        UpdateExpenses();
    }

    void UpdateExpenseItemUI(ExpenseItem item)
    {
        int missedCount = missedExpenseCounts[item.type];
        
        // Metin rengini güncelle
        if (missedCount >= 2)
        {
            item.descriptionText.color = Color.red;
            item.descriptionText.text = $"{GetExpenseDescription(item.type)} (Uyarı: {missedCount} gündür ödenmedi!)";
        }
        else
        {
            item.descriptionText.color = Color.black;
            item.descriptionText.text = GetExpenseDescription(item.type);
        }
    }
    
    void UpdateExpenses()
    {
        float totalExpenses = 0f;
        var costs = gameRules.GetExpenseCosts();
        
        foreach (var item in expenseItems)
        {
            if (item.toggle.isOn)
            {
                totalExpenses += costs[item.type];
            }
        }
        
        remainingMoney = gameRules.GetDailySalary() - totalExpenses;
        UpdateUI();
    }
    
    void UpdateUI()
    {
        currentMoneyText.text = $"Mevcut Para: {gameRules.GetCurrentMoney():F0} TL";
        salaryText.text = $"Günlük Maaş: {gameRules.GetDailySalary():F0} TL";
        remainingMoneyText.text = $"Kalan: {remainingMoney:F0} TL";
        
        // Eğer bütçe eksideyse kırmızı yap ve butonu devre dışı bırak
        Color textColor = remainingMoney < 0 ? Color.red : Color.black;
        remainingMoneyText.color = textColor;
        confirmButton.interactable = remainingMoney >= 0;
    }

    string GetExpenseDescription(ExpenseType type)
    {
        switch (type)
        {
            case ExpenseType.Rent: return "Ev Kirası";
            case ExpenseType.Food: return "Yemek Giderleri";
            case ExpenseType.Bills: return "Su, Elektrik, İnternet";
            case ExpenseType.RoomExpenses: return "Oda Bakım ve Temizlik";
            default: return "";
        }
    }
    
    public void ConfirmExpenses()
    {
        Dictionary<ExpenseType, bool> selectedExpenses = new Dictionary<ExpenseType, bool>();
        
        foreach (var item in expenseItems)
        {
            selectedExpenses[item.type] = item.toggle.isOn;
            
            if (!item.toggle.isOn)
            {
                missedExpenseCounts[item.type]++;
                
                if (missedExpenseCounts[item.type] >= MAX_MISSED_EXPENSES)
                {
                    string expenseName = GetExpenseDescription(item.type);
                    gameRules.GameOver($"{expenseName} ödemesi 3 gün üst üste yapılmadı!");
                    return;
                }
            }
            else
            {
                missedExpenseCounts[item.type] = 0;
            }

            // Her masraf için UI'ı güncelle
            UpdateExpenseItemUI(item);
        }
        
        gameRules.ProcessDailyExpenses(selectedExpenses);
        expensePanel.SetActive(false);
        mainPanel.SetActive(true);
    }
}