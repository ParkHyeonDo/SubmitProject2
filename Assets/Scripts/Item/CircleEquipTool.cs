using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class CircleEquipTool : Equip
{
    public float attackRate;
    private bool attacking;
    public float attackDistance;
    public float useStamina;
    public float useHealth;

    [Header("Combat")]
    public bool doesDealDamage;
    public int damage;

    private Animator animator;
    private Camera camera;
    private NPC npc;


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

    public void OnHit()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, attackDistance);
        if (cols.Length == 0) return;
        else if (cols.Length > 0) 
        {
            for (int i = 0; i < cols.Length; i++) 
            {
                if (cols[i].CompareTag("Enemy")) 
                {
                    npc = cols[i].GetComponent<NPC>();
                    Debug.Log(damage + " " + npc.name);
                    npc.TakePhysicalDamage(damage);
                }
            }
        }
    }

    
}

