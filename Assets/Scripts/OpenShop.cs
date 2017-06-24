using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenShop : MonoBehaviour {
    public UI2DSprite shop;

	// Use this for initialization
	void Start () {
        shop.enabled = false;
        shop.UpdateVisibility(false, false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
