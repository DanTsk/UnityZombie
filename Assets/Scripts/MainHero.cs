using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MainHero : MonoBehaviour {
    public GameObject gun;
    public GameObject barell;
    public float ShootRange;
    public float ShootDamage;

    public float timeBetweenShoots;
    public int maxAmmo;


    RaycastHit mousePoint;
    Ray mouseWrapper;

    float currentTime;
    float oneReloadLength;
    int zombieMask;
    int currentAmmo;


    Animator animator;
    ParticleSystem gunParticles;
    RaycastHit shootHit;

    IEnumerator reloadState,afterState;
    Mode currentMode;


    enum Mode {
        Shooting,
        Reloading,
        Idle
    }


    void Start()
    {
        animator = this.GetComponent<Animator>();
        gunParticles = barell.GetComponent<ParticleSystem>();

        zombieMask = LayerMask.GetMask("Zombie");

        currentAmmo = maxAmmo;
        currentTime = 0f;

        animationClipInfo();
    }



    void Update()
    {
      
        updateLooking(mouseInWorld().point);
        updateShooting(mouseInWorld().collider);
        updateReload();
    }


    RaycastHit mouseInWorld() {
        mouseWrapper = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(mouseWrapper, out mousePoint))
        {          
            return mousePoint;
        }

        return new RaycastHit();
    }



    void updateLooking(Vector3 way) {

        Vector3 lookAt = new Vector3(way.x, this.transform.position.y, way.z);

        if (way != Vector3.zero && way.z > 5f)
        {
            this.transform.LookAt(lookAt);
            this.barell.transform.LookAt(lookAt);
        }
    }

    void updateShooting(Collider collider) {
        currentTime -= Time.deltaTime;

        if (Input.GetMouseButtonDown(0) && currentTime <= 0 && currentAmmo > 0) {
            currentTime = timeBetweenShoots;
            currentAmmo--;
            currentMode = Mode.Shooting;


            animator.SetTrigger("Shoot");
            cancelReload();

            gunParticles.Stop();
            gunParticles.Play();


            ZombieHittable enemy = collider.GetComponent<ZombieHittable>();

            if (enemy)
            {
                enemy.onPartHitted(10f);
            }
        }
    }

    void updateReload() {
        if (currentAmmo < maxAmmo && currentMode != Mode.Reloading) {
            currentMode = Mode.Reloading;
            afterState = afterShoot();
            StartCoroutine(afterState);      
        }
    }




    void startReload() {
        animator.SetBool("Reload", true);     

        reloadState = reloadCoroutine(maxAmmo - currentAmmo);
        StartCoroutine(reloadState);
    }

    void cancelReload() {
        if (reloadState != null) {
            animator.SetBool("Reload", false);
            StopCoroutine(reloadState);          
        }

        if (afterState != null) {
            StopCoroutine(afterState);
        }


    }




    IEnumerator reloadCoroutine(int diffrenence) {
        for (int i = 0; i < diffrenence; i++)
        {
            yield return new WaitForSeconds(oneReloadLength);     
            currentAmmo++;
        }

        currentMode = Mode.Idle;
        animator.SetBool("Reload", false);
    }

    IEnumerator afterShoot() {
        yield return new WaitForSeconds(shootAnimationLength - 0.05f);
        startReload();
    }



    void animationClipInfo() {
        RuntimeAnimatorController ac = animator.runtimeAnimatorController;
        for (int i = 0; i < ac.animationClips.Length; i++)
        {
            if (ac.animationClips[i].name == "Character_Shotgun_Reload")
            {
                oneReloadLength =  ac.animationClips[i].length + 0.10f;                
            }
            else if(ac.animationClips[i].name == "Character_Shotgun_Shoot")
            {
                shootAnimationLength = ac.animationClips[i].length + 0.10f;
            }            
        }
    }

    float shootAnimationLength;

}


