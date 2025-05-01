using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]

public class JenkaPiece : MonoBehaviour
{

    [Header("Drag Settings")]
    public float maxDragDistance = 2f; // Distancia maxima que a peca pode ser arrastada

    [Header("Push Settings")]
    public float pushForce = 5f; //Forca aplicada em clique simples
    public float doubleClickForceMultiplier = 2f; //Muiltiplicador de forca para duplo clique
    public float doubleClickThresold = 0.3f; //Tempo maximo entre cliques para ser considerado duplo clique

    private Rigidbody rb;
    private Camera mainCamera;
    private bool isDragging = false;
    private Vector3 offset;
    private Vector3 startDragPosition;
    private float lastClickTime = -1f;
 
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
    }

    private void OnMouseDown()
    {
        //Detecta clique duplo
        float timeSinceLastClick = Time.time - lastClickTime;
        lastClickTime = Time.time;

        //Se for duplo clique, aplica forca maior e nao inicia arrasto
        if (timeSinceLastClick <= doubleClickThresold) 
        {
            ApplyPush(pushForce * doubleClickForceMultiplier);
            return;
        }

        //se for clique simples, aplica forca normal
        ApplyPush(pushForce);

        //Inicia arrasto
        isDragging = true;
        rb.isKinematic = true;

        //Calcula offset entre o mouse e a posicao atual da peca
        Vector3 mousePos = GetMouseWorldPosition();
        offset = transform.position - mousePos;
        startDragPosition = transform.position;
    }

    private void OnMouseDrag()
    {
        if (!isDragging) return;

        //Obtem posicao do mouse no mundo
        Vector3 mousePos = GetMouseWorldPosition();
        Vector3 targetPos = mousePos + offset;

        //Limita a distancia de arrasto
        Vector3 dragVector = targetPos - startDragPosition;
        if (dragVector.magnitude > maxDragDistance) 
        {
            dragVector = dragVector.normalized * maxDragDistance;
        }

        transform.position = startDragPosition + dragVector;
    }

    private void OnMouseUp() 
    {
        //Libera arrasto e ativa fisica novamente
        isDragging = false;
        rb.isKinematic = false;
    }

    //<summary>
    //Aplica Forca na direcao da camera como um "empurrao'
    //</summary>
    private void ApplyPush(float force) 
    {
        Vector3 direction = (transform.position - mainCamera.transform.position).normalized;
        rb.AddForce(direction * force, ForceMode.Impulse);
    }

    private Vector3 GetMouseWorldPosition() 
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, transform.position); // Plano Horizontal baseado na altura atual da peca
        if (plane.Raycast(ray, out float distance)) 
        {
            return ray.GetPoint(distance);
        }
        return transform.position;
    }

    private void Update()
    {
        
    }
}
