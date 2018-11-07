using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;

public class MoveCamera : MonoBehaviour {

    public HandController handController;
    public GameObject goHandController;

    private Vector3 direction;
    private HandModel handModel;
    private Hand hand;
    private Finger finger0;
    private Finger finger1;
    private Finger finger2;
    private Finger finger3;
    private Vector pos;
    private Vector oldPos;
    private Vector velocity;
    private Vector3 handControllerOldPos;
    private Vector3 camPos;
    private Vector3 pivot;
    private Quaternion camRot;
    private Quaternion oldCamRot;
    private float speed;

    // Use this for initialization
    void Start ()
    {
        pos = new Vector();
        oldPos = new Vector();
        camPos = this.transform.position;
        camRot = this.transform.rotation;
        oldCamRot = camRot;
        handControllerOldPos = goHandController.transform.position;
        pivot = Vector3.zero;
    }
    
    // Update is called once per frame
    void LateUpdate () {
        if (handController.GetAllGraphicsHands().Length != 0)
        {
            handModel = handController.GetAllGraphicsHands()[0];
            hand = handModel.GetLeapHand();
            finger0 = hand.Fingers[0]; //thumb
            finger1 = hand.Fingers[1]; //index
            finger2 = hand.Fingers[2]; //middle finger
            finger3 = hand.Fingers[3]; //ring finger
            pos = hand.PalmPosition;
            camRot = this.transform.rotation;
            //movement and rotation speed
            velocity = hand.PalmVelocity;
            speed = velocity.Magnitude / 50f;

            /* The transformations are based on the palm position and movement. 
               The fingers are used to create new commands */

            Debug.DrawRay(this.GetComponentInChildren<Camera>().transform.position, Vector3.forward);

            //Extend index to activate camera movement
            if (finger1.IsExtended && !finger2.IsExtended && !finger0.IsExtended)
            {
                moveCamera();
            }

            //Extend index and middle finger to activate camera rotation
            if (finger1.IsExtended && finger2.IsExtended && !finger3.IsExtended && !finger0.IsExtended)
            {
                rotateCamera();
            }
            oldPos = pos;
            oldCamRot = camRot;
        }
    }

    //Translates the camera along the X or Z axes
    void moveCamera()
    {
        float diffX = Mathf.Abs(pos.x - oldPos.x);
        float diffZ = Mathf.Abs(pos.z - oldPos.z);

        /*Translation along z*/
        if (diffZ >= diffX)
        {
            direction = this.transform.forward.normalized;
            if (pos.z >= oldPos.z + 0.1f)
            {
                this.transform.position -= speed * Time.deltaTime * direction;
            }
            if (pos.z <= oldPos.z - 0.1f)
            {
                this.transform.position += speed * Time.deltaTime * direction;
            }
            pivot.z = this.transform.position.z - camPos.z;
        }
        /*Translation along x*/
        else
        {
            direction = this.transform.right.normalized;
            if (pos.x >= oldPos.x + 0.25f)
            {  
                this.transform.position += speed * Time.deltaTime * direction;
            }
            if (pos.x <= oldPos.x - 0.25f)
            {
                this.transform.position -= speed * Time.deltaTime * direction;
            }
            pivot.x = this.transform.position.x - camPos.x;
        }

    }

    //Rotates the camera around the Y axis
    void rotateCamera()
    {
        float diffX = Mathf.Abs(pos.x - oldPos.x);
        float diffY = Mathf.Abs(pos.y - oldPos.y);

        if (diffX >= diffY)
        {
            
            if (pos.x >= oldPos.x + 0.25f)
            {
                transform.RotateAround(pivot, -Vector3.up, 5 * speed * Time.deltaTime);
            }
            if (pos.x <= oldPos.x - 0.25f)
            {
                transform.RotateAround(pivot, Vector3.up, 5 * speed * Time.deltaTime);
            }
        }
    }

}
