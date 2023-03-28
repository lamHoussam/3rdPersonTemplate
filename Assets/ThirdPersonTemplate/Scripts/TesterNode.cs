using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeEditorFramework;

public class TesterNode : Node
{
    #region Members
    #endregion Members
	

#if UNITY_EDITOR
    public override bool Removable => base.Removable;
    public static TesterNode Create(Rect rect)
    {
        TesterNode node = CreateInstance<TesterNode>();

        node.m_Rect = rect;
        node.m_InitialRect = rect;

        node.Init();

        return node;
        
    }

    public override void Draw(float scale = 1)
    {
        
        base.Draw(scale);
        GUILayout.BeginArea(m_Rect, m_isEvaluationResult ? NodeEditor.Instance.m_EvaluatedNodeResult : NodeEditor.Instance.m_NodeBox);
    }
    
    public override void OnRemove()
    {
        
        base.OnRemove();
    }
#endif
}
