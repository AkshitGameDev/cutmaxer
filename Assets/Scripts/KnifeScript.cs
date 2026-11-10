
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class KnifeScript : MonoBehaviour
{
    [Header("Launch Settings")]
    [SerializeField] private float horizontalSpeed = 1.5f;
    [SerializeField] private float verticalSpeed = 3f;

    [Header("Gravity Settings")]
    [SerializeField] private float gravityMultiplier = 1.5f;

    [Header("Rotation Settings")]
    [SerializeField] private float rotationSpeed = 720f;

    private Transform knifeChild;
    private Rigidbody rb;

    private bool isRotating = false;
    private bool launchRequested = false;

    private float rotatedAngle = 0f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        if (transform.childCount > 0)
        {
            knifeChild = transform.GetChild(0);
        }
        else
        {
            Debug.LogError("No knife child found!");
        }

        // Rigidbody settings
        rb.isKinematic = false;
        rb.useGravity = true;

        rb.interpolation = RigidbodyInterpolation.Interpolate;

        rb.collisionDetectionMode =
            CollisionDetectionMode.Continuous;

        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void Update()
    {
        if (knifeChild == null)
            return;

        // Launch on Space
        if (Keyboard.current != null &&
            Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            launchRequested = true;
        }

        // Rotate only during a flip
        if (isRotating)
        {
            UpdateRotation();
        }
    }

    void FixedUpdate()
    {
        // Handle launch
        if (launchRequested)
        {
            launchRequested = false;
            Launch();
        }

        // Apply extra gravity
        if (!rb.isKinematic)
        {
            rb.AddForce(
                Physics.gravity * (gravityMultiplier - 1f),
                ForceMode.Acceleration
            );
        }
    }

    void Launch()
    {
        // Reset velocity to prevent accumulating speed
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Launch forward and upward
        rb.linearVelocity = new Vector3(
            horizontalSpeed,
            verticalSpeed,
            0f
        );

        // Start rotation from current orientation
        isRotating = true;
        rotatedAngle = 0f;
    }

    void UpdateRotation()
    {
        float remainingAngle = 360f - rotatedAngle;

        float rotationStep = Mathf.Min(
            rotationSpeed * Time.deltaTime,
            remainingAngle
        );

        // Rotate knife around local Z axis
        knifeChild.Rotate(
            0f,
            0f,
            -rotationStep,
            Space.Self
        );

        rotatedAngle += rotationStep;

        // Stop after one complete rotation
        if (rotatedAngle >= 360f)
        {
            isRotating = false;
        }
    }
}
