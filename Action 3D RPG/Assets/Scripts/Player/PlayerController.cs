using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Look")]
    public Transform cameraContainer;
    private Vector2 deltaMouse;
    public float cameraBoundry = 90f;
    public float lookSensivity = 0.2f;
    private float currentCamera_X;
    private float currentCamera_Y;

    [Header("Movement")]
    public float moveSpeed;
    private Vector2 currentMovementInput;
    public float jumpForce;
    public LayerMask groundLayerMask;

    [HideInInspector]
    public bool canLook = true;

    //Components
    private Rigidbody rb;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Move();
    }

    void LateUpdate()
    {
        if(canLook == true)
        {
            CameraLook();
        }

        
    }

    private void CameraLook()
    {

        currentCamera_X += deltaMouse.y * lookSensivity;
        currentCamera_Y += deltaMouse.x * lookSensivity;

        currentCamera_X = Mathf.Clamp(currentCamera_X, -cameraBoundry, cameraBoundry);

        //Rotating up and down
        cameraContainer.localEulerAngles = new Vector3(-currentCamera_X, 0, 0);

        //Rotating in left an right angel
        transform.localEulerAngles = new Vector3(0, currentCamera_Y, 0);

    }
    
    private void Move()
    {
        
        float xMov = currentMovementInput.x * moveSpeed;
        float yMov = currentMovementInput.y * moveSpeed;

        Vector3 movHorizontal = transform.right * xMov;
        Vector3 movVertical = transform.forward * yMov;
        Vector3 finalMov = movHorizontal + movVertical;

        rb.MovePosition(rb.position + finalMov * Time.fixedDeltaTime);
    }
    
    bool IsGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (Vector3.up * 0.01f) ,Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (Vector3.up * 0.01f) ,Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (Vector3.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) + (Vector3.up * 0.01f) , Vector3.down),
        };

        
        for(int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.1f, groundLayerMask))
            {
                return true;
            }
        }

        return false;
    }
    
    public void OnLook(InputAction.CallbackContext context)
    {
        deltaMouse = context.ReadValue<Vector2>();
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        //Are we holding down W,A,S,D keys
        if(context.phase == InputActionPhase.Performed)
        {
            //Move to direction depend on Vector2 value
            currentMovementInput = context.ReadValue<Vector2>();
        }
        //No longer pressing down any keys
        else if(context.phase == InputActionPhase.Canceled)
        {
            //No movement
            currentMovementInput = Vector2.zero;
        }

    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        //Is this the first frame we are pressing the button
        if(context.phase == InputActionPhase.Started)
        {
            //Are we standing on the ground
            if (IsGrounded())
            {
                //Add force in upwards direction
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }

    } 

    public void ToggleCursor(bool toggle)
    {
        //If the cursor is locked set it to none otherwise set the cursor to lock mode
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;

        //Can look set to opposite of toggle
        canLook = !toggle;
    }

    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position + (transform.forward * 0.2f) + (Vector3.up * 0.1f), Vector3.down);
        Gizmos.DrawRay(transform.position + (-transform.forward * 0.2f) + (Vector3.up * 0.1f), Vector3.down);
        Gizmos.DrawRay(transform.position + (transform.right * 0.2f) + (Vector3.up * 0.1f), Vector3.down);
        Gizmos.DrawRay(transform.position + (-transform.right * 0.2f) + (Vector3.up * 0.1f), Vector3.down);
    }

}
