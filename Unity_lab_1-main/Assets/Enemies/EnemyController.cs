using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private int _health;
    private bool _died;
    [SerializeField] private float moveSpeed = 2f;
    private Animator _animator;
    [SerializeField] private Transform groundCheck; 
    [SerializeField] private float groundCheckDistance = 1f; 
    [SerializeField] private LayerMask groundLayer; 

    private bool movingRight = true;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }


    private void Update()
    {
        Move();
        CheckForPlatform();
        if (!_died)
        {
            
            if (_health <= 0)
                {
                    //_animator.SetTrigger("Died");
                    _died = true;
                    //_player.Score++;
                }
        }
        else
        {
            Destroy(gameObject, 1f);
        }
    }

    public void TakeDamage(int damage)
    {
        if (!_died)
        {
            _health -= damage;
            _animator.SetBool("mush_walk",false);
            _animator.SetTrigger("mush_damage");
        }
    }

    private void Move()
    {
        _animator.SetBool("mush_walk",true);
        Vector2 movement = new Vector2(movingRight ? moveSpeed : -moveSpeed, rb.velocity.y);
        rb.velocity = movement;
    }

    private void CheckForPlatform()
    {
        
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);

        
        if (hit.collider == null)
        {
            ChangeDirection();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.collider.CompareTag("Wall"))
        {
            ChangeDirection();
        }
    }

    private void ChangeDirection()
    {
        movingRight = !movingRight;
        Flip();
    }

    private void Flip()
    {
        Vector3 enemyScale = transform.localScale;
        enemyScale.x *= -1;
        transform.localScale = enemyScale;
    }
}
