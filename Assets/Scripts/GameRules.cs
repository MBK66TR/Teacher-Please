using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameRules : MonoBehaviour
{
    // Oyuncu durumu
    [SerializeField] private float studentSatisfaction = 50f; // Öğrenci memnuniyeti (0-100)
    [SerializeField] private float administrationTrust = 50f; // Yönetim güveni (0-100)
    [SerializeField] private GameObject expenseManager;
    
    // Sınırlar
    private const float MIN_THRESHOLD = 10f; // Bu değerin altına düşerse kovulma
    private const float MAX_THRESHOLD = 90f; // Bu değerin üstüne çıkarsa kovulma
    
    // Günlük değişkenler
    private int currentDay = 1;
    private int requestsHandledToday = 0;
    private const int MIN_REQUESTS_PER_DAY = 5;
    private const int MAX_REQUESTS_PER_DAY = 7;
    private int todayMaxRequests;

    private bool isGameOver = false;

    // Ekonomi sistemi
    [SerializeField] private float dailySalary = 100f;
    private float currentMoney = 0f;
    
    // Sabit masraflar
    private Dictionary<ExpenseType, float> expenseCosts = new Dictionary<ExpenseType, float>()
    {
        { ExpenseType.Rent, 80f },     // Kira: 80₺
        { ExpenseType.Food, 50f },     // Yemek: 50₺
        { ExpenseType.Bills, 30f },    // Faturalar: 30₺
        { ExpenseType.RoomExpenses, 40f } // Oda giderleri: 40₺
    };

    private Dictionary<ExpenseType, bool> activeExpenses = new Dictionary<ExpenseType, bool>();

    // Para kazanma oranları
    private const float PRO_STUDENT_MONEY = 50f;      // Öğrenci yanlısı karar: 50₺
    private const float PRO_ADMIN_MONEY = 100f;       // Yönetim yanlısı karar: 100₺
    
    // Bar değişim oranları
    private const float PRO_STUDENT_BAR_CHANGE = 0.5f;  // Öğrenci yanlısı karar için bar değişimi
    private const float PRO_ADMIN_BAR_CHANGE = 1.5f;    // Yönetim yanlısı karar için bar değişimi

    [SerializeField] private GameObject mainPanel; // Inspector'da atanacak
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI gameOverText;

    // Start is called before the first frame update
    void Start()
    {
        InitializeGame();
        SetDailyRequestCount();
    }

    void InitializeGame()
    {
        studentSatisfaction = 50f;
        administrationTrust = 50f;
        currentDay = 1;
        isGameOver = false;
        currentMoney = dailySalary; // Başlangıç parası

        // Masrafları başlat
        foreach (ExpenseType expense in System.Enum.GetValues(typeof(ExpenseType)))
        {
            activeExpenses[expense] = true; // Başlangıçta hepsi aktif
        }
    }

    private void SetDailyRequestCount()
    {
        todayMaxRequests = Random.Range(MIN_REQUESTS_PER_DAY, MAX_REQUESTS_PER_DAY + 1);
    }

    public void HandleRequest(bool isProStudent)
    {
        if (isGameOver) return;

        float moneyGain = isProStudent ? PRO_STUDENT_MONEY : PRO_ADMIN_MONEY;
        currentMoney += moneyGain;

        // Öğrenci yanlısı karar
        if (isProStudent)
        {
            studentSatisfaction += Random.Range(5f, 10f) * PRO_STUDENT_BAR_CHANGE;
            administrationTrust -= Random.Range(3f, 8f) * PRO_STUDENT_BAR_CHANGE;
        }
        // Yönetim yanlısı karar
        else
        {
            studentSatisfaction -= Random.Range(3f, 8f) * PRO_ADMIN_BAR_CHANGE;
            administrationTrust += Random.Range(5f, 10f) * PRO_ADMIN_BAR_CHANGE;
        }

        // Değerleri sınırla
        studentSatisfaction = Mathf.Clamp(studentSatisfaction, 0f, 100f);
        administrationTrust = Mathf.Clamp(administrationTrust, 0f, 100f);

        CheckGameOver();
        requestsHandledToday++;
        
        if (requestsHandledToday >= todayMaxRequests)
        {
            EndDay();
        }
    }

    private void CheckGameOver()
    {
        string message = "";
        if (studentSatisfaction < MIN_THRESHOLD)
            message = "Öğrenci memnuniyeti çok düşük!";
        else if (administrationTrust < MIN_THRESHOLD)
            message = "Yönetim güveni çok düşük!";
        else if (studentSatisfaction > MAX_THRESHOLD)
            message = "Öğrenci memnuniyeti çok yüksek!";
        else if (administrationTrust > MAX_THRESHOLD)
            message = "Yönetim güveni çok yüksek!";
            
        if (!string.IsNullOrEmpty(message))
            GameOver(message);
    }

    public void GameOver(string message)
    {
        isGameOver = true;
        Debug.Log($"Oyun Bitti! Gün: {currentDay} - Sebep: {message}");
        
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
            
        if (gameOverText != null)
            gameOverText.text = $"Oyun Bitti!\nGün: {currentDay}\nSebep: {message}";
            
        mainPanel.SetActive(false);
    }

    private void EndDay()
    {
        currentDay++;
        requestsHandledToday = 0;
        SetDailyRequestCount();
        
        // Panelleri aç/kapa
        mainPanel.SetActive(false);
        expenseManager.gameObject.SetActive(true);
    }

    // Getter metodları
    public float GetStudentSatisfaction() => studentSatisfaction;
    public float GetAdministrationTrust() => administrationTrust;
    public int GetCurrentDay() => currentDay;
    public bool IsGameOver() => isGameOver;

    public void LoadGame(float studentSat, float adminTrust, int day, bool gameOver)
    {
        studentSatisfaction = studentSat;
        administrationTrust = adminTrust;
        currentDay = day;
        requestsHandledToday = 0;
        isGameOver = gameOver;
    }

    public void ProcessDailyExpenses(Dictionary<ExpenseType, bool> selectedExpenses)
    {
        float totalExpenses = 0f;
        activeExpenses = selectedExpenses;
        
        foreach (var expense in expenseCosts)
        {
            if (activeExpenses[expense.Key])
            {
                totalExpenses += expense.Value;
            }
        }
        
        // Maaşı ekle ve masrafları düş
        currentMoney += dailySalary;
        currentMoney -= totalExpenses;
        
        // Para bitti mi kontrol et
        if (currentMoney < 0)
        {
            GameOver("Para bitti!");
            return;
        }
        
        // Masrafların etkilerini uygula
        ApplyExpenseEffects();
    }

    private void ApplyExpenseEffects()
    {
        // Seçili olmayan masrafların olumsuz etkileri
        if (!activeExpenses[ExpenseType.Food])
        {
            studentSatisfaction -= 10f;
            administrationTrust -= 5f;
        }

        if (!activeExpenses[ExpenseType.Rent])
        {
            studentSatisfaction -= 15f;
            administrationTrust -= 10f;
        }
        
        // Değerleri sınırla
        studentSatisfaction = Mathf.Clamp(studentSatisfaction, 0f, 100f);
        administrationTrust = Mathf.Clamp(administrationTrust, 0f, 100f);
    }

    // Yeni getter metodları
    public float GetDailySalary() => dailySalary;
    public float GetCurrentMoney() => currentMoney;
    public Dictionary<ExpenseType, float> GetExpenseCosts() => expenseCosts;

    // Update is called once per frame
    void Update()
    {
        
    }
}

public enum ExpenseType
{
    Rent,           // Kira
    Food,           // Yemek
    Bills,          // Faturalar
    RoomExpenses    // Oda giderleri
}
