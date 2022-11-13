using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class Entity_Smiler : MonoBehaviour
{

    public Transform visionTransform;

    public AudioSource smilerMusic;

    public float wanderRadius;
    public float wanderTimer;

    public bool no_ai;

    private Transform target;
    public NavMeshAgent agent;
    private float timer;

    public GameObject targetEntity;
    public int time;

    // Use this for initialization
    void OnEnable()
    {
        smilerMusic.gameObject.SetActive(false);
        agent = GetComponent<NavMeshAgent>();
        timer = wanderTimer;

        StartCoroutine(ReduceTimer());
    }

    // Update is called once per frame
    void Update()
    {
        if (no_ai)
        {
            return;
        }
        timer += Time.deltaTime;

        if (timer >= wanderTimer && targetEntity == null)
        {
            print("Finding new wander point!");
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            agent.SetDestination(newPos);
            timer = 0;
        }
        else if (targetEntity != null)
        {
            agent.SetDestination(targetEntity.gameObject.transform.position);
        }

        RaycastHit hitForward;
        int layerMask = 1 << 8;

        layerMask = ~layerMask;
        if (Physics.Raycast(visionTransform.transform.position, visionTransform.transform.TransformDirection(Vector3.forward), out hitForward, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(visionTransform.transform.position, visionTransform.transform.TransformDirection(Vector3.forward) * 1000, Color.yellow);
            if (hitForward.collider.gameObject.tag == "Entity.Player")
            {
                smilerMusic.gameObject.SetActive(true);
                time = 10;
                targetEntity = hitForward.collider.gameObject;
                print("Found entity target!");
                print("Distance: " + hitForward.distance);

                if (hitForward.distance <= 1)
                {
                    //Jumpscare

                    PlayerPrefs.SetInt("Data.Achievement.SayCheese", 1);
                    GameObject.Find("Level Manager").GetComponent<LevelManager>().CallJumpscare("Entity.Smiler");
                }
            }
            else
            {

            }
        }
    }

    IEnumerator ReduceTimer()
    {
        yield return new WaitForSeconds(1);
        time -= 1;

        if (time <= 0)
        {
            print("Lost entity target...");
            smilerMusic.gameObject.SetActive(false);
            targetEntity = null;
        }

        StartCoroutine(ReduceTimer());
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }

    public void ToggleAI()
    {
        no_ai = !no_ai;
    }
}