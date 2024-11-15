using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable 
{
    void TakePhysicalDamage(int damage);
}
public class PlayerCondition : MonoBehaviour , IDamagable
{
    public UICondition uICondition;

    Condition health { get { return uICondition.health; } }
    Condition hunger { get { return uICondition.hunger; } }
    Condition stamina { get { return uICondition.stamina; } }

    Condition mana { get { return uICondition.mana; } }

    // Start is called before the first frame update
    public float noHungerHealthDecay;

    public event Action onTakeDamage;

    // Update is called once per frame
    void Update()
    {
        hunger.Subtract(hunger.passiveValue * Time.deltaTime);
        stamina.Add(stamina.passiveValue * Time.deltaTime);
        mana.Add(mana.passiveValue * Time.deltaTime);

        if (hunger.curValue <= 0f) 
        {
            health.Subtract(noHungerHealthDecay * Time.deltaTime);
        }

        if (health.curValue <= 0f) 
        {
            Die();
        }
         
    }

    public void Heal(float amount) 
    {
        health.Add(amount);
    }

    public void Eat(float amount)
    {
        hunger.Add(amount);
    }

    public void manaGen(float amount) 
    {
        mana.Add(amount);
    }

    private void Die()
    {
        Debug.Log("Áê±Ý");
    }

    public void TakePhysicalDamage(int damage)
    {
        health.Subtract(damage);
        onTakeDamage?.Invoke();
    }

    public bool UseStamina(float amount) 
    {
        if (stamina.curValue - amount < 0f) 
        {
            return false;
        }

        stamina.Subtract(amount);
        return true;
    }

    public bool UseHealth(float amount)
    {
        if (health.curValue - amount < 0f)
        {
            return false;
        }

        health.Subtract(amount);
        return true;
    }

    public bool UseMana(float amount) 
    {
        if (mana.curValue - amount < 0f) 
        {
            return false;
        }

        mana.Subtract(amount);
        return true;
    }
}
