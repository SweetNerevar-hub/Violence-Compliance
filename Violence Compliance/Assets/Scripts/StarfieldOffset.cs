using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarfieldOffset : MonoBehaviour {

    [SerializeField] private Material material;
    [SerializeField] private GameObject player;
    [SerializeField] private float offsetSpeed;    

    // Update is called once per frame
    void Update(){

        if(SceneChangeManager.Instance.currentScene == 0) {
            material.mainTextureOffset += new Vector2(0, offsetSpeed * Time.deltaTime);
        }

        else {
            material.mainTextureOffset = player.transform.position * offsetSpeed;
        }
    }
}
