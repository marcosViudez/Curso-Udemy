using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    private Rigidbody _rigidody;

    public float moveForce;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        _rigidody = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // normalizar vector para magnitud uno del vector resultante
        Vector3 lookDirection = (player.transform.position - this.transform.position).normalized;

        _rigidody.AddForce(lookDirection * moveForce, ForceMode.Force);

        // destruir a los enemigos al caer
        // se puede hacer por trigger
        if(this.transform.position.y < -10)
        {
            Destroy(gameObject);
        }
    }
}
