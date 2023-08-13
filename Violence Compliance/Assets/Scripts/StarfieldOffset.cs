using UnityEngine;

public class StarfieldOffset : MonoBehaviour {

    [SerializeField] private Material material;
    [SerializeField] private GameObject player;
    [SerializeField] private float offsetSpeed;    

    void Update(){
        if(SceneChangeManager.Instance.currentScene == 1) {
            material.mainTextureOffset += new Vector2(0, offsetSpeed * Time.deltaTime);
        }

        else {
            material.mainTextureOffset = player.transform.position * offsetSpeed;
        }
    }
}
