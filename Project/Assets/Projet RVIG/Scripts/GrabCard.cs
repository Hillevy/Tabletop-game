using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabCard : Interactable {

    public GameObject indicator;
    public AudioClip pick;
    public AudioClip place;

    private bool front;
    private bool back;
    private Quaternion rot;
    private Vector3 oldPosCard;
    private Quaternion oldRot;
    private Vector3 newPos;
    private Quaternion newRot;
    private bool isSnapping;
    private float speed;
    private AudioSource audio;
    private bool touch;

    // Use this for initialization
    protected override void Start ()
    {
        base.Start();
        //Cursor which indicates where the card will be snapped
        indicator = Instantiate(indicator, new Vector3(1000, 1000, 1000), Quaternion.identity);
        //Position of the card
        oldPosCard = this.transform.position;
        //Rotation of the card
        oldRot = this.transform.rotation;
        rot = new Quaternion(0,0, Mathf.PI, 0);
        //Smooth snapping
        isSnapping = false;
        speed = 10f;
        //Audio when snapping
        audio = GetComponent<AudioSource>();
        touch = false;
    }

    protected override void Update()
    {
        base.Update();
        /* When the user releases the pawn, the latter moves smoothly toward its new position
           and stops when it has reached its destination */
        if (isSnapping)
        {
            if (Grabbed)
            {
                isSnapping = false;
            }
            else
            {
                this.transform.position = Vector3.Slerp(this.transform.position, newPos, speed*Time.deltaTime);
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, newRot, speed*Time.deltaTime);
                if (Vector3.Distance(this.transform.position, newPos) <= 0.05f && Quaternion.Angle(this.transform.rotation, newRot)<=0.05f)
                {
                    this.transform.position = newPos;
                    this.transform.rotation = newRot;
                    oldPosCard = newPos;
                    oldRot = newRot;
                    isSnapping = false;
                    audio.clip = place;
                    audio.Play();
                }
            }
        }
    }

    protected override void highlight()
    {
    }

    protected override void grab()
    {
        rend.enabled = false;

        //The card moves with the hand
        this.transform.SetPositionAndRotation(Cursor.transform.position, Cursor.transform.rotation);

        //We determine the face of the card we see (front or back)
        if (Cursor.transform.up.y >= 0.0f)
        {
            front = true;
        }
        else if (Cursor.transform.up.y < 0.0f)
        {
            front = false;
        }

        Ray ray = new Ray(this.transform.position, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000, LayerMask.GetMask("Cards")))
        {
            indicator.transform.position = hit.transform.position + new Vector3(0, hit.collider.bounds.extents.y, 0);
        }
        else
        {
            indicator.transform.position = new Vector3(1000, 1000, 1000);
        }

        if (!touch)
        {
            audio.clip = pick;
            audio.Play();
            touch = true;
        }
    }

    protected override void release()
    {
        rend.enabled = true;
        indicator.transform.position = new Vector3(1000, 1000, 1000);

        Ray ray = new Ray(this.transform.position, Vector3.down);
        RaycastHit hit;
        /* If the card is held above a valid place, it will be snapped on it.
         * The rotation applied by the user is kept.
           Otherwise the card goes back to its previous position and rotation */
        if (Physics.Raycast(ray, out hit, 1000, LayerMask.GetMask("Cards")))
        {
            //this.transform.position = hit.transform.position + new Vector3(0, hit.collider.bounds.extents.y * 2, 0);
            newPos = hit.transform.position + new Vector3(0, hit.collider.bounds.extents.y * 2+0.08f, 0);
            if (front)
            {
                //this.transform.rotation = Quaternion.identity;
                newRot = Quaternion.identity;
            }
            else
            {
                //this.transform.rotation = rot;
                newRot = rot;
            }
            
        }
        else
        {
            //this.transform.position = oldPos;
            //this.transform.rotation = oldRot;
            newPos = oldPosCard;
            newRot = oldRot;
        }
        isSnapping = true;
        touch = false;
    }
}
