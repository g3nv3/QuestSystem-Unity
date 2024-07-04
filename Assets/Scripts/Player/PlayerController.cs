using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Main Components")]
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private float _playerMass = 5;
    [SerializeField] private Transform _cameraTransform;
    private Transform _transform;

    [Header("Move")]
    [SerializeField] private bool _canMove = true;
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _rotationSpeed = 15f;
    private Vector3 movementDirection;

    [Header("Jump")]
    [SerializeField] private bool _isGrounded;
    [SerializeField] private float _jumpForce = 15f;
    private float gravityForce = -9.8f;
    private float tempFallingSpeed = -1f;

    private void Start()
    {
        _transform = GetComponent<Transform>();
        _cameraTransform = Camera.main.GetComponent<Transform>();
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Move();
    }
    private void Move()
    {
        _isGrounded = _characterController.isGrounded;
        movementDirection.x = 0f;
        movementDirection.z = 0f;
        if ((Input.GetAxisRaw("Horizontal") != 0f || Input.GetAxisRaw("Vertical") != 0f) && _canMove)
        {
            MovePlayer();
            RotatePlayer();
        }
        CalculateFallingSpeed();
        Jump();
        movementDirection.y = tempFallingSpeed;
        _characterController.Move(movementDirection * Time.deltaTime);
    }

    private void MovePlayer()
    {
        movementDirection.x = Input.GetAxisRaw("Horizontal") * _speed;
        movementDirection.z = Input.GetAxisRaw("Vertical") * _speed;
        movementDirection = Vector3.ClampMagnitude(movementDirection, _speed);
    }

    private void RotatePlayer()
    {
        Quaternion tempCameraRotation = _cameraTransform.rotation;
        _cameraTransform.rotation = Quaternion.Euler(0f, _cameraTransform.eulerAngles.y, 0f);
        movementDirection = _cameraTransform.TransformDirection(movementDirection);
        _cameraTransform.rotation = tempCameraRotation;

        _transform.rotation = Quaternion.Lerp(_transform.rotation, Quaternion.LookRotation(new Vector3(movementDirection.x, 0, movementDirection.z)), _rotationSpeed * Time.deltaTime);
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
            tempFallingSpeed = _jumpForce;
    }

    private void CalculateFallingSpeed()
    {
        if (!_isGrounded)
            tempFallingSpeed += gravityForce * _playerMass * Time.deltaTime;
        else
            tempFallingSpeed = -1f;
    }
}