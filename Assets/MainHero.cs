using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MainHero : MonoBehaviour {
    public GameObject gun;
    public GameObject barell;

    RaycastHit mousePoint;
    Ray mouseWrapper;
    
    Animator animator;

    ParticleSystem gunParticles;
    Light gunLight;
    Ray bulletRay;

    void Start()
    {
        animator = this.GetComponent<Animator>();
        gunParticles = barell.GetComponent<ParticleSystem>();    
    }

    
    
    void Update()
    {
        Vector3 currentPonit = mouseInWorld();
        updateLooking(currentPonit);
        updateShooting(currentPonit);
    }


    Vector3 mouseInWorld() {
        mouseWrapper = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(mouseWrapper, out mousePoint))
        {
            return mousePoint.point;
        }

        return Vector3.zero;
    }


    void updateLooking(Vector3 way) {    

        Vector3 lookAt = new Vector3(way.x, this.transform.position.y, way.z);

        if (way != Vector3.zero && way.z > 5f)
        {
            this.transform.LookAt(lookAt);       
        }
    }

    void updateShooting(Vector3 point) {    
        if (Input.GetMouseButtonDown(0)) {
            animator.SetTrigger("Shoot");

            gunParticles.Stop();
            gunParticles.Play();
       

            bulletRay.origin = transform.position;
            bulletRay.direction = point;
        
        }
    }

   
}


