using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBehaviour : MonoBehaviour
{
    [Header("Shooting")]
    [Tooltip("Aiming script. Requires an isAiming() method, returning a bool informing whether the player is currently aiming.")]
    [SerializeField] private AimBehaviourBasic aimer;
    [Tooltip("Shooting button, references entry in Project Settings -> InputManager")]
    [SerializeField] private string shootButton = "Fire1";
    [Tooltip("How often the player will shoot in seconds.")]
    [SerializeField] private float fireRate = 0.2f;
    [SerializeField] private float range = 100f;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private GameObject impactEffect;
    
    
    private Camera cam;
    private float NextFire;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // FixedUpdate is called 50 times per second
    void FixedUpdate()
    {
        if(Input.GetAxisRaw(shootButton) != 0 && aimer.isAiming()) 
        {
            if(Time.time > NextFire)
            {
                NextFire = Time.time + fireRate;
                Debug.Log("Pew");
                Shoot();
            }
        }
    }


    void Shoot()
    {
        muzzleFlash.Play(); 

        RaycastHit hit;
        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);

            if(hit.collider.gameObject.CompareTag("Enemy")) {
                //Call take damage method on enemy
            }
            GameObject impact = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impact, 2f);
        }
    }
    
}
