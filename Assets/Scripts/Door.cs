using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Vector3 decal;
    private Vector3 openPosition;
    private Vector3 closedPositon;

    public float doorTime = 1f;


    public enum State {open, closed, opening, closing}
    public State status = State.closed;

    IEnumerator OpeningDoor(){
        status = State.opening;
        float time = 0f;
        while(time < doorTime){
            time += Time.deltaTime;
            float ratio = time/doorTime;
            transform.position = Vector3.Lerp(closedPositon, openPosition, ratio); 
            yield return null;
        }

        transform.position = openPosition;
        status = State.open;
    }
IEnumerator ClosingDoor(){
        status = State.closing;
        float time = 0f;
        while(time < doorTime){
            time += Time.deltaTime;
            float ratio = time/doorTime;
            transform.position = Vector3.Lerp(openPosition, closedPositon, ratio); 
            yield return null;
        }

        transform.position = closedPositon;
        status = State.closed;
    }


    // Start is called before the first frame update
    void Start()
    {
        closedPositon = transform.position;
        openPosition = transform.position + decal;
    }

    // Update is called once per frame
    void Update()
    {
    }


    private void OpenDoor(){
        StartCoroutine(OpeningDoor());
    }

    private void CloseDoor(){
        transform.position = closedPositon;
        status = State.closed;
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")){
            if(status == State.closed) {
                OpenDoor();
                }
            else if(status == State.open){
                CloseDoor();
            }
        }
    }
}