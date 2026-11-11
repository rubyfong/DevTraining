using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    #region Singleton
    public static PlayerController Instance { get; set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            _RB = GetComponent<Rigidbody>();
            _RB.freezeRotation = true;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
    #endregion

    public float MoveSpeed = 6f;
    public float JumpForce = 7f;
    private Rigidbody _RB;

    public Transform Cam;
    public float CameraDistance = 5f;
    public float CameraHeight = 1.6f;
    public float mouseSensitivity = 0.12f;
    private float _Yaw;
    private float _Pitch;

    private void Update()
    {
        if (GameManager.Instance.EnableControls)
        {
            Look();
            Jump();
        }
        Reset();
        Quit();
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.EnableControls)
        {
            Move();
        }
    }

    private void LateUpdate()
    {
        PositionCamera();
    }

    private void Look()
    {
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();
        _Yaw += mouseDelta.x * mouseSensitivity;
        _Pitch -= mouseDelta.y * mouseSensitivity;
        _Pitch = Mathf.Clamp(_Pitch, -40f, 70f);

        transform.rotation = Quaternion.Euler(0f, _Yaw, 0f);
    }

    private Vector3 GetMoveInput()
    {
        var kb = Keyboard.current;

        float x = 0f, z = 0f;
        if (kb.wKey.isPressed) z += 1f;
        if (kb.sKey.isPressed) z -= 1f;
        if (kb.dKey.isPressed) x += 1f;
        if (kb.aKey.isPressed) x -= 1f;
        return new Vector3(x, 0f, z).normalized;
    }

    private void Jump()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame && IsGrounded())
        {
            _RB.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
        }
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }

    private void PositionCamera()
    {
        Quaternion camRotation = Quaternion.Euler(_Pitch, _Yaw, 0f);
        Vector3 focusPoint = transform.position + Vector3.up * CameraHeight;

        Cam.position = focusPoint - camRotation * Vector3.forward * CameraDistance;
        Cam.rotation = camRotation;
    }

    private void Move()
    {
        Vector3 input = GetMoveInput();
        Vector3 moveDir = transform.forward * input.z + transform.right * input.x;

        Vector3 velocity = moveDir * MoveSpeed;
        velocity.y = _RB.linearVelocity.y;
        _RB.linearVelocity = velocity;
    }

    private void Reset()
    {
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene(0);
        }
    }

    private void Quit()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Application.Quit();
        }
    }
}