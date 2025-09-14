using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectRaycastInteractor : MonoBehaviour
{
    private Camera cam;
    private IInteracable currentTarget;

    void Awake()
    {
        cam = Camera.main;
        if (cam == null)
        {
            Debug.LogError("[ObjectRaycastInteractor] Không tìm thấy Camera với tag MainCamera!");
        }
    }

    void Update()
    {
        // Nếu đang mở UI thì bỏ qua raycast
        if (UIGamePlayManager.Instance != null && UIGamePlayManager.Instance.OpenAtap)
        {
            return;
        }

        if (cam == null) return;

        // Lấy vị trí chuột trong thế giới
        Vector2 mouseWorldPos = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        // Bắn raycast ngay tại vị trí chuột
        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);
        IInteracable newTarget = null;
        if (hit.collider != null)
        {
            newTarget = hit.collider.GetComponent<IInteracable>();
            if (newTarget == null)
            {
                Debug.LogWarning("[Raycast] Collider " + hit.collider.name + " không có IInteracable!");
            }
        }

        // Quản lý hover
        if (newTarget != currentTarget)
        {
            if (currentTarget != null)
                currentTarget.OnHoverExit();

            if (newTarget != null)
                newTarget.OnHoverEnter();

            currentTarget = newTarget;
        }

        // Click chuột trái
        if (currentTarget != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            currentTarget.OnClick();
        }
    }
}
