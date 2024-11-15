using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCont : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _stoppingDistance;
    [SerializeField] private float _retreatDistance;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private Transform[] _jumpChecks;
    [SerializeField] private float _checkRadius;
    [SerializeField] private float _checkJumpRadius;
    [SerializeField] private LayerMask _ground;
    [SerializeField] private GameObject _rotate;
    [SerializeField] private float _offset;
    [SerializeField] private GameObject _bullet;
    [SerializeField] private Transform _shotPoint;
    [SerializeField] private float _startTimeShots;
    [SerializeField] private float _distance;
    [SerializeField] private LayerMask _playerMask;
    [SerializeField] private int _health;
    [SerializeField] private TextMesh _healthText;
    private float _timeShots;
    private bool _died;
    private bool _canShoot;
    private bool _isGrounded;
    private bool _jump;
    private bool _jump2;
    private bool _facingRight = true;
    private Rigidbody2D _rigidbody2D;
    private PlayerMove _player;
    private Animator _animator;
    private Vector2 _position;

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _player = FindObjectOfType<PlayerMove>();
    }

    private void Update()
    {
        if (!_died)
        {
            _healthText.text = "HP: " + _health.ToString();

            if (_health <= 0)
            {
                //_animator.SetTrigger("Died");
                _died = true;
                //_player.Score++;
            }

            _isGrounded = Physics2D.OverlapCircle(_groundCheck.position, _checkRadius, _ground);
            _jump = Physics2D.OverlapCircle(_jumpChecks[0].position, _checkJumpRadius, _ground);
            _jump2 = Physics2D.OverlapCircle(_jumpChecks[1].position, _checkJumpRadius, _ground);
            _canShoot = Physics2D.OverlapCircle(transform.position, _distance, _playerMask);
            //_animator.SetBool("onGround", _isGrounded);

            if (_isGrounded)
            {
                if (_jump || _jump2)
                {
                    //_animator.StopPlayback();
                    //_animator.Play("Jump");
                    _rigidbody2D.velocity = Vector2.up * _jumpForce;
                }
            }

            if (_canShoot)
            {
                if (_timeShots <= 0)
                {
                    Instantiate(_bullet, _shotPoint.position, _rotate.transform.rotation);
                    _timeShots = _startTimeShots;
                }
                else
                {
                    _timeShots -= Time.deltaTime;
                }
            }

            if (!_facingRight && _player.transform.position.x > transform.position.x)
            {
                //Flip();
                _offset = 0f;
                //                             Положение текста жизней по x    ↓            ↓            ↓
                _healthText.gameObject.transform.localScale = new Vector3(0.07381891f, 0.07381891f, 0.07381891f);
                _bullet.GetComponent<Bullet>().Direction = Vector2.right;
            }
            else if (_facingRight && _player.transform.position.x < transform.position.x)
            {
                //Flip();
                _offset = 180f;
                //                               Положение текста жизней по x (без -)        ↓            ↓
                //                                                      Нужен - ↓
                _healthText.gameObject.transform.localScale = new Vector3(0.07381891f, 0.07381891f, 0.07381891f);
                _bullet.GetComponent<Bullet>().Direction = Vector2.left;
            }

            Vector3 difference = _player.transform.position - _rotate.transform.position;
            float rot2 = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            _rotate.transform.rotation = Quaternion.Euler(0f, 0f, rot2 + _offset);

            if (Vector2.Distance(transform.position, _player.transform.position) > _stoppingDistance)
            {
                _position = Vector2.MoveTowards(transform.position, _player.transform.position, _speed * Time.deltaTime);
                //_animator.SetBool("Walk", true);
                //_animator.SetBool("WalkBack", false);
            }
            else if (Vector2.Distance(transform.position, _player.transform.position) < _stoppingDistance && Vector2.Distance(transform.position, _player.transform.position) > _retreatDistance)
            {
                _position = transform.position;
                //_animator.SetBool("Walk", false);
                //_animator.SetBool("WalkBack", false);
            }
            else if (Vector2.Distance(transform.position, _player.transform.position) < _retreatDistance)
            {
                _position = Vector2.MoveTowards(transform.position, _player.transform.position, -_speed * Time.deltaTime);
                //_animator.SetBool("Walk", false);
                //_animator.SetBool("WalkBack", true);
            }

            transform.position = new Vector2(_position.x, transform.position.y);
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
            //_animator.SetTrigger("Damage");
        }
    }

    public void SizeCollider()
    {
        BoxCollider2D boxCollider2D = GetComponent<BoxCollider2D>();
        boxCollider2D.size = new Vector2(4.125334f, 2.069604f);
        boxCollider2D.offset = new Vector2(-0.7977967f, 0.3616319f);
    }

    void Flip()
    {
        _facingRight = !_facingRight;
        Vector3 Scale = transform.localScale;
        Scale.x *= -1;
        transform.localScale = Scale;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _distance);
    }
}