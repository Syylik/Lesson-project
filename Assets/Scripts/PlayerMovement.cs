using UnityEngine;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField, Range(0f, 30f)] private float _swingPower;
    [SerializeField] private List<Rigidbody2D> _endOfRopes;

    private bool _inAir = false;

    private Rigidbody2D _rigidbody2D;
    private SpringJoint2D _springJoint;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _springJoint = GetComponent<SpringJoint2D>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            if(_inAir) { GrapRope(); return; }
            if(!_inAir) { Drop(); return; }
        }

        if(Input.GetKey(KeyCode.A)) Swing(Vector2.left);
        else if(Input.GetKey(KeyCode.D)) Swing(Vector2.right);
    }

    private void Swing(Vector2 dir) => _rigidbody2D.AddForce(dir * _swingPower);

    private void Drop() { _springJoint.enabled = false; _inAir = true; }

    private void GrapRope()
    {
        _springJoint.enabled = true;
        var nearestLink = _endOfRopes[0];
        foreach(var link in _endOfRopes)
        {
            var linkDistance = Vector2.SqrMagnitude((Vector2)transform.position - link.position);
            var nearestLinkDstance = Vector2.SqrMagnitude((Vector2)transform.position - nearestLink.position);
            if(linkDistance < nearestLinkDstance) nearestLink = link;
        }
        _springJoint.connectedBody = nearestLink.GetComponent<Rigidbody2D>();
        _inAir = false;
    }
}