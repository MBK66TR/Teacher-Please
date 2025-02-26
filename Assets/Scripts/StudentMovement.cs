using UnityEngine;
using UnityEngine.Events;

public class StudentMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private const float STOP_POSITION_X = 0.15f; // Durma pozisyonu
    private const float EXIT_POSITION_X = -10f;
    private bool isMoving = true;
    private bool isLeaving = false;
    private SpriteRenderer spriteRenderer;
    
    public UnityEvent onArrived; // Öğrenci vardığında tetiklenecek event
    public UnityEvent onLeft;
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        transform.position = new Vector3(EXIT_POSITION_X, transform.position.y, transform.position.z);
        
        if(onArrived == null)
            onArrived = new UnityEvent();
        if(onLeft == null)
            onLeft = new UnityEvent();
    }
    
    void Update()
    {
        if (isMoving)
        {
            // Sağa doğru hareket
            float newX = Mathf.MoveTowards(transform.position.x, STOP_POSITION_X, moveSpeed * Time.deltaTime);
            transform.position = new Vector3(newX, transform.position.y, transform.position.z);
            
            // Durma pozisyonuna gelince dur ve event'i tetikle
            if (transform.position.x >= STOP_POSITION_X)
            {
                isMoving = false;
                onArrived.Invoke();
            }
        }
        else if (isLeaving)
        {
            // Sola doğru hareket
            float newX = Mathf.MoveTowards(transform.position.x, EXIT_POSITION_X, moveSpeed * Time.deltaTime);
            transform.position = new Vector3(newX, transform.position.y, transform.position.z);
            
            // Çıkış pozisyonuna gelince yok et
            if (transform.position.x <= EXIT_POSITION_X)
            {
                onLeft.Invoke();
                Destroy(gameObject);
            }
        }
    }
    
    public void LeaveScene()
    {
        isLeaving = true;
        isMoving = false;
        
        // Sprite'ı ters çevir
        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = true;
        }
    }
} 