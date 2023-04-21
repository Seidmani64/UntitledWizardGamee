using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private float fireDelay;
    [SerializeField] private Transform firePos;
    [SerializeField] private int maxmana = 20;
    private float fireTimer = 0f;
    private int mana;
    [SerializeField] private ManaBar manaBar;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private float chargeTime = 2f;
    [SerializeField] private Animator animator;
    private float currentCharge  = 0f;
    private AudioSource audioSource;
    [SerializeField] private AudioClip lightningCharge, lightningRelease;
    private bool canShoot;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        mana = maxmana;
        manaBar.SetMaxMana(mana);

    }

    // Update is called once per frame
    void Update()
    {
        fireTimer += Time.deltaTime;
        if(fireTimer >= fireDelay)
        {
            canShoot = true;
            animator.SetBool("CanShoot", true);
        }
        else
        {
            canShoot = false;
            animator.SetBool("CanShoot", false);
        }

        if(canShoot && Input.GetMouseButtonDown(0))
            Fireball();
        else if(currentCharge >= chargeTime && Input.GetMouseButtonUp(1))
        {
            Lightning();
        }
        else if(canShoot && Input.GetMouseButton(1) && mana >= 10)
        {
            animator.SetBool("Charging", true);
            if (currentCharge <= 0)
            {
                audioSource.clip = lightningCharge;
                audioSource.Play();
            }
            currentCharge += Time.deltaTime;
            if(currentCharge >= chargeTime)
            {
                animator.SetBool("ChargeReady", true);
                audioSource.Stop();
            } 
        }
        else
        {
            animator.SetBool("Charging", false);
            currentCharge = 0f;
            if(canShoot)
                audioSource.Stop();
        }
            
    }

    void Fireball()
    {
        animator.SetTrigger("Fire");
        fireTimer = 0;
        Instantiate(bullet, firePos.position, Quaternion.identity);
    }

    void Lightning()
    {
        animator.SetBool("Charging", false);
        animator.SetBool("ChargeReady", false);
        audioSource.PlayOneShot(lightningRelease);
        fireTimer = -fireDelay/2;
        currentCharge = 0;
        mana -= 10;
        manaBar.SetMana(mana);
        RaycastHit rayHit;
        if(Physics.Raycast(firePos.position, Camera.main.transform.forward, out rayHit, Mathf.Infinity, enemyMask))
        {
            Debug.Log(rayHit.collider.gameObject.tag);
            Enemy enemyHit = rayHit.collider.GetComponent<Enemy>();
            if(enemyHit != null)
                enemyHit.TakeDamage(3);
            
            Debug.Log("Did Hit");
        }
    }

    public void GainMana()
    {
        if(mana <= maxmana)
            mana += 1;
        manaBar.SetMana(mana);
    }
}
