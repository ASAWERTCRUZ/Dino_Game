using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private FixedJoystick _joystick;
    [SerializeField] private Animator _animator;
    [SerializeField] private Camera cam;
    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private float _runSpeed = 6f; // Velocidad al correr

    // Configuración de la cámara
    [Header("Camera Settings")]
    [SerializeField] private float sensitivityX = 0.1f;
    [SerializeField] private float sensitivityY = 0.1f;
    [SerializeField] private float minY = -45f;
    [SerializeField] private float maxY = 45f;

    private float rotationY = 0f;
    private bool isRunning = false;

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void Update()
    {
        RotateCamera();
    }

    void MovePlayer()
    {
        Vector3 move = new Vector3(_joystick.Horizontal, 0, _joystick.Vertical);
        move = Vector3.ClampMagnitude(move, 1);
        float currentSpeed = isRunning ? _runSpeed : _moveSpeed; // Usa _runSpeed si se está corriendo
        transform.Translate(move * currentSpeed * Time.deltaTime);
    }

    void RotateCamera()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);

            if (touch.position.x > Screen.width / 2)
            {
                if (touch.phase == TouchPhase.Moved)
                {
                    float rotationX = touch.deltaPosition.x * sensitivityX;
                    rotationY -= touch.deltaPosition.y * sensitivityY;
                    rotationY = Mathf.Clamp(rotationY, minY, maxY);
                    cam.transform.localEulerAngles = new Vector3(rotationY, 0f, 0f);
                    transform.Rotate(Vector3.up * rotationX);
                }
            }
        }
    }

    // Método para manejar cuando el botón de correr es presionado
    public void StartRunning()
    {
        isRunning = true;
    }

    // Método para manejar cuando el botón de correr es soltado
    public void StopRunning()
    {
        isRunning = false;
    }
}
