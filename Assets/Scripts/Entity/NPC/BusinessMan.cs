using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.QuestSystem;
using RPG.InventorySystem;
using RPG.DialogueSystem;
using RPG.Utility;
namespace RPG.Entity
{
    [RequireComponent(typeof(Animator), typeof(DialogueNPC), typeof(CapsuleCollider))]
    public class BusinessMan : BaseEntity
    {
        public bool IsCollidePlayer => collidePlayerCheck.IsCollide;
        [SerializeField] private LayerMask targetLayer;             // 对象层级
        private Animator animator;
        private DialogueNPC dialogueNPC;
        private OverlabSphereCheck collidePlayerCheck;
        private void Awake()
        {
            animator = GetComponent<Animator>();
            dialogueNPC = GetComponent<DialogueNPC>();
            collidePlayerCheck = GetComponent<OverlabSphereCheck>();
        }
        
        public void StartQuest(QuestSO quest)
        {
            PlayerQuestManager.Instance.AddQuest(quest);
        }

        public void SubmitQuest(QuestSO quest)
        {
            // TODO: 使任务UI显示奖励物品
            // TODO: 完成与NPC对话AddItem
            Debug.Log(string.Concat("完成了任务", quest.questTitle));
            // TODO: 创建物品生成类
            for (int i = 0; i < quest.questReward.itemObjAmount; i++)
            {
                ItemData newItem = new ItemData(quest.questReward.itemObj);
                newItem.itemBuffs.RenerateValues();
                if (!PlayerInventoryManager.Instance.inventoryObject.AddItem(newItem, 1))
                {
                    Debug.Log("背包已满");
                }
            }
            // 任务提交完成 移除任务
            PlayerQuestManager.Instance.RemoveQuest(quest);
        }
        public void SetTalkState()
        {
            animator.SetTrigger("Talk");
            dialogueNPC.StartDialogue();
        }
        public void SetIdleState()
        {
            dialogueNPC.ResetDialogue();
        }
    }

}