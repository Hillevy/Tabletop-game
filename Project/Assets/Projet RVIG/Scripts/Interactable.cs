using System.Collections;
using System.Collections.Generic;
using Leap;
//using NUnit.Framework.Constraints;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public GameObject Cursor;

    protected  Virtual3DTrackerLeap LeapTracker;
    protected float grabSpeed = 20f;
    protected bool isGrabbing;
    protected bool Grabbed ;
    protected bool Released;
    protected bool Highlighted;
    protected Renderer rend;
    protected Collider coll;
    protected Vector3 oldPos;

    protected abstract void highlight();
    protected abstract void grab();
    protected abstract void release();

    virtual protected void Start()
    {
        LeapTracker = (Virtual3DTrackerLeap)Cursor.GetComponent<Virtual3DTrackerLeap>();
        rend = Cursor.GetComponent<Renderer>();
        coll = this.GetComponent<Collider>();
        coll.bounds.Expand(100);
        Released = true;
        Grabbed = false;
        isGrabbing = false;
        Highlighted = false;
    }

    virtual protected void Update()
    {
        if (coll.bounds.Contains(Cursor.transform.position))
        {
            rend.material.color = Color.blue;
            Highlighted = true;
            highlight();

            if (LeapTracker.Grab && Released && rend.enabled)
            {
                isGrabbing = true;
                Grabbed = true;
            }
        }
        else
        {
            if (Highlighted)
            {
                Highlighted = false;
                rend.material.color = Color.red;
            }
        }
        if (!LeapTracker.Grab)
            Grabbed = false;

        if (Grabbed)
        {
            if (isGrabbing)
            {
                SmoothGrab();
            }
            else
            {
                grab();       
            }
            Released = false;

        }
        else if(!Released)
        {
            release();
            Released = true;
            isGrabbing = false;
        }
    }

    protected void SmoothGrab()
    {
        //Move the pawn to the palm
        this.transform.position = Vector3.Slerp(this.transform.position, Cursor.transform.position,
            grabSpeed*Time.deltaTime);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Cursor.transform.rotation,
            grabSpeed*Time.deltaTime);
        if (Vector3.Distance(this.transform.position,Cursor.transform.position)<=0.2f)
        {
            isGrabbing = false;
        }
    }
}
