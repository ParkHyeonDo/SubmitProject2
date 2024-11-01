using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICondition : MonoBehaviour
{
    public Condition health;
    public Condition hunger;
    public Condition stamina;
    public Condition mana;

    // Start is called before the first frame update
    void Start()
    {
        CharacterManager.Instance.Player.condition.uICondition = this; //캐릭터매니저.캐릭터매니저.플레이어.플레이어.UI컨디션
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
