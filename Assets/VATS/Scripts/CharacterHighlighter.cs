#define Kara_Debug

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHighlighter : MeshHighlighter
{
    readonly float[] m_zoneVals = { 1.0f, 0.9f, 0.8f, 0.7f, 0.6f, 0.5f };
    BodyZone m_selectedZone = BodyZone.None;

    public void HighlightBodyZone(BodyZone bodyZone)
    {
        m_selectedZone = bodyZone;

        HighlightZone(m_zoneVals[(int)bodyZone]);
    }

    public enum BodyZone
    {
        Head,
        LeftArm,
        RightArm,
        Torso,
        LeftLeg,
        RightLeg,
        None
    }

#if Kara_Debug
    Array debug_EnumVals = typeof(BodyZone).GetEnumValues();
    private void OnGUI()
    {
        TogglePulse(GUILayout.Toggle(IsPulse, "Pulse"));        

        foreach (var enumVal in debug_EnumVals)
        {
            BodyZone cur = (BodyZone)enumVal;

            if (cur == m_selectedZone)
            {
                GUI.color = Color.green;
            }
            if (GUILayout.Button($"Select {enumVal.ToString()}"))
            {
                HighlightBodyZone(cur);
            }
            GUI.color = Color.white;
        }
    }

#endif


    private void Start() => Init();
    private void Update() => Process();
}
