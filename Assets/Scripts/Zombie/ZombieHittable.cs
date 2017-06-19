using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieHittable : MonoBehaviour {

    public GameObject zombie;
    public string part;

    Zombie zombieController;

    private void Awake()
    {
        zombieController = zombie.GetComponent<Zombie>();
    }

   
    public void onPartHitted(float dps)
    {
        float exact = dps;
  
        if (part == "Head") exact *= 2.5f;
        else if (part == "Leg") exact /= 2f;

        zombieController.hit(part,dps);
    }

}
