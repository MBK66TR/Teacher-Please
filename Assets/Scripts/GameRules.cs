using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
    [SerializeField] private float dailySalary = 100f;  // Günlük maaş
    private float currentMoney = 200f;  // Başlangıç parası (2 günlük masraf karşılığı)
    
    // Sabit masraflar
    private Dictionary<ExpenseType, float> expenseCosts = new Dictionary<ExpenseType, float>()
    {
        { ExpenseType.Rent, 45f },      // Kira: 45₺
        { ExpenseType.Food, 35f },      // Yemek: 35₺
        { ExpenseType.Bills, 20f },     // Faturalar: 20₺
        { ExpenseType.RoomExpenses, 20f }  // Oda giderleri: 20₺
    };

    private Dictionary<ExpenseType, bool> activeExpenses = new Dictionary<ExpenseType, bool>();

    // Para kazanma oranları
    private const float PRO_STUDENT_MONEY = 10f;      // Öğrenci yanlısı karar: 10₺
    private const float PRO_ADMIN_MONEY = 20f;       // Yönetim yanlısı karar: 20₺
    
    // Bar değişim oranları
    private const float PRO_STUDENT_BAR_CHANGE = 1f;     // Öğrenci yanlısı karar için bar değişimi
    private const float PRO_ADMIN_BAR_CHANGE = 1f;       // Yönetim yanlısı karar için bar değişimi

    [SerializeField] private GameObject mainPanel; // Inspector'da atanacak
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI gameOverText;

    // UI Referansları
    [Header("Bar UI Elemanları")]
    [SerializeField] private Image studentBarFill;        // Öğrenci memnuniyeti bar fill
    [SerializeField] private Image adminBarFill;          // Yönetim güveni bar fill

    // Start is called before the first frame update
    void Start()
    {
        InitializeGame();
        SetDailyRequestCount();
        UpdateUI(); // UI'ı başlangıçta güncelle
    }

    void InitializeGame()
    {
        studentSatisfaction = 50f;
        administrationTrust = 50f;
        currentDay = 1;
        isGameOver = false;
        currentMoney = 200f; // Başlangıç parası (2 günlük masraf karşılığı)

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

    // UI güncelleme fonksiyonu
    private void UpdateUI()
    {
        Debug.Log($"Öğrenci: {studentSatisfaction}, Yönetim: {administrationTrust}"); // Debug için

        // Her bar için ayrı değer
        if (studentBarFill != null)
        {
            float studentValue = studentSatisfaction / 100f;
            studentBarFill.fillAmount = studentValue;
            Debug.Log($"Öğrenci Bar Değeri: {studentValue}");
        }

        if (adminBarFill != null)
        {
            float adminValue = administrationTrust / 100f;
            adminBarFill.fillAmount = adminValue;
            Debug.Log($"Yönetim Bar Değeri: {adminValue}");
        }
    }

    public void HandleRequest(bool isProStudent)
    {
        if (isGameOver) return;

        float moneyGain = isProStudent ? PRO_STUDENT_MONEY : PRO_ADMIN_MONEY;
        currentMoney += moneyGain;

        // Öğrenci yanlısı karar
        if (isProStudent)
        {
            studentSatisfaction += Random.Range(5f, 8f);    // Öğrenci memnuniyeti artar (10-20'den 5-8'e)
            administrationTrust -= Random.Range(6f, 10f);   // Yönetim güveni düşer (15-25'ten 6-10'a)
        }
        // Yönetim yanlısı karar
        else
        {
            studentSatisfaction -= Random.Range(6f, 10f);   // Öğrenci memnuniyeti düşer (15-25'ten 6-10'a)
            administrationTrust += Random.Range(5f, 8f);    // Yönetim güveni artar (10-20'den 5-8'e)
        }

        // Değerleri sınırla
        studentSatisfaction = Mathf.Clamp(studentSatisfaction, 0f, 100f);
        administrationTrust = Mathf.Clamp(administrationTrust, 0f, 100f);

        UpdateUI(); // UI'ı güncelle
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
        
        // Yeni güne başlarken maaşı ekle
        currentMoney += dailySalary;
        Debug.Log($"Yeni gün başladı. Maaş eklendi. Toplam para: {currentMoney}");
        
        // Sadece expense panelini aç
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

    public void ProcessDailyExpenses()
    {
        // Artık sadece UI'ı kapatıyoruz
        expenseManager.gameObject.SetActive(false);
        mainPanel.SetActive(true);
    }

    // Yeni getter metodları
    public float GetDailySalary() => dailySalary;
    public float GetCurrentMoney() => currentMoney;
    public Dictionary<ExpenseType, float> GetExpenseCosts() => expenseCosts;

    // Update is called once per frame
    void Update()
    {
        UpdateUI(); // Her frame'de UI'ı güncelle
    }
}

public enum ExpenseType
{
    Rent,           // Kira
    Food,           // Yemek
    Bills,          // Faturalar
    RoomExpenses    // Oda giderleri
}
