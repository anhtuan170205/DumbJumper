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

    private Vector3 grapplePoint;
    private DistanceJoint2D grappleJoint;

    private void Start()
    {
        grappleJoint = GetComponent<DistanceJoint2D>();
        grappleJoint.enabled = false;
        ropeRenderer.enabled = false;
        inputReader.ShootEvent += HandleShoot;
    }

    private void OnDestroy()
    {
        inputReader.ShootEvent -= HandleShoot;
    }

    private void Update()
    {
        if (grappleJoint.enabled && ropeRenderer.enabled)
        {
            ropeRenderer.SetPosition(1, transform.position);
        }
    }


    private void HandleShoot(bool isShooting)
    {
        if (isShooting)
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
            }
        }
        else
        {
            grappleJoint.enabled = false;
            ropeRenderer.enabled = false;
        }

    }
}
