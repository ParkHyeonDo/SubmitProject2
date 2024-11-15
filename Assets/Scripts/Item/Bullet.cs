using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class Bullet : MonoBehaviour
{

    [SerializeField]private float speed;
    [SerializeField] private int damage;
    private Rigidbody rb;
    


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = CharacterManager.Instance.Player.ShootPosition.transform.forward * speed;
        Destroy(gameObject, 5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy")) return;
        else if (other.CompareTag("Enemy"))
        {
            NPC npc = other.GetComponent<NPC>();
            npc.TakePhysicalDamage(damage);
            Destroy(this.gameObject);
        }
    }
}

