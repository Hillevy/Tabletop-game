using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CastPosition : MonoBehaviour
{
    public GameObject prefab;
    public float dist = 10000.0f;

    private int mask;
    private RaycastHit hit;
    private Collider terrainCollider ;
    private GameObject currentCursor ;

	// Use this for initialization
	void Start ()
    {
	    currentCursor = GameObject.Instantiate(prefab, new Vector3(1000, 1000, 1000), Quaternion.identity);
	    mask = LayerMask.GetMask("Tiles");
    }
	
	// Update is called once per frame
	void Update ()
    {
        Debug.DrawRay(this.transform.position,Vector3.down);
        if (Physics.Raycast(this.transform.position,Vector3.down, out hit, dist, mask))
	    {
	        currentCursor.transform.position = hit.transform.position;
	    }
	}
}
