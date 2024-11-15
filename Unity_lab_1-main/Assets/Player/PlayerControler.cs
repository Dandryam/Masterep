using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private float _checkRadius;
    [SerializeField] private LayerMask _ground;
    [SerializeField] private int _health;
    [SerializeField] private Text _healthText;
    [SerializeField] private GameObject _rotate;
    [SerializeField] private float _offset;
    [SerializeField] private float _startTimeShots;
    [SerializeField] private GameObject _bullet;
    [SerializeField] private Transform _shotPoint;
    private float _timeShots;
    private Rigidbody2D _rigidbody2D;
    private float _horizontalMove;
    private bool _facingRight = true;
    private bool _isGrounded;
    private bool _died;
    private Animator _animator;
    SpriteRenderer spriteRenderer;


    private void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>(); 
    }
    void RestartLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
    public void Die()
    {
        _died=true;
        Debug.Log("Player died!");
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
    void Flip()
    {
        if(!_died)
        {
            _facingRight = !_facingRight;
            Vector3 Scale = transform.localScale;
            Scale.x *= -1;
            //transform.localScale = Scale;
            
        }
    }
     Vector2 moveInput;

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        if (moveInput.x > 0)
        {
            spriteRenderer.flipX = false; 
        }
        else if (moveInput.x < 0)
        {
            spriteRenderer.flipX = true; 
        }
    }
    public void FixedUpdate()
    {
        if(!_died)
        {
            _horizontalMove = Input.GetAxisRaw("Horizontal");

            _rigidbody2D.velocity = new Vector2(_horizontalMove * _speed, _rigidbody2D.velocity.y);

            if(!_facingRight && _horizontalMove > 0)
            {
                Flip();
            }
            else if (_facingRight && _horizontalMove < 0)
            {
                Flip();
            }

            if(_horizontalMove == 0)
            {
                _animator.SetBool("Walk", false);
            }
            else
            {
                _animator.SetBool("Walk", true);
            }
        }
    }

    private void Update()
    {
        if(!_died)
        {
            _healthText.text = "HP: " + _health.ToString();

            if(_health <= 0)
            {
                _animator.SetTrigger("Died");
                Invoke(nameof(ResetScene), 2f);
                _died = true;
            }
            if (transform.position.y <= -10f)
            {
                _animator.SetTrigger("Died");
                Invoke(nameof(ResetScene), 2f);
                _died = true;
            }

            if (Input.GetButtonDown("Cancel"))
            {
                SceneManager.LoadScene(0);  
            }
            _isGrounded = Physics2D.OverlapCircle(_groundCheck.position, _checkRadius, _ground);

            if(_isGrounded && Input.GetKeyDown(KeyCode.Space))
            {
                _rigidbody2D.velocity = Vector2.up * _jumpForce;
            }

            if(_isGrounded)
            {
                _animator.SetBool("Jump", false);
            }
            else
            {
                _animator.SetBool("Jump", true);
            }
            Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - _rotate.transform.position;
            float rot2 = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            _rotate.transform.rotation = Quaternion.Euler(0f, 0f, rot2 + _offset);


            if(_timeShots <= 0)
            {
                if(Input.GetMouseButton(0) )
                {
                    
                    _animator.SetBool("Attack",true);
                                       
                    _timeShots = _startTimeShots;
                }
                
            }
            else
            {
                
                _timeShots -= Time.deltaTime;
            }

            _bullet.GetComponent<Bullet>().Direction = Vector2.right;
            
        }

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        PlayerProgress.SaveProgress(currentSceneIndex);
    }
    
    void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void AttackToogle()
    {
        GetComponent<Animator>().SetBool("Attack", false);
        
    }
    public void SetBullet()
    {
        Instantiate(_bullet, _shotPoint.position, _rotate.transform.rotation);
    }
    public void TakeDamage(int damage)
    {
        if(!_died)
        {
            _health -= damage;
            _animator.SetTrigger("Damage");
        }
    }

    public void SizeCollider()
    {
        BoxCollider2D boxCollider2D = GetComponent<BoxCollider2D>();
        boxCollider2D.size = new Vector2(1.492382f, 0.229647f);
        boxCollider2D.offset = new Vector2(-0.003047407f, -0.06369022f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            TakeDamage(1);
        }
        if (collision.collider.CompareTag("Dragon"))
        {
            TakeDamage(1);
        }
    }

    [SerializeField] float teleportDistance = 5f;
    GameObject firePrefab;
    [SerializeField] float delayBetweenTeleports = 1f; 
    private float lastTeleportTime = 0f; 
    public void OnTeleport(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (Time.time >= lastTeleportTime + delayBetweenTeleports)
            {
                StartCoroutine(Teleport());
            }
        }
    }

    private IEnumerator Teleport()
    {
        Vector2 newPosition = (Vector2)transform.position + new Vector2(teleportDistance * (spriteRenderer.flipX ? -1 : 1), 0);
        if (!Physics2D.OverlapCircle(newPosition, 0.1f)) // ����������� ���������� ������
        {
            firePrefab = Resources.Load<GameObject>("FireEffect");
            GameObject Fire = Instantiate(firePrefab, transform.position, Quaternion.identity);
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(0.2f);
            transform.position = newPosition;
            spriteRenderer.enabled = true;
            GameObject Fire1 = Instantiate(firePrefab, transform.position, Quaternion.identity);

            lastTeleportTime = Time.time;
            yield return new WaitForSeconds(1f);
            Destroy(Fire);
            Destroy(Fire1);

        }
        else
        {
            Debug.Log("������������ �������������: �����������");
        }
    }


}
