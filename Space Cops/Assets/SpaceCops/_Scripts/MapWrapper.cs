using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapWrapper : MonoBehaviour {
    public Transform boundaryLeft;
    public Transform boundaryRight;
    public Transform boundaryTop;
    public Transform boundaryBottom;

	// Update is called once per frame
	void Update () {
        Vector3 pos = transform.position;
        float distanceLeft = Vector3.Distance(pos, boundaryLeft.position);
        float distanceRight = Vector3.Distance(pos, boundaryRight.position);
        float distanceTop = Vector3.Distance(pos, boundaryTop.position);
        float distanceBottom = Vector3.Distance(pos, boundaryBottom.position);
        if (distanceLeft < 15f)
        {
            pos = boundaryRight.position;
        }
        else
        {
            pos = transform.position;
        }
        if (distanceRight < 15f)
        {
            pos = boundaryLeft.position;
        }
        else
        {
            pos = transform.position;
        } 
        if (distanceTop < 15f)
        {
            pos = boundaryBottom.position;
        }
        else
        {
            pos = transform.position;
        }
        if (distanceBottom < 15f)
        {
            pos = boundaryTop.position;
        }
        else
        {
            pos = transform.position;
        }
        transform.position = pos;
    }
}
