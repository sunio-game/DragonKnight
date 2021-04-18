﻿using System.Data.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 物品插槽
namespace RPG.InventorySystem
{
    [System.Serializable]
    public class InventorySlot
    {
        public BaseItemObject itemObject
        {
            get
            {
                return slotData.itemObject;
            }
        }
        public bool isEmpty
        {
            get
            {
                return slotData.isEmpty;
            }
        }
        [DisplayOnly] public int slotIndex;         // 插槽编号
        public AllowType allowType;                 // 允许防止的类型
        public InventorySlotData slotData;          // 插槽存储数据
        [System.NonSerialized] public Action<int> OnBeforeUpdate;   // 插槽更新前事件
        [System.NonSerialized] public Action<int> OnAfterUpdate;    // 插槽更新后事件
                                                                    // 添加数量
        public void AddAmount(int _amount)
        {
            OnBeforeUpdate?.Invoke(slotIndex);
            slotData.amount += _amount;
            OnAfterUpdate?.Invoke(slotIndex);
        }
        // 更新插槽值
        public void UpdateSlot(InventorySlotData _data)
        {
            OnBeforeUpdate?.Invoke(slotIndex);
            slotData.itemData = _data.itemData;
            slotData.amount = _data.amount;
            OnAfterUpdate?.Invoke(slotIndex);
        }
        // 清空插槽值
        public void ClearSlot()
        {
            UpdateSlot(new InventorySlotData());
        }
        public bool TypeMatch(BaseItemObject itemObj)
        {
            // itemObj为空 itemObj无记录物体
            if (itemObj == null || itemObj.item.id < 0)
            {
                return true;
            }
            return allowType.Match(itemObj.itemType);
        }
    }
}