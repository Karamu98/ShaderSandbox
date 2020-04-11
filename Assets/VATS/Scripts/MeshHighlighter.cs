using System;
using UnityEngine;


public abstract class MeshHighlighter : MonoBehaviour
{
    [SerializeField] MeshRenderer m_meshRenderer              = default;
    [SerializeField] Color m_highlightColour                  = Color.green;
    [Header("Pulse settings")]
    [SerializeField] bool m_shouldPulse                       = true;
    [SerializeField] float m_pulseRangeMin                    = 0.7f;
    [SerializeField] float m_pulseRangeMax                    = 1.0f;
    [SerializeField] float m_pulseSpeed                       = 10f;

    CachingManager m_cachingManager;
    Material m_VATsMaterial;

    const float m_precisionAllowence = 0.03f;


    protected void Init()
    {
        m_VATsMaterial = m_meshRenderer.material;

        m_cachingManager = new CachingManager();
        m_cachingManager.Init(typeof(ShaderValues), Shader.PropertyToID);

        m_VATsMaterial.SetColor(m_cachingManager[ShaderValues._HighlightColor], m_highlightColour);
    }

    protected void Process()
    {
        if (m_shouldPulse)
        {
            float pulseBrightness = Mathf.Lerp(m_pulseRangeMin, m_pulseRangeMax, ((Mathf.Sin(Time.timeSinceLevelLoad * m_pulseSpeed) + 1) * 0.5f));
            m_VATsMaterial.SetFloat(m_cachingManager[ShaderValues._HighlightBrightness], pulseBrightness);
        }
    }

    protected void TogglePulse(bool shouldPulse)
    {
        m_shouldPulse = shouldPulse;
        m_VATsMaterial.SetFloat(m_cachingManager[ShaderValues._HighlightBrightness], 1.0f);
    }

    public bool IsPulse { get { return m_shouldPulse; } }

    protected void HighlightZone(float zoneID)
    {
        m_VATsMaterial.SetFloat(m_cachingManager[ShaderValues._SelectedZoneMin], zoneID - m_precisionAllowence);
        m_VATsMaterial.SetFloat(m_cachingManager[ShaderValues._SelectedZoneMax], zoneID + m_precisionAllowence);
    }

    private enum ShaderValues
    {
        _SelectedZoneMin,
        _SelectedZoneMax,
        _HighlightColor,
        _HighlightBrightness
    }
}