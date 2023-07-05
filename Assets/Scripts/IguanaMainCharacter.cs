using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class IguanaMainCharacter : MonoBehaviour {
	Animator iguanaAnimator;

    public Transform foodParent;
    public Waypoints waypoints;
    public Transform currentWaypoint = null;
	private NavMeshAgent navMeshAgent;

    private float moveSpeed = 3f;
    private float distanceThresholds = 1f;

    private int eaten = 1;

    void Start () {
		iguanaAnimator = GetComponent<Animator> ();
        currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);
    }

    private void Awake()
    {
		navMeshAgent = GetComponent<NavMeshAgent>();
    }

    public void Attack(){
		iguanaAnimator.SetTrigger("Attack");
	}
	
	public void Hit(){
		iguanaAnimator.SetTrigger("Hit");
	}
	
	public void Eat(){
		iguanaAnimator.SetTrigger("EatDown");
	}

	public void Death(){
		iguanaAnimator.SetTrigger("Death");
	}

	public void Rebirth(){
		iguanaAnimator.SetTrigger("Rebirth");
	}

    void Update()
    {
        Transform food = FindFood();
        if (food != null)
        {
            navMeshAgent.destination = food.position;
            navMeshAgent.speed = 2f;
        }
        else
        {
            navMeshAgent.destination = currentWaypoint.position;
            navMeshAgent.speed = 0.3f;

            Move(navMeshAgent.transform.position.x, navMeshAgent.transform.position.y);
            if (Vector3.Distance(transform.position, currentWaypoint.position) < distanceThresholds)
            {
                currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);
            }
        }
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
            if ((transform.position-food.position).magnitude < (transform.position - min_food.position).magnitude)
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
