using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;

public class GrabPawn : Interactable
{
    public GameObject indicator;

    private AudioSource audio;
    private Vector3 newPos;
    private bool isSnapping;
    private float speed;

    protected override void Start()
    {
        base.Start();
        //Purple sphere appearing where the pawn will be snapped when the user releases it :
        indicator = Instantiate(indicator, new Vector3(1000, 1000, 1000), Quaternion.identity); 
        oldPos = this.transform.position;
        isSnapping = false;
        speed = 10f;
        //Audio when snapping
        audio = GetComponent<AudioSource>();
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
                this.transform.position = Vector3.Slerp(this.transform.position, newPos, speed * Time.deltaTime);
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.identity, speed * Time.deltaTime);
                if (Vector3.Distance(this.transform.position,newPos)<=0.05f)
                {
                    this.transform.position = newPos;
                    oldPos = newPos;
                    isSnapping = false;
                    audio.Play();
                }
            }
        }
    }

    protected override void highlight()
    {
        rend.material.color = Color.blue;
    }
    protected override void grab()
    {
        rend.enabled = false;
        //Move the pawn with the hand
        this.transform.SetPositionAndRotation(Cursor.transform.position, Cursor.transform.rotation);
        

        Ray ray = new Ray(this.transform.position, Vector3.down);
        RaycastHit hit;
        //The indicator appears on the destination when the pawn is above a valid position, otherwise it doesn't
        if (Physics.Raycast(ray, out hit, 1000, LayerMask.GetMask("Tiles")))
        {
            indicator.transform.position = hit.transform.position + new Vector3(0, hit.collider.bounds.extents.y, 0);
        }
        else
        {
            indicator.transform.position = new Vector3(1000,1000,1000);
        }
    }

    protected override void release()
    {
        rend.enabled = true;
        indicator.transform.position = new Vector3(1000,1000,1000);

        Ray ray = new Ray(this.transform.position, Vector3.down);
        RaycastHit hit;
        //We save the new position in order to move the pawn in Update()
        if (Physics.Raycast(ray, out hit, 1000, LayerMask.GetMask("Tiles")))
        {        
            //oldPos = this.transform.position;
            newPos = hit.transform.position + new Vector3(0, hit.collider.bounds.extents.y * 2, 0);
        }
        else
        {
            newPos = oldPos;
        }
        isSnapping = true;
    }
}
