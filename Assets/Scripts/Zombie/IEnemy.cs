using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy {

    void die();
    void hit(string part, float dps,Vector3 point);
      
}
