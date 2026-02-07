using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController2D : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float minX = -7f;
    [SerializeField] private float maxX = 7f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameController gameController;

    private float horizontalInput;

    private void Awake()
    {
        if (!ValidateSetup())
        {
            enabled = false;
        }
    }

    private void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
    }

    private void FixedUpdate()
    {
        Vector2 currentPosition = rb.position;
        float nextX = currentPosition.x + (horizontalInput * moveSpeed * Time.fixedDeltaTime);
        nextX = Mathf.Clamp(nextX, minX, maxX);

        rb.MovePosition(new Vector2(nextX, currentPosition.y));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("FallingObject"))
        {
            gameController.GameOver();
        }
    }

    private bool ValidateSetup()
    {
        bool isValid = true;

        if (rb == null)
        {
            Debug.LogError("PlayerController2D: Rigidbody2D reference is not set.", this);
            isValid = false;
        }

        if (gameController == null)
        {
            Debug.LogError("PlayerController2D: GameController reference is not set.", this);
            isValid = false;
        }

        if (minX > maxX)
        {
            Debug.LogError("PlayerController2D: minX must be less than or equal to maxX.", this);
            isValid = false;
        }

        return isValid;
    }
}
