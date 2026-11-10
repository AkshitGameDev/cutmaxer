
using UnityEngine;
using UnityEngine.InputSystem;

public class KnifeScript : MonoBehaviour
{
    [Header("Launch Settings")]
    [SerializeField] private float moveX = 1f;
    [SerializeField] private float moveY = 1.5f;
    [SerializeField] private float launchDuration = 0.5f;

    [Header("Rotation Settings")]
    [SerializeField] private float rotationSpeed = 720f;

    [Header("Gravity Settings")]
    [SerializeField] private float gravity = 9.81f;

    private Transform knifeChild;

    private bool isLaunching;
    private float timer;

    private float verticalVelocity;

    private Vector3 startPosition;
    private Vector3 targetPosition;

    private Rigidbody rb;

    void Awake()
    {
        if (transform.childCount > 0)
        {
            knifeChild = transform.GetChild(0);
        }
        else
        {
            Debug.LogError("No knife child found!");
        }

        rb = GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }
    }

    void Update()
    {
        if (knifeChild == null)
            return;

        // Continuous knife rotation
        knifeChild.Rotate(
            0f,
            0f,
            -rotationSpeed * Time.deltaTime,
            Space.Self
        );

        // Launch input
        if (Keyboard.current != null &&
            Keyboard.current.spaceKey.wasPressedThisFrame &&
            !isLaunching)
        {
            Launch();
        }

        if (isLaunching)
        {
            UpdateLaunch();
        }
        else
        {
            ApplyGravity();
        }
    }

    void Launch()
    {
        isLaunching = true;
        timer = 0f;

        // Reset falling velocity on every launch
        verticalVelocity = 0f;

        startPosition = transform.position;

        targetPosition = startPosition +
            new Vector3(moveX, moveY, 0f);
    }

    void UpdateLaunch()
    {
        timer += Time.deltaTime;

        float progress = Mathf.Clamp01(
            timer / Mathf.Max(launchDuration, 0.01f)
        );

        // Smooth movement
        float smoothProgress =
            Mathf.SmoothStep(0f, 1f, progress);

        transform.position = Vector3.Lerp(
            startPosition,
            targetPosition,
            smoothProgress
        );

        if (progress >= 1f)
        {
            isLaunching = false;
            verticalVelocity = 0f;
        }
    }

    void ApplyGravity()
    {
        verticalVelocity -= gravity * Time.deltaTime;

        transform.position +=
            Vector3.up * verticalVelocity * Time.deltaTime;
    }
}
