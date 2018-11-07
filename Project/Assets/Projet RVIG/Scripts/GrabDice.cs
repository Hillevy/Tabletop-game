using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GrabDice : Interactable
{

    public GameObject Plane;
    public Vector3 velocityDampen;

    private Rigidbody rigidB;

    private float velocityThreshold = 15f;
    private float trayThreshold = 1f;
    private bool Rotating;
    private Quaternion targetQuaternion;
    private Vector3 lastPos;
    private Vector3 velocity;
    private float rotTime = 50;
    private float timer;

    private Collider cursorColl;

    //Dice tray
    public GameObject Tray;
    private Collider trayCubeColl;
    private GameObject trayCube;
    private GameObject trayPlaneN;
    private GameObject trayPlaneE;
    private GameObject trayPlaneS;
    private GameObject trayPlaneW;
    private Renderer trayCubeRend;
    private Renderer trayPlaneNRend;
    private Renderer trayPlaneERend;
    private Renderer trayPlaneSRend;
    private Renderer trayPlaneWRend;

    private void Awake()
    {
        rigidB = this.gameObject.AddComponent<Rigidbody>();
    }
    protected override void Start()
    {

        trayCube = Tray.transform.Find("trayCube").gameObject;
        trayPlaneN = Tray.transform.Find("planeN").gameObject;
        trayPlaneE = Tray.transform.Find("planeE").gameObject;
        trayPlaneS = Tray.transform.Find("planeS").gameObject;
        trayPlaneW = Tray.transform.Find("planeW").gameObject;
        trayCubeColl = trayCube.GetComponent<Collider>();
        trayCubeRend = trayCube.GetComponent<Renderer>();
        trayPlaneNRend = trayPlaneN.GetComponent<Renderer>();
        trayPlaneERend = trayPlaneE.GetComponent<Renderer>();
        trayPlaneSRend = trayPlaneS.GetComponent<Renderer>();
        trayPlaneWRend = trayPlaneW.GetComponent<Renderer>();

        LeapTracker = (Virtual3DTrackerLeap)Cursor.GetComponent<Virtual3DTrackerLeap>();

        rend = Cursor.GetComponent<Renderer>();

        coll = this.gameObject.AddComponent<BoxCollider>();
        coll.material = new PhysicMaterial();
        coll.material.bounciness = 0.55f;
        coll.bounds.Encapsulate(rend.bounds);
        coll.bounds.Expand(100);

        Released = true;
        Grabbed = false;

        
        rigidB.mass = 0.5f;
        //rigidB.isKinematic = true;

        targetQuaternion = this.transform.rotation;
        lastPos = this.transform.position;
    }

    protected override void highlight()
    {
        //rend.material.color = Color.blue;
    }

    protected override void grab()
    {
        rigidB.useGravity = false;  //disable gravity to avoid downwards velocity spike
        rend.enabled = false;       //disable cursor for better dice visibility

        this.transform.position = Cursor.transform.position; //Dice position to cursor
        
        //calculate velocity
        timer += Time.deltaTime;
        velocity = (this.transform.position - lastPos) / Time.deltaTime;

        //check hand shake
        if (velocity.x >= velocityThreshold || velocity.y >= velocityThreshold || velocity.z >= velocityThreshold)
        {
            targetQuaternion = Random.rotation;
            Rotating = true;
            timer = 0;
        }
        if ( (Rotating && timer < rotTime && targetQuaternion == this.transform.rotation))
        {
            targetQuaternion = Random.rotation;
        }
        if (targetQuaternion != this.transform.rotation)
        {
            this.transform.rotation = Quaternion.Slerp(transform.rotation, targetQuaternion, Time.deltaTime * 4);
        }

        
        lastPos = this.transform.position;

        // Check tray
        if (!trayCubeColl.bounds.Contains(this.transform.position))
        {
            trayPlaneNRend.material.color = new Color(1, 0, 0, 0.5f);
            trayPlaneERend.material.color = new Color(1, 0, 0, 0.5f);
            trayPlaneSRend.material.color = new Color(1, 0, 0, 0.5f);
            trayPlaneWRend.material.color = new Color(1, 0, 0, 0.5f);
        }
        else
        {
            float distanceN = Mathf.Abs(trayPlaneN.transform.position.z - this.transform.position.z);
            float distanceE = Mathf.Abs(trayPlaneE.transform.position.x - this.transform.position.x);
            float distanceS = Mathf.Abs(trayPlaneS.transform.position.z - this.transform.position.z);
            float distanceW = Mathf.Abs(trayPlaneW.transform.position.x - this.transform.position.x);
            if (distanceN < trayThreshold || distanceE < trayThreshold || distanceS < trayThreshold || distanceW < trayThreshold)
            {
                float alpha = (1-Mathf.Min(distanceW, distanceN, distanceE, distanceS))/2;
                trayPlaneNRend.material.color = new Color(1, 0, 0, alpha);
                trayPlaneERend.material.color = new Color(1, 0, 0, alpha);
                trayPlaneSRend.material.color = new Color(1, 0, 0, alpha);
                trayPlaneWRend.material.color = new Color(1, 0, 0, alpha);
            }
            else
            {  
                trayPlaneNRend.material.color = new Color(1, 0, 0, 0f);
                trayPlaneERend.material.color = new Color(1, 0, 0, 0f);
                trayPlaneSRend.material.color = new Color(1, 0, 0, 0f);
                trayPlaneWRend.material.color = new Color(1, 0, 0, 0f);
            }

        }
    }

    protected override void release()
    {
        velocity.Scale(velocityDampen);
        rigidB.velocity = velocity;
        rend.enabled = true;
        rigidB.useGravity = true;
        trayCubeRend.material.color = new Color(1, 0, 0, 0f);
        trayPlaneNRend.material.color = new Color(1, 0, 0, 0f);
        trayPlaneERend.material.color = new Color(1, 0, 0, 0f);
        trayPlaneSRend.material.color = new Color(1, 0, 0, 0f);
        trayPlaneWRend.material.color = new Color(1, 0, 0, 0f);
    }
}
