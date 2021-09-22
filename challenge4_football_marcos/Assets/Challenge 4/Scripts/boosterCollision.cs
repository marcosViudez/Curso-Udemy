using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boosterCollision : MonoBehaviour
{
    public bool isCollision;
    private GameObject player;

    private GameObject target;
    public GameObject Target { get => target; set => target = value; }

    public Material materialTarget;

    private void Start()
    {
        player = GameObject.Find("Player");
    }

    private void Update()
    {
        // sigue las coordenadas del player
        transform.position = player.transform.position;
        transform.rotation = player.transform.rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            isCollision = true;
            Target = other.gameObject;

            // cambia de color cuando el player esta cerca
            Target.GetComponent<Renderer>().material.color = Color.red;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
        if(isCollision)
        {
            isCollision = false;
            // retorna el color del enemy original
            Target.GetComponent<Renderer>().material = materialTarget;
        }
        
    }
}
