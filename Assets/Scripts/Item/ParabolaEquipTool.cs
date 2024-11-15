using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class ParabolaEquipTool : Equip
{
    public float attackRate;
    private bool attacking;
    public float attackDistance;
    public float useStamina;
    public float useHealth;

    public int damage;
    public GameObject ThrowThings;

    private Animator animator;
    private Camera camera;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        camera = Camera.main;
    }

    public override void OnAttackInput()
    {
        if (!attacking)
        {
            if (CharacterManager.Instance.Player.condition.UseStamina(useStamina)
                && CharacterManager.Instance.Player.condition.UseHealth(useHealth))
            {
                attacking = true;
                animator.SetTrigger("Attack");
                Invoke("OnCanAttack", attackRate);
            }
        }
    }

    void OnCanAttack()
    {
        attacking = false;
    }

    public override void OnHit()
    {
        GameObject inst = Instantiate(ThrowThings, transform.position, transform.rotation);
    }
}

