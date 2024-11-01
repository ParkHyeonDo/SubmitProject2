using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField]private int damageRate;
    [SerializeField]private int damage;

    List<IDamagable>hitedObject = new List<IDamagable>();


    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Hit", 0, damageRate);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Hit() 
    {
        for (int i = 0; i < hitedObject.Count; i++)
        {
            hitedObject[i].TakePhysicalDamage(damage);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDamagable damagable))
        {
            hitedObject.Add(damagable);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out IDamagable damagable))
        {
            hitedObject.Remove(damagable);
        }
    }
}
