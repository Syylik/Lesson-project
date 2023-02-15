using System.Data;
using UnityEngine;

public class Rope : MonoBehaviour
{
    [SerializeField, Range(0f, 30f)] private float swingPower;
    private bool inAir = false;
    private string endRopeLinkTag = "EndRopeLink";

    private Rigidbody2D _rb;
    private SpringJoint2D _springJoint;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _springJoint = GetComponent<SpringJoint2D>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            if(inAir) { GrapRope(); return; }
            if(!inAir) { Drop(); return; }
        }

        if(Input.GetKey(KeyCode.A)) Swing(Vector2.left);
        else if(Input.GetKey(KeyCode.D)) Swing(Vector2.right);
    }

    private void Swing(Vector2 dir) => _rb.AddForce(dir * swingPower);

    private void Drop() { _springJoint.enabled = false; inAir = true; }

    private void GrapRope()
    {
        _springJoint.enabled = true;
        var links = GameObject.FindGameObjectsWithTag(endRopeLinkTag);
        var nearestLink = links[0];
        foreach(var link in links)
        {
            var linkDistance = Vector2.Distance(transform.position, link.transform.position);
            var nearestLinkDstance = Vector2.Distance(transform.position, nearestLink.transform.position);
            if(linkDistance < nearestLinkDstance) nearestLink = link;
        }
        _springJoint.connectedBody = nearestLink.GetComponent<Rigidbody2D>();
        inAir = false;
    }
}
