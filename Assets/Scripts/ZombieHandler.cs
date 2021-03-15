using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieHandler : MonoBehaviour {

    private float _speed;
    private Transform _target;
    private UnityEngine.AI.NavMeshAgent _navMeshAgent;
    private Animator _animator;
    private Rigidbody[] _rigidbodies;
    private int _health;
    private BoxCollider _collider;

    private bool _destroy = false; // Used if "Start" failed

    private RoundHandler _roundHandler; 

    // Start is called before the first frame update
    public void Start() {
        _target = GameObject.FindGameObjectWithTag("Player").transform;
        _navMeshAgent = this.GetComponent<UnityEngine.AI.NavMeshAgent>();
        _animator = this.GetComponent<Animator>();
        _rigidbodies = GetComponentsInChildren<Rigidbody>();
        _collider = GetComponent<BoxCollider>();
        _roundHandler = FindObjectOfType<RoundHandler>();
        _health = 50;
        
        if(_target == null) {
            Debug.LogError(gameObject.name + " couldn't locate a player!");
            _destroy = true;
        }
        
        if(_navMeshAgent == null) {
            Debug.LogError(gameObject.name + " is missing a NavMeshAgent component!");
            _destroy = true;
        }

        if(_animator == null) {
            Debug.LogError(gameObject.name + " is missing a Animator component!");
            _destroy = true;
        }

        if(_destroy) {
            Destroy(gameObject);
        }

        foreach (Rigidbody rb in _rigidbodies) {
            if (rb.gameObject != gameObject) {
                rb.useGravity = false;
                rb.isKinematic = true;
            }
        }

        GoRagdoll(false);

        _speed = Random.Range(0.2F, 5F);
        _navMeshAgent.speed = _speed;
        _animator.SetFloat("Speed", _speed);

        _navMeshAgent.SetDestination(_target.position);
    }

    // Update is called once per frame
    private void Update() {
        if (IsAlive()) {
            Move();
        }
    }

    /* ##################################################

            Health related

    ################################################## */

    public void SetHealth(int health) {
        _health = health;
    }

    public void Damage(int damage) {
        if(_health > 0) {
            _health -= damage;
            CheckHealth();
        }
    }

    private void CheckHealth() {
        if(_health <= 0) {
            Die();
        }
    }

    private void Die() {
        _roundHandler.CheckZombiesLeft();
        _collider.enabled = false;
        _animator.enabled = false;
        _navMeshAgent.speed = 0;
        GoRagdoll(true);
    }

    public bool IsAlive() {
        return _health > 0;
    }

    private void GoRagdoll(bool status) {
        _rigidbodies = GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody rb in _rigidbodies) {
            if (rb.gameObject != gameObject) {
                rb.useGravity = status;
                rb.isKinematic = !status;
            }
        }
    }

    /* ##################################################

            Movement related

    ################################################## */

    private float _distance;
    private bool _notAttacking;

    private void Move() {
        _navMeshAgent.SetDestination(_target.position);

        _distance = _navMeshAgent.remainingDistance - _navMeshAgent.stoppingDistance;
        _notAttacking = _animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != "Attack";

        if(_distance <= 0) {
            DoAttack(_notAttacking);
        } else if(_distance > 0 && _notAttacking) {
            _animator.SetFloat("Speed", _speed);
            _navMeshAgent.speed = _speed;
        }
    }

    private void DoAttack(bool notAttacting) {
        if(_navMeshAgent.speed != 0) {
            _navMeshAgent.speed = 0;
            _animator.SetFloat("Speed", 0);
        }

        if(notAttacting) {
            _animator.SetTrigger("Attack");
        }

        if(_isAttacking) {
            // Call take damage method on Player
        }
    }

    /* ##################################################

            Attack related

    ################################################## */

    private bool _isAttacking = false;

    public void StartAttackEvent() {
        _isAttacking = true;
    }

    public void StopAttackEvent() {
        _isAttacking = false;
    }

    /* ##################################################

            Victory related

    ################################################## */

    public void StartDancing() {
        Debug.Log("HEJJJ");
        _navMeshAgent.speed = 0;
        _animator.SetTrigger("Victory");
    }

}
