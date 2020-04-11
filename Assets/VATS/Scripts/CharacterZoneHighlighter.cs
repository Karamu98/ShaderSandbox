#define Kara_Debug

using System;
using UnityEngine;


public class CharacterZoneHighlighter : MonoBehaviour
{
    [SerializeField] MeshRenderer m_meshRenderer              = default;
    [SerializeField] Color m_highlightColour                  = default;
    [Header("Zone Values")]
    [SerializeField][Range(0, 1)] float m_headTexValue      = default;
    [SerializeField] [Range(0, 1)] float m_leftArmTexValue  = default;
    [SerializeField][Range(0, 1)] float m_rightArmTexValue  = default;
    [SerializeField][Range(0, 1)] float m_torsoTexValue     = default;
    [SerializeField][Range(0, 1)] float m_leftLegTexValue   = default;
    [SerializeField][Range(0, 1)] float m_rightLegTexValue  = default;
    [Header("Pulse settings")]
    [SerializeField] bool m_shouldPulse                       = default;
    [SerializeField] float m_pulseRangeMin                    = default;
    [SerializeField] float m_pulseRangeMax                    = default;
    [SerializeField] float m_pulseSpeed                       = default;

    CachingManager m_cachingManager                           = default;
    Material m_VATsMaterial                                   = default;

    const float m_precisionAllowence = 0.03f;
    BodyZone m_selectedZone = BodyZone.None;


    public void Init()
    {
        m_VATsMaterial = m_meshRenderer.material;

        m_cachingManager = new CachingManager();
        m_cachingManager.Init(typeof(ShaderValues), Shader.PropertyToID);

        m_VATsMaterial.SetColor(m_cachingManager[ShaderValues._HighlightColor], m_highlightColour);
    }

    public void Process()
    {
        if (m_shouldPulse)
        {
            float brightness = Mathf.Lerp(m_pulseRangeMin, m_pulseRangeMax, ((Mathf.Sin(Time.timeSinceLevelLoad * m_pulseSpeed) + 1) * 0.5f));
            m_VATsMaterial.SetFloat(m_cachingManager[ShaderValues._HighlightBrightness], brightness);
        }
    }

    public void HighlightZone(BodyZone bodyZone)
    {
        m_selectedZone = bodyZone;

        switch (bodyZone)
        {
            case BodyZone.Head:
                SetSelectedZone(m_headTexValue);
                break;
            case BodyZone.LeftArm:
                SetSelectedZone(m_leftArmTexValue);
                break;
            case BodyZone.RightArm:
                SetSelectedZone(m_rightArmTexValue);
                break;
            case BodyZone.Torso:
                SetSelectedZone(m_torsoTexValue);
                break;
            case BodyZone.LeftLeg:
                SetSelectedZone(m_leftLegTexValue);
                break;
            case BodyZone.RightLeg:
                SetSelectedZone(m_rightLegTexValue);
                break;
            case BodyZone.None:
                m_VATsMaterial.SetFloat(m_cachingManager[ShaderValues._SelectedZoneMin], 0.0f);
                m_VATsMaterial.SetFloat(m_cachingManager[ShaderValues._SelectedZoneMax], 0.0f);
                break;
            default:
                break;
        }
            
    }

    public void TogglePulse(bool shouldPulse)
    {
        m_shouldPulse = shouldPulse;
        m_VATsMaterial.SetFloat(m_cachingManager[ShaderValues._HighlightBrightness], 1.0f);
    }


    private void SetSelectedZone(float zoneID)
    {
        m_VATsMaterial.SetFloat(m_cachingManager[ShaderValues._SelectedZoneMin], zoneID - m_precisionAllowence);
        m_VATsMaterial.SetFloat(m_cachingManager[ShaderValues._SelectedZoneMax], zoneID + m_precisionAllowence);
    }


    public enum BodyZone
    {
        Head = 1,
        LeftArm,
        RightArm,
        Torso,
        LeftLeg,
        RightLeg,
        None
    }

    private enum ShaderValues
    {
        _SelectedZoneMin,
        _SelectedZoneMax,
        _HighlightColor,
        _HighlightBrightness
    }




    private void Update()
    {
        Process();
    }

    private void Awake()
    {
        Init();
    }

#if Kara_Debug

    Array debug_EnumVals = (typeof(BodyZone)).GetEnumValues();

    private void OnGUI()
    {
        TogglePulse(GUILayout.Toggle(m_shouldPulse, "Pulse"));

        foreach (var enumVal in debug_EnumVals)
        {
            BodyZone cur = (BodyZone)enumVal;

            if (cur == m_selectedZone)
            {
                GUI.color = Color.green;
            }
            if (GUILayout.Button($"Select {enumVal.ToString()}"))
            {
                HighlightZone(cur);
            }
            GUI.color = Color.white;
        }
    }

#endif
}