using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public GameObject particles;

    Rigidbody rigidBody;
    ParticleSystem explosion;
    MeshRenderer mesh;
    Vector3 target;

    Mode currentMode;

    enum Mode {
       none,
       grounded
    }

    void Start()
    {
        currentMode = Mode.none;
    }

    void Update()
    {
       
        if (Vector3.Distance(transform.position, target) < 0.55f && currentMode!= Mode.grounded) {
            currentMode = Mode.grounded;
            mesh.enabled = false;

            particles.transform.position = this.transform.position;
            
            this.rigidBody.isKinematic = true;
            explosion.Stop();
            explosion.Play();
        }
    }


    public void launchGrenade(Vector3 target)
    {
        this.target = target;

        rigidBody = this.GetComponent<Rigidbody>();
        explosion = particles.GetComponent<ParticleSystem>();
        mesh = this.GetComponent<MeshRenderer>();

        rigidBody.isKinematic = false;
        rigidBody.velocity = calculateGrenade(this.transform.position, target, 1f);


    }

    private Vector3 calculateGrenade(Vector3 origin, Vector3 target, float timeToTarget)
    {

        Vector3 toTarget = target - origin;
        Vector3 toTargetXZ = toTarget;
        toTargetXZ.y = 0;


        float y = toTarget.y;
        float xz = toTargetXZ.magnitude;

        float t = timeToTarget;
        float v0y = y / t + .5f * Physics.gravity.magnitude * t;
        float v0xz = xz / t;


        Vector3 result = toTargetXZ.normalized;
        result *= v0xz;
        result.y = v0y;

        return result;
    }

}