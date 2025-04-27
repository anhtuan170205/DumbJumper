using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private float grappleLength;
    [SerializeField] private LayerMask grappleableLayer;
    [SerializeField] private LineRenderer ropeRenderer;
    private Vector3 previousPlayerPosition;
    [Header("Grapple Cooldown")]
    [SerializeField] private float grappleCooldown = 1f;
    private float lastGrappleTime;
    private bool isOnCooldown;

    private Vector3 grapplePoint;
    private DistanceJoint2D grappleJoint;

    // Add to GrapplingHook.cs
    public float CurrentCooldown => Mathf.Clamp(Time.time - lastGrappleTime, 0, grappleCooldown);
    public float GrappleCooldown => grappleCooldown;
    public bool IsOnCooldown => Time.time < lastGrappleTime + grappleCooldown;

    private void Start()
    {
        grappleJoint = GetComponent<DistanceJoint2D>();
        grappleJoint.enabled = false;
        ropeRenderer.enabled = false;
        inputReader.ShootEvent += HandleShoot;
        lastGrappleTime = -grappleCooldown;
    }

    private void OnDestroy()
    {
        inputReader.ShootEvent -= HandleShoot;
    }

    private void Update()
    {
        isOnCooldown = Time.time < lastGrappleTime + grappleCooldown;
        if (grappleJoint.enabled && ropeRenderer.enabled)
        {
            ropeRenderer.SetPosition(1, transform.position);
        }
    }


    private void HandleShoot(bool isShooting)
    {
        if (isShooting && !isOnCooldown)
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, grappleableLayer);
            if (hit.collider != null)
            {
                grapplePoint = hit.point;
                grapplePoint.z = 0; // Ensure grapplePoint is in 2D space
                grappleJoint.connectedAnchor = grapplePoint;
                grappleJoint.enabled = true;
                grappleJoint.distance = grappleLength;
                ropeRenderer.SetPosition(0, grapplePoint);
                ropeRenderer.SetPosition(1, transform.position);
                ropeRenderer.enabled = true;
                lastGrappleTime = Time.time;
            }
        }
        else
        {
            grappleJoint.enabled = false;
            ropeRenderer.enabled = false;
        }

    }

    private void OnGUI()
    {
        if (isOnCooldown)
        {
            float remaining = (lastGrappleTime + grappleCooldown) - Time.time;
            GUI.Label(new Rect(10, 10, 200, 20), $"Grapple Cooldown: {remaining.ToString("F1")}s");
        }
    }
}
