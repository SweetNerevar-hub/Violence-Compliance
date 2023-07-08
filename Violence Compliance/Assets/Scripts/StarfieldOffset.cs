using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarfieldOffset : MonoBehaviour {

    [SerializeField] private Material material;
    [SerializeField] private GameObject player;
    [SerializeField] private float offsetSpeed;    

    // Update is called once per frame
    void Update(){

        material.mainTextureOffset = player.transform.position * offsetSpeed;

    }
}
