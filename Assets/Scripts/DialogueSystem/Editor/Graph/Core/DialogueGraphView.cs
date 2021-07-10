﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UIElements;

namespace RPG.DialogueSystem.Graph
{
    public class DialogueGraphView : GraphView
    {
        private readonly DialogueGraphEditorWindow _editorWindow;        // 所在编辑器窗口
        public DialogueGraphView(DialogueGraphEditorWindow editorWindow)
        {
            _editorWindow = editorWindow;
            
            // 添加对话编辑器Style
            name = DialogueGraphUSSName.DIALOGUE_GRAPH;
            StyleSheet styleSheet =
                AssetDatabase.LoadAssetAtPath<StyleSheet>(DialogueGraphAssetsPath.DialogueGraphWindowViewSheet);
            styleSheets.Add(styleSheet);
            
            // 设置节点拖拽
            var dragger = new SelectionDragger()
            {
                // 不允许拖出边缘
                clampToParentEdges = true
            };
            // 其他按键触发节点拖拽
            dragger.activators.Add(new ManipulatorActivationFilter()
            {
                button = MouseButton.RightMouse,
                clickCount = 1,
                modifiers = EventModifiers.Alt
            });
            // 添加节点拖拽
            this.AddManipulator(dragger);
            
            // 设置界面缩放
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale * 5);
            
            // 设置创建节点回调
            nodeCreationRequest += (info) =>
            {
                AddElement(new DialogueGraphEndNode(new Vector2(0, 0), this));
            };
            
            // 添加界面移动
            this.AddManipulator(new ContentDragger());
            // 添加举行选择框
            this.AddManipulator(new RectangleSelector());
            
            // 创建背景
            Insert(0, new GridBackground());
            DialogueSearchWindowProvider provider = ScriptableObject.CreateInstance<DialogueSearchWindowProvider>();
            provider.OnSelectEntryCallback = CreateNode;
            nodeCreationRequest = (info) =>
            {
                SearchWindow.Open(new SearchWindowContext(info.screenMousePosition), provider);
            };
        }
        
        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            return ports.Where(port =>
            {
                DialogueGraphBaseNode targetNode = port.node as DialogueGraphBaseNode;
                DialogueGraphBaseNode startNode = startPort.node as DialogueGraphBaseNode;
                bool isTypaMatch = targetNode.CanConnectNode(startNode) && startNode.CanConnectNode(targetNode);
                // 连接的节点不是自身 && 节点的方向需要不同(进与出)
                return startPort.node != port.node && startPort.direction != port.direction && isTypaMatch;
            }).ToList();
        }

        /// <summary>
        /// SearchTree中单击节点选项回调
        /// </summary>
        /// <param name="SearchTreeEntry">点击节点信息</param>
        /// <param name="context">当前上下文</param>
        /// <returns>是否已处理点击事件</returns>
        private bool CreateNode(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            if (!(SearchTreeEntry.userData is Type nodeType)) return false;
            Vector2 nodePosition = contentViewContainer.WorldToLocal(context.screenMousePosition - _editorWindow.position.position);
            AddElement(Activator.CreateInstance(nodeType, nodePosition, this, null) as Node);
            return true;
        }
    }
}