using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Door : MonoBehaviour
{
    public Vector3 decal;
    private Vector3 openPosition;
    private Vector3 closedPositon;

    public float doorTime = 1f;
    public float openTime = 2f;

    public UnityEvent whenOpen;
    public UnityEvent whenClosed;


    public enum State {open, closed, opening, closing}
    public State status = State.closed;

    IEnumerator OpeningDoor(){
        status = State.opening;
        Vector3 startPosition = transform.position;
        float time = 0f;
        while(time < doorTime){
            time += Time.deltaTime;
            float ratio = time/doorTime;
            transform.position = Vector3.Lerp(startPosition, openPosition, ratio); 
            yield return null;
        }

        transform.position = openPosition;
        status = State.open;
        yield return new WaitForSeconds(openTime);
        CloseDoor();
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
        StartCoroutine(ClosingDoor());
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")){
            if(status == State.closed) {
                OpenDoor();
                }
            else if(status == State.closing){
                StopAllCoroutines();
                OpenDoor();
            }
        }
    }
}