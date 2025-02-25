using UnityEngine;

public class StudentManager : MonoBehaviour
{
    [SerializeField] private GameObject studentPrefab; // Şimdilik tek prefab kullanalım
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
        
        // Varış event'ini ayarla
        onStudentArrived = onArrived;
        currentStudent.onArrived.AddListener(() => {
            onStudentArrived?.Invoke();
        });
    }
} 