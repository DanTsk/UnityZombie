using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MainHero : MonoBehaviour {
    public GameObject gun;
    public GameObject barell;
    public GameObject shootEnd;
    public GameObject grenade,grenadePlace,grenadeMesh;

    public UI2DSprite grenade1;
    public UI2DSprite grenade2;
    public UI2DSprite grenade3;

    public float ShootRange;
    public float ShootDamage;

    public float timeBetweenShoots;
    public int maxAmmo;
    public int grenades;

    RaycastHit mousePoint;
    Ray mouseWrapper;

    float currentTime;
    float oneReloadLength;
    int zombieMask;
    int currentAmmo;

    MeshRenderer gunMesh,grenMesh;
    Animator animator;
    ParticleSystem gunParticles,groundGun;
    TrailRenderer grenadeTrail;

    RaycastHit shootHit;

    IEnumerator reloadState,afterState;
    Mode currentMode;


    enum Mode {
        Shooting,
        Reloading,
        Grenade,
        Idle
    }


    void Start()
    {
        animator = this.GetComponent<Animator>();
        gunParticles = barell.GetComponent<ParticleSystem>();
        groundGun = shootEnd.GetComponent<ParticleSystem>();

        gunMesh = gun.GetComponent<MeshRenderer>();
        grenMesh = grenadeMesh.GetComponent<MeshRenderer>();
        grenadeTrail = grenadeMesh.GetComponent<TrailRenderer>();

        grenMesh.enabled = false;
        grenadeTrail.enabled = false;

        zombieMask = LayerMask.GetMask("Zombie");

        currentAmmo = maxAmmo;
        currentTime = 0f;

        animationClipInfo();

     
    }



    void Update()
    {
        RaycastHit hit = mouseInWorld();

        updateLooking(hit.point);
        updateShooting(hit);
        updateGrenade(hit);
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

        if (way != Vector3.zero && way.z > 5f && currentMode != Mode.Grenade)
        {            
            this.transform.LookAt(lookAt);
            this.barell.transform.LookAt(lookAt);
        }
    }

    void updateShooting(RaycastHit hit) {
        currentTime -= Time.deltaTime;

        if (hit.point.z < 5f) return;

        if (Input.GetMouseButtonDown(0) && currentTime <= 0 && currentAmmo > 0 && currentMode != Mode.Grenade) {
            currentTime = timeBetweenShoots;
            currentAmmo--;
            LevelController.Instance.onShooted(currentAmmo);
            currentMode = Mode.Shooting;

           
            animator.SetTrigger("Shoot");
            cancelReload();

            gunParticles.Stop();
            gunParticles.Play();
                       
            if (hit.collider != null)
            {
                ZombieHittable enemy = hit.collider.GetComponent<ZombieHittable>();

                if (enemy)
                {
                    enemy.onPartHitted(ShootDamage, hit.point);
                }
                else
                {
                    shootEnd.transform.position = hit.point;
                    groundGun.Stop();
                    groundGun.Play();
                }
                
            }

        }
    }

    void updateGrenade(RaycastHit hit) {
        if (hit.point.z < 5f) return;

        if (Input.GetMouseButtonDown(1) && currentMode != Mode.Grenade && grenades > 0)
        {
            grenades--;

            if (grenade3.enabled) grenade3.enabled = false;
            else if (grenade2.enabled) grenade2.enabled = false;
            else if (grenade1.enabled) grenade1.enabled = false;

            LevelController.Instance.onGrenadeThrow(grenades);

            currentMode = Mode.Grenade;
            cancelGranadeReload();

            animator.SetTrigger("Throw");
            gunMesh.enabled = false;

            GameObject obj = GameObject.Instantiate(this.grenade);
            obj.transform.parent = this.grenadePlace.transform.parent;
            obj.transform.position = this.grenadePlace.transform.position;
          
            StartCoroutine(grenadeCoroutine(obj, hit));           
        }
    }

    void updateReload() {
        if (currentAmmo < maxAmmo && currentMode != Mode.Reloading && currentMode != Mode.Grenade) {
            currentMode = Mode.Reloading;
            afterState = afterShoot();
            StartCoroutine(afterState);      
        }
    }


    Vector3 test = new Vector3(149f, 0f, 16f);


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

    void cancelGranadeReload() {
        if (reloadState != null)
        {           
            StopCoroutine(reloadState);
        }

        if (afterState != null)
        {
            StopCoroutine(afterState);
        }
    }



    IEnumerator reloadCoroutine(int diffrenence) {
        for (int i = 0; i < diffrenence; i++)
        {
            yield return new WaitForSeconds(oneReloadLength);     
            currentAmmo++;
            LevelController.Instance.onReloaded(currentAmmo);          
        }

        currentMode = Mode.Idle;
        animator.SetBool("Reload", false);
    }

    IEnumerator afterShoot() {
        yield return new WaitForSeconds(shootAnimationLength + 0.05f);
        startReload();
    }

    IEnumerator grenadeCoroutine(GameObject obj, RaycastHit hit) {
        yield return new WaitForSeconds(.5f);
        grenMesh.enabled = true;

        yield return new WaitForSeconds(.5f);        
        grenadeTrail.enabled = true;

        yield return new WaitForSeconds(.8f);
        grenMesh.enabled = false;
        grenadeTrail.enabled = false;

        obj.GetComponent<Grenade>().launchGrenade(hit.point);
       

        yield return new WaitForSeconds(.6f);
        gunMesh.enabled = true;
        currentMode = Mode.Idle;

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


