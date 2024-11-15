using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _lifeTime;
    [SerializeField] private float _distance;
    [SerializeField] private int _damage;
    [SerializeField] private LayerMask _solid;
    [HideInInspector] public Vector2 Direction;
    [SerializeField] private bool _enemyBullet;

    private void Start()
    {
        Destroy(gameObject, _lifeTime);
    }

    private void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Direction, _distance, _solid);

        if(hit.collider != null)
        {
            if(hit.collider.CompareTag("Player") && _enemyBullet)
            {
                hit.collider.GetComponent<PlayerMove>().TakeDamage(_damage);
            }
            if (hit.collider.CompareTag("Dragon"))
            {
                hit.collider.GetComponent<EnemyCont>().TakeDamage(_damage);
            }
            if (hit.collider.CompareTag("Enemy"))
            {
                hit.collider.GetComponent<EnemyController>().TakeDamage(_damage);
            }
            
            Destroy(gameObject);
        }

        transform.Translate(Direction * _speed * Time.deltaTime);
    }
}