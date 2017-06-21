using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour {

    public static LevelController Instance;

    public Vector3 destination;

    void Awake () {
        Instance = this;
        destination = new Vector3(87f, 0, 16f);
    }

    void Start() {
     
    }

}
