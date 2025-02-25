using UnityEngine;

public class StudentManager : MonoBehaviour
{
    [SerializeField] private GameObject studentPrefab; // Şimdilik tek prefab kullanalım
    [SerializeField] private Sprite[] maleStudentSprites;   // Erkek öğrenci sprite'ları
    [SerializeField] private Sprite[] femaleStudentSprites; // Kız öğrenci sprite'ları
    [SerializeField] private float yPosition = 0f;
    
    private StudentMovement currentStudent;
    private System.Action onStudentArrived; // Callback için
    
    public void SpawnNewStudent(StudentType type, System.Action onArrived)
    {
        // Eğer mevcut öğrenci varsa, onu gönder
        if (currentStudent != null)
        {
            currentStudent.LeaveScene();
        }
        
        // Yeni öğrenciyi spawn et
        Vector3 spawnPosition = new Vector3(-10f, yPosition, 0);
        GameObject newStudent = Instantiate(studentPrefab, spawnPosition, Quaternion.identity);
        currentStudent = newStudent.GetComponent<StudentMovement>();
        
        // Rastgele cinsiyet seç (50% şans)
        bool isMale = Random.value > 0.5f;
        
        // Sprite'ı ayarla
        SpriteRenderer spriteRenderer = newStudent.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            Sprite[] selectedSpriteArray = isMale ? maleStudentSprites : femaleStudentSprites;
            if (selectedSpriteArray != null && selectedSpriteArray.Length > 0)
            {
                int randomIndex = Random.Range(0, selectedSpriteArray.Length);
                spriteRenderer.sprite = selectedSpriteArray[randomIndex];
            }
        }
        
        // Varış event'ini ayarla
        onStudentArrived = onArrived;
        currentStudent.onArrived.AddListener(() => {
            onStudentArrived?.Invoke();
        });
    }
} 