//using UnityEngine;
//using UnityEditor;
//using Sirenix.OdinInspector.Editor;
//using System.Collections.Generic;
//using EscapeGame.Core.Manager;
//using Spine;
//using Animancer;
//using Sirenix.OdinInspector;
//using System;
//using FairyGUI;
//using NUnit;
//using UnityEngine.InputSystem.iOS;


//public class ProcedureNodeTool : OdinEditorWindow
//{
//    [MenuItem("Tools/流程状态图", false, 24)]
//    private static void ShowWindow()
//    {
//        var window = GetWindow<ProcedureNodeTool>();
//        window.titleContent = new GUIContent("流程状态图【运行时时点击重绘】");
//        window.Show();
//    }

//    #region private param
//    private bool m_IsInit = false;
//    /// <summary>
//    ///流程拥有的所有节点  key：节点名字 
//    /// </summary>
//    private Dictionary<string, FlowNode> m_FlowData = new Dictionary<string, FlowNode>();
//    /// <summary>
//    /// 所有已经绘制的节点
//    /// </summary>
//    private HashSet<string> m_AllDrawsNode = new HashSet<string>();

//    /// <summary>
//    /// 所有的节点名
//    /// </summary>
//    private HashSet<string> allNodesName = new HashSet<string>();

//    //绘制x轴位置
//    private float m_XMin = 100;
//    /// <summary>
//    /// 绘制y轴位置
//    /// </summary>
//    private float m_YMin = 0;
//    /// <summary>
//    /// 矩形宽度
//    /// </summary>
//    private float m_Width = 400;
//    /// <summary>
//    /// 矩形高度
//    /// </summary>
//    private float m_Height = 10;
//    /// <summary>
//    /// 矩形间距
//    /// </summary>
//    private float m_Space = 100;
//    /// <summary>
//    ///文字样式
//    /// </summary>
//    private GUIStyle NameStyle;

//    /// <summary>
//    ///箭头大小
//    /// </summary>
//    private float m_ArrowHeadSize = 5;
//    /// <summary>
//    ///选中框颜色
//    /// </summary>
//    private Color m_Color_NoSelected ;
//    /// <summary>
//    ///非选中框颜色
//    /// </summary>
//    private Color m_Color_Selected;

//    private string m_LastStateName;
//    #endregion
//    [Button("重绘。黄色表示当前进程", ButtonSizes.Large, Style = ButtonStyle.Box),]
//    public void ReDraw()
//    {
//        m_IsInit = false;
//    }

//    [LabelText("矩形框间距缩放，改变此值后请点击重绘")]
//    [ProgressBar(0.3, 1)]
//    public float scale = 1f;

//    [ProgressBar(10,80)]
//    [LabelText("箭头间距，改变此值后请点击重绘")]
//    public float m_ArrowSpace = 10;

//    private void ResetParam()
//    {
//        m_XMin = 100;
//        m_YMin = 0;
//        m_Width = 400;
//        m_Height = 20;
//        m_Space = 50;
//        m_ArrowSpace = 10;
//        m_ArrowHeadSize = 6;
//        m_Color_NoSelected = new Color(1, 1, 1, 0.3f);
//        m_Color_Selected = new Color(1, 0.92f, 0, 0.3f);
//        m_LastStateName = ProcedureBoot.GetNowTransitionName();
//        NameStyle = new GUIStyle()
//        {
//            normal = new GUIStyleState() { textColor = Color.black },
//            fontSize = 14
//        };
//    }
//    Vector2 scrollPosition = Vector2.zero;

//    private void OnInspectorUpdate()
//    {
//        if(ProcedureBoot.GetNowTransitionName() != m_LastStateName)
//        {
//            m_LastStateName = ProcedureBoot.GetNowTransitionName();
//            Repaint();
//        }
//    }
//    protected override void OnImGUI()
//    {
//        if (!Application.isPlaying)
//        {
//            return;
//        }
//        base.OnImGUI(); // 确保Odin属性正常渲染

//        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
//        if (!m_IsInit)
//        {
//            ResetParam();
//            InitData();
//        }

//        m_AllDrawsNode.Clear();
//        int i = 0;
//        foreach (var fromNode in m_FlowData.Values)
//        {
//            DrawRect(fromNode);
//            //绘制连线
//            foreach (var targetNodeName in fromNode.ConnectedNodes)
//            {
//                var targetNode = GetNode(targetNodeName);
//                if (targetNode != null)
//                {
//                    DrawRect(targetNode);

//                    var fromPos = fromNode.Position.center;
//                    fromPos.x += i * m_ArrowSpace - m_Width *0.4f;
//                    var targetPos = targetNode.Position.center;
//                    targetPos.x += i * m_ArrowSpace - m_Width * 0.4f;
//                    DrawArrow(fromPos, targetPos, m_ArrowHeadSize);
//                }
//                i++;
//            }
//        }

//        EditorGUILayout.EndScrollView();
//    }

//    private void DrawRect(FlowNode fromNode)
//    {
//        //绘制完成的不再做绘制
//        if(m_AllDrawsNode.Contains(fromNode.NodeName))
//        {
//            return;
//        }
//        //显示矩形框
//        Handles.DrawSolidRectangleWithOutline(fromNode.Position, GetNodeColor(fromNode.NodeName), m_Color_NoSelected);
//        //显示文字
//        Handles.Label(fromNode.Position.position, fromNode.NodeName, NameStyle);

//        m_AllDrawsNode.Add(fromNode.NodeName);
//    }

//    private void DoReset()
//    {
//        foreach (var data in m_FlowData)
//        {
//            data.Value.ConnectedNodes.Clear();
//            ObjectPool<FlowNode>.Release(data.Value);
//        }
//        m_FlowData.Clear();
//        m_AllDrawsNode.Clear();
//        allNodesName.Clear();
//    }
//    private void AddNodeName(string _name)
//    {
//        if (!allNodesName.Contains(_name))
//        {
//            allNodesName.Add(_name);
//        }
//    }
//    private void InitData()
//    {
//        m_IsInit = true;
//        DoReset();
//        var map = ProcedureBoot.GetAllTransition();
//        foreach (var i in map)
//        {
//            AddNodeName(i.Key);
//            foreach (var j in i.Value)
//            {
//                AddNodeName(j);
//            }
//        }
        
//        int index = 0;
//        foreach (var name in allNodesName)
//        {
//            FlowNode data = ObjectPool<FlowNode>.Acquire();
//            data.index = index;
//            data.NodeName = name;
//            data.Position = new Rect(m_XMin, m_YMin + m_Space * index * scale, m_Width, m_Height);
//            bool flag = map.TryGetValue(name, out var item);
//            if(flag)
//            {
//                data.ConnectedNodes.AddRange(item);
//            }
//            m_FlowData.Add(name, data);
//            index++;
//        }
//    }

//    /// <summary>
//    /// 绘制箭头
//    /// </summary>
//   private void DrawArrow(Vector3 start, Vector3 end, float headSize = 0.5f)
//    {
//        // 绘制主体线段
//        Handles.DrawLine(start, end);

//        // 计算箭头方向
//        Vector3 direction = (end - start).normalized;
//        Vector3 right = Quaternion.LookRotation(direction) * Vector3.right * headSize;
//        Vector3 left = Quaternion.LookRotation(direction) * Vector3.left * headSize;

//        // 绘制箭头头部三角形
//        Handles.DrawLine(end, end - direction * headSize + right);
//        Handles.DrawLine(end, end - direction * headSize + left);
//        Handles.DrawLine(end - direction * headSize + right, end - direction * headSize + left);
//    }
//    private FlowNode GetNode(string nodeId)
//    {
//        bool flag = m_FlowData.TryGetValue(nodeId, out var node);
//        return flag ? node : null;
//    }
//    private Color GetNodeColor(string nodeId)
//    {
//        return ProcedureBoot.GetNowTransitionName() == nodeId ? m_Color_Selected : m_Color_NoSelected;
//    }

//    [System.Serializable]
//    public class FlowNode
//    {
//        public int index;
//        public string NodeName;
//        public Rect Position; // 节点位置
//        public List<string> ConnectedNodes = new List<string>(); // 连接的其他节点NodeName
//    }
//}