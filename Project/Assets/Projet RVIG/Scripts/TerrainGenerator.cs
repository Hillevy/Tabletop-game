using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{

    public int width;
    public int heigth;
    public GameObject TilePrefab;
    public GameObject PawnPrefab;
    public Vector3 Offset;

    // Use this for initialization
    void Start ()
    {
		generateTerrain();
    }

    private void generateTerrain()
    {
        float hexHeight = 2;
        float hexWidth = Mathf.Sqrt(3) / 2 * hexHeight;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < heigth; y++)
            {
                GameObject go;
                if (y % 2 == 0)
                {
                    go = (GameObject)Instantiate(TilePrefab, new Vector3(x * hexWidth + 0.5f * hexWidth, 0, y * 3 * hexHeight / 4), Quaternion.Euler(-90, 0, 0));
                    go.transform.position += Offset;
                    //go.GetComponent<Renderer>().material.color = Random.ColorHSV();
                    go.transform.localScale = new Vector3(1,1,Random.Range(1,3));
                    go.layer = 8;
                }
                else if (y % 2 == 1)
                {
                    go = (GameObject)Instantiate(TilePrefab, new Vector3(x * hexWidth, 0, y * 3 * hexHeight / 4), Quaternion.Euler(-90, 0, 0));
                    go.transform.position += Offset;
                    //go.GetComponent<Renderer>().material.color = Random.ColorHSV();
                    go.transform.localScale = new Vector3(1, 1, Random.Range(1, 3));
                    go.layer = 8;
                }       
            }
        }
    }

}
