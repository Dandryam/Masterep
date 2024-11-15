using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTouchCheck : MonoBehaviour
{   
    bool _isGrounded;
    
    BoxCollider2D collision; 

    public bool IsGrounded
    {
        get
        {
            return _isGrounded;
        }
        set
        {
              _isGrounded = value;
        }
    }
    
    
    void Awake()
    {
        collision = GetComponent<BoxCollider2D>();
    }
    
    [SerializeField]
    ContactFilter2D groundFilter;
    RaycastHit2D[] groundHits = new RaycastHit2D[5];
    float groundCheckDistance = 0.01f;
    void FixedUpdate()
    {
        IsGrounded = collision.Cast(Vector2.down, groundFilter, groundHits, groundCheckDistance)>0;
    }

    //wall climb
    //when character is close enough to any wall then
    //falling slower 
}


