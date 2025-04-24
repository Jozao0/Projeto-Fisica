using System.Collections;
using UnityEngine;

public class AnimationToRagdoll : MonoBehaviour
{
    [SerializeField] Collider myCollider;
    [SerializeField] float ragdollTime = 3f;
    Rigidbody[] rigidbodies;
    bool isRagdoll = false;
    Animator animator;

    void Start()
    {
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        animator = GetComponent<Animator>();
        SetRagdollActive(false); // começa animando
        animator.Play("Walk_F"); // inicia andando
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile") && !isRagdoll)
        {
            Debug.Log("NPC atingido!");

            // Prende o projétil no corpo
            ContactPoint contact = collision.contacts[0];
            collision.transform.position = contact.point;
            collision.transform.SetParent(contact.otherCollider.transform);

            if (collision.rigidbody != null)
                collision.rigidbody.isKinematic = true;

            // Ativa ragdoll
            SetRagdollActive(true);
            StartCoroutine(RecoverFromRagdoll());
        }
    }

    private IEnumerator RecoverFromRagdoll()
    {
        yield return new WaitForSeconds(ragdollTime);

        // Desativa ragdoll e volta a animar
        SetRagdollActive(false);

        // Inicia a sequência de animações
        animator.Play("Hit_F_2");
        yield return new WaitForSeconds(1.5f); // tempo da animação de impacto

        animator.Play("HumanM@Talk01");
        yield return new WaitForSeconds(2f);

        animator.Play("HumanM@Run01_Forward");

        // Permite ser atingido novamente
        isRagdoll = false;
    }

    private void SetRagdollActive(bool state)
    {
        isRagdoll = state;
        animator.enabled = !state;
        myCollider.enabled = !state;

        foreach (Rigidbody rb in rigidbodies)
            rb.isKinematic = !state;
    }
}