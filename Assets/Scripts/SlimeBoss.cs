using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class SlimeBoss : Enemy
{
        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            player = GameObject.FindWithTag("Player").transform;
        }

        // Update is called once per frame
        void Update()
        {
            recoveryTime += Time.deltaTime;
            if(recoveryTime >= maxRecoveryTime)
                animator.SetBool("Hurt", false);
            playerInSights = Physics.CheckSphere(transform.position, sightRange, isPlayer);
            playerInRange = Physics.CheckSphere(transform.position, attackRange, isPlayer);

            if(playerInSights && !playerInRange)
                Chasing();
            else if(playerInSights && playerInRange)
                Attacking();
        }

        public override void Attacking()
        {
            Vector3 targetPosition = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
            transform.LookAt(targetPosition);
        }

    public override void Die()
    {
        Debug.Log("Owie");
    }
}
