using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A component piece on the Lunar Lander
public class LanderComponent : MonoBehaviour
{
    public bool breakOnCollision;
    private bool isDestroyed;

    // Start is called before the first frame update
    void Start()
    {
        isDestroyed = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Collidable>() && !isDestroyed)
        {
            float angle = transform.parent.transform.rotation.eulerAngles.z;
            float speed = collision.relativeVelocity.magnitude;

            // The lander will crash if it touches the moon's surface under any of these conditions
            if((angle < 350f && angle > 10f) || breakOnCollision || speed > 4)
            {
                this.gameObject.GetComponentInParent<LunarLander>().Crash();
            }
        }
    }

    // Break off from the main body of the lunar lander and apply a random force and torque
    public void BreakOff()
    {
        FixedJoint[] joints = GetComponents<FixedJoint>();
        foreach(FixedJoint joint in joints)
        {
            Destroy(joint);
        }

        isDestroyed = true;

        Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>();
        if (rigidbody != null)
        {
            float randomForceX = Random.Range(-10f, 10f);
            float randomForceY = Random.Range(3f, 15f);
            float randomTorqueForce = Random.Range(-10f, 10f);

            Vector3 collisionForce = new Vector3(randomForceX, randomForceY, 0f);
            Vector3 torqueForce = new Vector3(0f, 0f, randomTorqueForce);
            rigidbody.AddForce(collisionForce);
            rigidbody.AddTorque(torqueForce);
        }
    }
}
