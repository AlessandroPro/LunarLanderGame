using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Represents a platform that the lander can land on to win the player points
public class Platform : MonoBehaviour
{
    public int value = 0;
    public float size = 0;

    // Places the platform at the specificed pos and with the specified platformSize
    public void Place(Vector3 pos, float platformSize)
    {
        transform.position = pos;

        var scaleY = transform.localScale.y;
        var scaleZ = transform.localScale.z;
        transform.localScale = new Vector3(platformSize, scaleY, scaleZ);
        var psShape = GetComponentInChildren<ParticleSystem>().shape;
        psShape.radius *= platformSize;

        float platformValue = (1 / platformSize) * 120f;
        value = (int)platformValue;
    }
}
