using UnityEngine;
using UnityEngine.Events;

public class StudentMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private const float STOP_POSITION_X = 0.15f; // Durma pozisyonu
    private bool isMoving = true;
    
    public UnityEvent onArrived; // Öğrenci vardığında tetiklenecek event
    
    void Start()
    {
        // Başlangıçta sol tarafta olacak
        transform.position = new Vector3(-10f, transform.position.y, transform.position.z);
        
        if(onArrived == null)
            onArrived = new UnityEvent();
    }
    
    void Update()
    {
        if (isMoving)
        {
            // Soldan sağa hareket
            float newX = Mathf.MoveTowards(transform.position.x, STOP_POSITION_X, moveSpeed * Time.deltaTime);
            transform.position = new Vector3(newX, transform.position.y, transform.position.z);
            
            // Durma pozisyonuna gelince dur ve event'i tetikle
            if (transform.position.x >= STOP_POSITION_X)
            {
                isMoving = false;
                onArrived.Invoke();
            }
        }
    }
    
    public void LeaveScene()
    {
        Destroy(gameObject);
    }
} 