
using UnityEngine;
using UnityEngine.InputSystem;

public class Pistol : MonoBehaviour
{
    [Header("Target")]
    public Transform aimTarget;

    [Header("Shot")]
    public float shotHeight = 0.5f;
    public float shotDistance = 1000f;
    public LayerMask hitMask = Physics.DefaultRaycastLayers;

    [Header("Laser")]
    public float laserWidth = 0.03f;

    private LineRenderer tracer;

    void Start()
    {
        tracer = gameObject.AddComponent<LineRenderer>();

        tracer.positionCount = 2;
        tracer.startWidth = laserWidth;
        tracer.endWidth = laserWidth;
        tracer.enabled = true;
        tracer.useWorldSpace = true;

        Material material = new Material(Shader.Find("Sprites/Default"));
        tracer.material = material;
    }

    void Update()
    {
        if (aimTarget == null)
        {
            tracer.enabled = false;
            return;
        }

        tracer.enabled = true;

        Vector3 start = transform.position + Vector3.up * shotHeight;

        Vector3 direction = aimTarget.position - start;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f)
            return;

        direction.Normalize();

        Vector3 end = start + direction * shotDistance;

        if (Physics.Raycast(start, direction, out RaycastHit hit, shotDistance, hitMask))
        {
            end = hit.point;
        }

        tracer.SetPosition(0, start);
        tracer.SetPosition(1, end);

        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Shoot(start, direction);
        }
    }

    void Shoot(Vector3 start, Vector3 direction)
    {
        if (Physics.Raycast(start, direction, out RaycastHit hit, shotDistance, hitMask))
        {
            ReactiveTarget target = hit.collider.GetComponent<ReactiveTarget>();

            if (target != null)
            {
                target.ReactToHit();
            }

            Debug.Log("Shot hit: " + hit.collider.name);
        }
    }

    void OnDestroy()
    {
        if (tracer != null && tracer.material != null)
        {
            Destroy(tracer.material);
        }
    }
}
