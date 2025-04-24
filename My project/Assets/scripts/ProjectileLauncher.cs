using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float launchForce = 20f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // dispara ao apertar espa�o
        {
            GameObject proj = Instantiate(projectilePrefab, transform.position, transform.rotation);
            Rigidbody rb = proj.GetComponent<Rigidbody>();
            if (rb != null)
                rb.linearVelocity = transform.forward * launchForce;
        }
    }
}