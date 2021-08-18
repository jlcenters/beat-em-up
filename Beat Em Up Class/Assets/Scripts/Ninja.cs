using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ninja : Enemy
{
    public float speed;
    public float chaseDistance;
    public float stopDistance;
    public GameObject target;
    private float distanceTarget;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        distanceTarget = Vector2.Distance(transform.position, target.transform.position);

        if(distanceTarget < chaseDistance && distanceTarget > stopDistance)
        {
            ChasePlayer();
        }
        else
        {
            StopChasePlayer();
        }
    }

    private void StopChasePlayer()
    {
        anim.SetBool("IsWalking", false);
    }

    private void ChasePlayer()
    {
        if(transform.position.x < target.transform.position.x)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }

        transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
        anim.SetBool("IsWalking", true);
    }
}
