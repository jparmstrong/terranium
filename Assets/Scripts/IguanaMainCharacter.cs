using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;
using System;

public class IguanaMainCharacter : MonoBehaviour {
	Animator iguanaAnimator;

    public Transform foodParent;
    public Waypoints waypoints;
    public Transform currentWaypoint = null;

    private float moveSpeed = 1f;
    private float distanceThresholds = 1f;

    private int eaten = 1;

    void Start () {
		iguanaAnimator = GetComponent<Animator> ();
        currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);
    }

    public void Attack(){
		iguanaAnimator.SetTrigger("Attack");
	}
	
	public void Hit(){
		iguanaAnimator.SetTrigger("Hit");
	}
	
	public void Eat(){
        float prev_speed = iguanaAnimator.speed;
        iguanaAnimator.speed = 1f;
        iguanaAnimator.SetTrigger("Eat");
        iguanaAnimator.speed = prev_speed;

    }

	public void Death(){
		iguanaAnimator.SetTrigger("Death");
	}

	public void Rebirth(){
		iguanaAnimator.SetTrigger("Rebirth");
	}

    void Update()
    {
        Vector3 targetDirection;
        Transform food = FindFood();
        if (food != null)
        {
            iguanaAnimator.speed = 3;
            targetDirection = food.position - transform.position;
        }
        else
        {
            if (Vector3.Distance(transform.position, currentWaypoint.position) < distanceThresholds)
            {
                currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);
            }

            iguanaAnimator.speed = 1;
            targetDirection = currentWaypoint.position - transform.position;
        }
        float angle = Vector3.Angle(targetDirection, transform.forward);
        Move(targetDirection.normalized.magnitude, angle * Mathf.PI / 180);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Food"))
        {
            iguanaAnimator.SetTrigger("Rebirth");
            Eat();
            Destroy(other.gameObject.transform.parent.gameObject);
            eaten++;
            Debug.Log($"Eaten: {eaten}");
            if (this.transform.localScale.x < 20)
            {
                this.transform.localScale += Vector3.one * 0.1f;
            }
        }
    }

    public Transform FindFood()
    {
        Transform min_food = null;
        foreach (Transform food in foodParent)
        {
            if (min_food == null)
            {
                min_food = food;
                continue;
            }
            if ((transform.position-food.position).magnitude < (transform.position - min_food.position).magnitude &&
                (transform.position-min_food.position).magnitude > 2f)
            {
                min_food = food;
            }
        }
        return min_food;
    }

    public void Move(float v,float h){
        iguanaAnimator.SetFloat("Forward", v);
        iguanaAnimator.SetFloat("Turn", h);
	}
}
