using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class UIOption : MonoBehaviour //## 꾸준실습
{
    PlayerController controller;
    public GameObject OptionWindow;


    private void Start()
    {
        controller = CharacterManager.Instance.Player.controller;
        controller.option = Toggle;
        OptionWindow.SetActive(false);
    }

    private void Toggle()
    {
        if (!OptionWindow.activeInHierarchy)
        {
            OptionWindow.SetActive(true);
        }
        else if (OptionWindow.activeInHierarchy) 
        {
            OptionWindow.SetActive(false);
        }
    }
}

