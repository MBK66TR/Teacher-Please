using UnityEngine;

public class StudentManager : MonoBehaviour
{
    [SerializeField] private GameObject studentPrefab; // Şimdilik tek prefab kullanalım
    [SerializeField] private float spawnPositionX = 10f;
    [SerializeField] private float stopPositionX = -2f;
    [SerializeField] private float yPosition = 0f;
    
    private StudentMovement currentStudent;
    
    public void SpawnNewStudent(StudentType type)
    {
        // Eğer mevcut öğrenci varsa, onu gönder
        if (currentStudent != null)
        {
            currentStudent.LeaveScene();
        }
        
        // Yeni öğrenciyi spawn et
        Vector3 spawnPosition = new Vector3(spawnPositionX, yPosition, 0);
        GameObject newStudent = Instantiate(studentPrefab, spawnPosition, Quaternion.identity);
        currentStudent = newStudent.GetComponent<StudentMovement>();
    }
} 