using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour {
    
    private Vector3 _position;

    public void Start() {
        _position = gameObject.transform.position;
    }

    public GameObject SpawnZombie(GameObject zombie, int health) {
        GameObject go = Instantiate(zombie, _position, Quaternion.identity) as GameObject;
        go.GetComponent<ZombieHandler>().SetHealth(health);
        return go;
    }

}
