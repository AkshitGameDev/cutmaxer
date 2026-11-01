using System.Collections;
using UnityEngine;

public enum EntityType
{
    Obstacle,
    Enemy,
    Boss,
    FinishLine,
    Neutral
}

public class EntityScript : MonoBehaviour
{
    [Header("Entity Settings")]
    [SerializeField] private EntityType entityType = EntityType.Obstacle;
    [SerializeField] private float vanishingTime = 1.5f;

    [Header("Slicing Settings")]
    [SerializeField] private Rigidbody cubeL;
    [SerializeField] private Rigidbody cubeR;

    [SerializeField] private float splitForce = 4f;

    private Collider entityCollider;
    private Material entityMaterial;
    private bool isVanishing = false;

    private Material cubeLMaterial;
    private Material cubeRMaterial;

    void Start()
    {
        entityCollider = GetComponent<Collider>();


        cubeLMaterial = cubeL.GetComponent<Renderer>().material;
        cubeRMaterial = cubeR.GetComponent<Renderer>().material;

        cubeL.isKinematic = true;
        cubeR.isKinematic = true;

        Debug.Log("Materials: " + cubeLMaterial + " / " + cubeRMaterial);
    }

    void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Player":
                if (entityType == EntityType.Obstacle)
                {
                    DestroyEntity();
                }
                break;
            case "Enemy":
                break;

            case "Boss":
                break;

            case "FinishLine":
                break;

            case "Neutral":
                break;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") &&
            entityType == EntityType.Obstacle)
        {
            DestroyEntity();
        }
    }

    void DestroyEntity()
    {
        if (isVanishing) return;

        isVanishing = true;

        // Disable parent trigger collider
        entityCollider.enabled = false;

        // Activate physics on both halves
        cubeL.isKinematic = false;
        cubeR.isKinematic = false;

        cubeL.useGravity = true;
        cubeR.useGravity = true;

        // Push left half to the left
        cubeL.AddForce(-transform.right * splitForce,
            ForceMode.Impulse
        );

        // Push right half to the right
        cubeR.AddForce(
            transform.right * splitForce,
            ForceMode.Impulse
        );

        

        // Start fading both halves
        StartCoroutine(FadeCoroutine());
    }

    IEnumerator FadeCoroutine()
    {
        Color leftColor = cubeLMaterial.color;
        Color rightColor = cubeRMaterial.color;

        float leftStartAlpha = leftColor.a;
        float rightStartAlpha = rightColor.a;

        float elapsed = 0f;

        while (elapsed < vanishingTime)
        {
            elapsed += Time.deltaTime;

            float progress = Mathf.Clamp01(elapsed / vanishingTime);

            // Fade left cube
            leftColor.a = Mathf.Lerp(leftStartAlpha, 0f, progress);
            cubeLMaterial.color = leftColor;

            // Fade right cube
            rightColor.a = Mathf.Lerp(rightStartAlpha, 0f, progress);
            cubeRMaterial.color = rightColor;

            yield return null;
        }

        leftColor.a = 0f;
        rightColor.a = 0f;

        cubeLMaterial.color = leftColor;
        cubeRMaterial.color = rightColor;

        // Disable the entire obstacle after fading
        gameObject.SetActive(false);
    }
}