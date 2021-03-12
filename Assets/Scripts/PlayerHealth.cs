using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int health = 100;
    [SerializeField] private int maxHealth = 200;
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody rigid;

    private void Start()
    {
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.H))
        {
            Damage(20);
        }
    }

    public void Damage(int damage)
    {
        health -= damage;
        checkHealth();
    }

    public void Heal(int heal)
    {
        health += heal;
        if(health > maxHealth)
        {
            health = maxHealth;
        }
    }

    public int GetHealth()
    {
        return health;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    private void checkHealth()
    {
        if (health <= 0)
        {
            StartCoroutine(Die());
        }
    }

    private IEnumerator Die()
    {
        animator.SetBool("Dead", true);
        yield return new WaitForSeconds(2f);
    }

}
