using UnityEngine;
using System;

public class StudentManager : MonoBehaviour
{
    [SerializeField] private GameObject studentPrefab; // Şimdilik tek prefab kullanalım
    [SerializeField] private Sprite[] maleStudentSprites;   // Erkek öğrenci sprite'ları
    [SerializeField] private Sprite[] femaleStudentSprites; // Kız öğrenci sprite'ları
    [SerializeField] private float yPosition = 0f;
    
    private StudentMovement currentStudent;
    private Action onStudentArrived;
    private Action onStudentLeft;
    private bool isDismissing = false;
    
    public void SpawnNewStudent(StudentType type, Action onArrived)
    {
        // Eğer öğrenci çıkış yapıyorsa bekle
        if (isDismissing)
        {
            Debug.LogWarning("Öğrenci çıkış yaparken yeni öğrenci oluşturulamaz!");
            return;
        }
        
        // Yeni öğrenciyi spawn et
        Vector3 spawnPosition = new Vector3(-10f, yPosition, 0);
        GameObject newStudent = Instantiate(studentPrefab, spawnPosition, Quaternion.identity);
        currentStudent = newStudent.GetComponent<StudentMovement>();
        
        // Rastgele cinsiyet seç (50% şans)
        bool isMale = UnityEngine.Random.value > 0.5f;
        
        // Sprite'ı ayarla
        SpriteRenderer spriteRenderer = newStudent.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            Sprite[] selectedSpriteArray = isMale ? maleStudentSprites : femaleStudentSprites;
            if (selectedSpriteArray != null && selectedSpriteArray.Length > 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, selectedSpriteArray.Length);
                spriteRenderer.sprite = selectedSpriteArray[randomIndex];
            }
        }
        
        // Varış event'ini ayarla
        onStudentArrived = onArrived;
        currentStudent.onArrived.AddListener(() => {
            onStudentArrived?.Invoke();
        });
    }
    
    public void DismissCurrentStudent(Action onLeft)
    {
        if (currentStudent != null && !isDismissing)
        {
            isDismissing = true;
            onStudentLeft = onLeft;
            
            currentStudent.onLeft.AddListener(() => {
                if (currentStudent != null)
                {
                    Destroy(currentStudent.gameObject);
                }
                currentStudent = null;
                isDismissing = false;
                onStudentLeft?.Invoke();
            });
            
            currentStudent.LeaveScene();
        }
        else
        {
            onLeft?.Invoke();
        }
    }
} 