﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using RPG.InventorySystem;
using RPG.Module;
using RPG.UI;
using RPG.SaveSystem;
using RPG.TradeSystem;
using UI;
using UnityEditor;

public class TestClass : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            
            // 显示背包界面
            if (InventoryController.controller.isActive)
            {
                InventoryController.controller.Hide();
            }
            else
            {
                InventoryController.controller.Show();
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            // 显示装备界面
            if (EquipmentController.controller.isActive)
            {
                EquipmentController.controller.Hide();
            }
            else
            {
                EquipmentController.controller.Show();
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PauseController.controller.isActive)
            {
                GlobalUIManager.Instance.CloseUI();
            }
            else
            {
                GlobalUIManager.Instance.OpenUI(PauseController.controller);
            }
        }
        
        
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (TradeController.controller.isActive)
            {
                TradeController.controller.Hide();
            }
            else
            {
                TradeController.controller.Show();
            }
        }
    }

}

