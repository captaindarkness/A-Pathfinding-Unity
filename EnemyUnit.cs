using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.AI;

public class EnemyUnit : MonoBehaviour {

    public Camera cam;
    public NavMeshAgent agent;
    //public Transform target;
    public GameObject target;
    public Vector3 targetPos;
    public float distanceFromPlayer;
	public Image healthBar;
    float speed = 3;
    Vector3[] path;
    int targetIndex;
	public int maxHealth;
	public int health;

    void Start()
    {
        //PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
		maxHealth = 100;
		health = maxHealth;
    }

	void Update()
	{
		// if (Input.GetKeyDown(KeyCode.LeftControl)) {
		// 	PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
		// }       
        targetPos = target.transform.position;
        distanceFromPlayer = Vector3.Distance(this.transform.position, targetPos);
        //Go to Player when "E" is pressed
        if(Input.GetKeyUp(KeyCode.E))
        {
            agent.SetDestination(targetPos);
        }
        //Go to point clicked on screen
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit))
            {
                agent.SetDestination(hit.point);
            }
        }
	}

	void TakeDamage(int damage)
	{
		// health -= damage;

		// healthBar.fillAmount = (float)health / (float)maxHealth;

		// if (health <= 0) {
		// 	print ("dead");
		// }
	}

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = newPath;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath()
    {
        Vector3 currentWaypoint = path[0];

        while (true)
        {
            if (transform.position == currentWaypoint)
            {
                targetIndex++;
                if (targetIndex >= path.Length)
                {
					print ("Movement DONE!");
					targetIndex = 0;
					path = new Vector3[0];
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }

            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
            yield return null;
        }
    }

    public void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], Vector3.one * 0.3f);

                if (i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }
}
