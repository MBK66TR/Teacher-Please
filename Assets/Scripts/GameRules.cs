using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRules : MonoBehaviour
{
    // Oyuncu durumu
    [SerializeField] private float studentSatisfaction = 50f; // Öğrenci memnuniyeti (0-100)
    [SerializeField] private float administrationTrust = 50f; // Yönetim güveni (0-100)
    
    // Sınırlar
    private const float MIN_THRESHOLD = 20f; // Bu değerin altına düşerse kovulma
    private const float MAX_THRESHOLD = 80f; // Bu değerin üstüne çıkarsa kovulma
    
    // Günlük değişkenler
    private int currentDay = 1;
    private int requestsHandledToday = 0;
    private const int MAX_REQUESTS_PER_DAY = 5;

    private bool isGameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        InitializeGame();
    }

    void InitializeGame()
    {
        studentSatisfaction = 50f;
        administrationTrust = 50f;
        currentDay = 1;
        isGameOver = false;
    }

    public void HandleRequest(bool isProStudent)
    {
        if (isGameOver) return;

        // Öğrenci yanlısı karar
        if (isProStudent)
        {
            studentSatisfaction += Random.Range(5f, 10f);
            administrationTrust -= Random.Range(3f, 8f);
        }
        // Yönetim yanlısı karar
        else
        {
            studentSatisfaction -= Random.Range(3f, 8f);
            administrationTrust += Random.Range(5f, 10f);
        }

        // Değerleri sınırla
        studentSatisfaction = Mathf.Clamp(studentSatisfaction, 0f, 100f);
        administrationTrust = Mathf.Clamp(administrationTrust, 0f, 100f);

        CheckGameOver();
        requestsHandledToday++;
        
        if (requestsHandledToday >= MAX_REQUESTS_PER_DAY)
        {
            EndDay();
        }
    }

    private void CheckGameOver()
    {
        if (studentSatisfaction < MIN_THRESHOLD || administrationTrust < MIN_THRESHOLD ||
            studentSatisfaction > MAX_THRESHOLD || administrationTrust > MAX_THRESHOLD)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        isGameOver = true;
        Debug.Log($"Oyun Bitti! Gün: {currentDay}");
        // Burada game over ekranını tetikleyebilirsiniz
    }

    private void EndDay()
    {
        currentDay++;
        requestsHandledToday = 0;
        // Günün sonunda ekstra olaylar veya değerlendirmeler eklenebilir
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
