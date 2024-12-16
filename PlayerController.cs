using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public Camera playerCamera;
    public float walkSpeed = 6f;
    public float runSpeed = 9f;
    public float jumpPower = 7f;
    public float gravity = 10f;

    public float lookSpeed = 2f;
    public float lookXLimit = 45;

    public float maxStamina = 5f;
    private float currentStamina;
    public float staminaRegenRate = 1f;
    public float staminaRegenDelay = 3f;

    public Slider staminaBar;
    public Text staminaMessage;

    private float staminaRegenTimer = 0f;
    private bool isSprinting = false;

    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;

    public bool canMove = true;
    private bool isDisabled = false;

    private CharacterController characterController;
    private Animator animator;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        currentStamina = maxStamina;
        UpdateStaminaUI();
    }

    void Update()
    {
        if (isDisabled)
            return;

        #region Handles Movement
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        bool isRunning = Input.GetKey(KeyCode.LeftShift) && currentStamina > 0;
        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;

        moveDirection = (forward * curSpeedX) + (right * curSpeedY);
        float currentSpeed = new Vector3(moveDirection.x, 0, moveDirection.z).magnitude;

        if (animator != null)
        {
            animator.SetFloat("Speed", currentSpeed);
        }
        #endregion

        #region Handles Stamina
        if (isRunning)
        {
            isSprinting = true;
            currentStamina -= Time.deltaTime;
            if (currentStamina < 0)
            {
                currentStamina = 0;
                isSprinting = false;
            }
            staminaRegenTimer = 0;
        }
        else
        {
            isSprinting = false;
            staminaRegenTimer += Time.deltaTime;
            if (staminaRegenTimer >= staminaRegenDelay && currentStamina < maxStamina)
            {
                currentStamina += staminaRegenRate * Time.deltaTime;
                if (currentStamina > maxStamina)
                {
                    currentStamina = maxStamina;
                }
            }

            if (currentStamina == maxStamina)
            {
                staminaMessage.enabled = false;
            }
        }

        if (currentStamina <= 0)
        {
            staminaMessage.text = "Stamina Empty!";
            staminaMessage.enabled = true;
        }
        else
        {
            staminaMessage.enabled = false;
        }

        UpdateStaminaUI();
        #endregion

        #region Handles Jumping
        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpPower;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }
        #endregion

        #region Handles Rotation
        characterController.Move(moveDirection * Time.deltaTime);

        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
        #endregion
    }

    private void UpdateStaminaUI()
    {
        if (staminaBar != null)
        {
            staminaBar.value = currentStamina / maxStamina;
        }
    }

    public void DisableCharacter()
    {
        isDisabled = true;
        canMove = false;
        moveDirection = Vector3.zero;
        if (animator != null)
            animator.SetFloat("Speed", 0);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void EnableCharacter()
    {
        isDisabled = false;
        canMove = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}








