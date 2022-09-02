using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{

    public enum State
    {
        walking, chasing
    }

    public State status = State.walking;
    public State previousStatus = State.walking;

    private NavMeshAgent agent;
    public Transform playerTransform;
    private Transform waypointTransform;
    public Transform groupTransform;
    public Transform centerEye;

    
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        GetNewDestination();
        if(centerEye == null) {
            Debug.LogError("You need to defind the variable centerEye");
        }
    }

    private void GetNewDestination () {
        Transform currentDestination = waypointTransform;
        do{
        int index = Random.Range(0, groupTransform.childCount);
        waypointTransform = groupTransform.GetChild(index);
        } while(waypointTransform == currentDestination);
        agent.SetDestination(waypointTransform.position);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.transform == waypointTransform) {
        GetNewDestination();
        }
    }

    private bool DetectPlayer() {
        Vector3 direction = playerTransform.position - centerEye.position;
        direction = direction.normalized;
        float angle = Vector3.Angle(transform.forward, direction);

        if(angle <= 45) {
            RaycastHit raycasthit;
            Debug.DrawRay(centerEye.position, direction * 100, Color.red, 1f);

            if(Physics.Raycast(centerEye.position, direction, out raycasthit)) {
                Debug.Log($"Hit : {raycasthit.transform.name}");
                if(raycasthit.transform == playerTransform){
                    return true;
                }
            }
        }
        return false;
    }

   void UpdateWalking(){
        if(DetectPlayer()){
            status = State.chasing;
        }
    }

    void UpdateChasing(){
        agent.SetDestination(playerTransform.position);
        if(!DetectPlayer()){
            status = State.walking;
            GetNewDestination();
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch(status)
        {
            case State.walking: UpdateWalking(); break;
            case State.chasing: UpdateChasing(); break;
        }

        if(DetectPlayer()) {
            agent.SetDestination(playerTransform.position);
        }
    }
}
