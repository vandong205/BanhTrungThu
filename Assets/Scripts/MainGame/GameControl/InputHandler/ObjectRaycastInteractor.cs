using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectRaycastInteractor : MonoBehaviour
{
    private Camera cam;
    private IInteracable currentTarget;

    void Awake()
    {
        cam = Camera.main;
    }

    void Update()
    {
        // Tạo ray từ chuột
        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

        IInteracable newTarget = null;
        if (hit.collider != null)
        {
            newTarget = hit.collider.GetComponent<IInteracable>();
        }
        if (newTarget != currentTarget)
        {
            if (currentTarget != null)
                currentTarget.OnHoverExit();

            if (newTarget != null)
                newTarget.OnHoverEnter();

            currentTarget = newTarget;
        }

        // Click
        if (currentTarget != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            currentTarget.OnClick();
        }
    }
}
