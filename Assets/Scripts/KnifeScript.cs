
using UnityEngine;
using UnityEngine.InputSystem;

public class KnifeScript : MonoBehaviour
{
    [Header("Launch Settings")]
    public float moveX = 1f;
    public float moveY = 1.5f;
    public float launchDuration = 0.5f;

    private Transform knifeChild;

    private bool isLaunching = false;
    private float timer = 0f;

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private Quaternion startRotation;

    void Start()
    {
        // Get first child of the parent
        if (transform.childCount > 0)
        {
            knifeChild = transform.GetChild(0);
        }
        else
        {
            Debug.LogError("No child found!");
        }
    }

    void Update()
    {
        if (Keyboard.current != null &&
            Keyboard.current.spaceKey.wasPressedThisFrame &&
            !isLaunching &&
            knifeChild != null)
        {
            Launch();
        }

        if (isLaunching)
        {
            timer += Time.deltaTime;

            float progress = Mathf.Clamp01(
                timer / launchDuration
            );

            // Move the PARENT object
            transform.position = Vector3.Lerp(
                startPosition,
                targetPosition,
                progress
            );

            // Rotate only the FIRST CHILD
            knifeChild.localRotation = startRotation *
                Quaternion.Euler(0f, 0f, -360f * progress);

            if (progress >= 1f)
            {
                isLaunching = false;
            }
        }
    }

    void Launch()
    {
        isLaunching = true;
        timer = 0f;

        // Parent movement
        startPosition = transform.position;

        targetPosition = startPosition +
            new Vector3(moveX, moveY, 0f);

        // Child rotation
        startRotation = knifeChild.localRotation;
    }
}
