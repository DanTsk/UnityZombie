using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour, IEnemy {
    public float hitPoints;

    public GameObject headBlood,bodyBlood;
    public GameObject body;


    [Header("Particles of dead")]
    public GameObject soul,soulLight;

    Animator animator;
    ParticleSystem headParticles,bodyParticles, soulParticles;
    Light lightDeath;

    BoxCollider bodyCollider;
    Rigidbody rigidBody;

    NavMeshAgent agent;
    Mode currentMode;

    Vector3 moveDown;

    enum Mode
    {
        Dead,
        AfterDead,
        Walk,
        Hitted,
        Run,
        Floored
    }

  
    void Start () {
        agent = GetComponent<NavMeshAgent>();
        Debug.Log(LevelController.Instance.destination);
        agent.destination = LevelController.Instance.destination;

        rigidBody = GetComponent<Rigidbody>();

        animator = GetComponent<Animator>();
        headParticles = headBlood.GetComponent<ParticleSystem>();
        bodyParticles = bodyBlood.GetComponent<ParticleSystem>();
        bodyCollider = body.GetComponent<BoxCollider>();

        lightDeath = soulLight.GetComponent<Light>();


        soulParticles = soul.GetComponent<ParticleSystem>();
        moveDown = new Vector3(0, -0.03f, 0); 

        currentMode = Mode.Walk;
    }
        
    void Update () {
        if (hitPoints <= 0 && currentMode != Mode.Dead && currentMode != Mode.AfterDead) {
            die();
        }

        if (currentMode == Mode.AfterDead)
        {
            transform.Translate(moveDown);

            if (this.transform.position.y <= -2f) {
                Destroy(this.transform.parent.gameObject);
            }
        }
	}

 
    public void die()
    {
    

        soulParticles.Stop();
        soulParticles.Play();
        lightDeath.enabled = true;

        currentMode = Mode.Dead;
        animator.SetBool("dead",true);
        agent.speed = 0;

        StartCoroutine(afterDead());
    }

    public void hit(string part, float dps, Vector3 point)
    {

        if (part == "Head")
        {
            headParticles.Stop();
            headParticles.Play();
            animator.SetTrigger("shot");
        }
        else if (part == "Leg")
        {
            if (currentMode != Mode.Floored)
            {
                setOnFloor();              
            }          
        }
        else
        {           
            bodyParticles.transform.position = point;

            bodyParticles.Stop();
            bodyParticles.Play();

            animator.SetTrigger("shot");
        }

        hitPoints -= dps;
    }

    public void setOnFloor() {

        currentMode = Mode.Floored;
        agent.speed = 0.4f;

        animator.SetTrigger("onfloor");
        animator.SetBool("walk",false);


        Vector3 centerBody = bodyCollider.center;
        centerBody.z = -0.14f;

        bodyCollider.center = centerBody;

        centerBody = bodyCollider.size;
        centerBody.z = 1.1f;

        bodyCollider.size = centerBody;
    }

    IEnumerator afterDead() {
        yield return new WaitForSeconds(1.2f);
        currentMode = Mode.AfterDead;
         rigidBody.isKinematic = true;
         agent.enabled = false;     
    }

    IEnumerator afterGrenade() {
        yield return new WaitForSeconds(.3f);
        die();
    }

    private void OnTriggerEnter(Collider other)
    {

        if(other.gameObject.layer == 9)
        {
            if (currentMode != Mode.Dead)
            {
                currentMode = Mode.Dead;
                StartCoroutine(afterGrenade());
            }
        }
               
    }
}
