using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ComponentMenu : MonoBehaviour
{
    [Serializable]
    public class SelectorConfiguration
    {
        public string Name;
        public GameObject[] Options;
    }

    [SerializeField]
    Canvas m_canvas;
    [SerializeField]
    Transform m_menuParent;
    [SerializeField]
    GameObject m_selectorPrefab;

    [SerializeField]
    SelectorConfiguration[] m_configurations;

    [HideInInspector]
    public ComponentMenuSelector CurrentSelector;
    ComponentMenuSelector[] m_selectors;

    Ship m_ship;

    private void Awake()
    {
        InstanciateSelector();
        m_ship = GetComponent<Ship>();
    }

    void InstanciateSelector()
    {
        m_selectors = new ComponentMenuSelector[m_configurations.Length];
        for (int i = 0; i < m_configurations.Length; i++)
        {
            m_selectors[i] = Instantiate(m_selectorPrefab, m_menuParent).GetComponent<ComponentMenuSelector>();
            m_selectors[i].name = $"Selector {m_configurations[i].Name}";
            m_selectors[i].Initialize(this, m_configurations[i]);
        }
    }

    public void OnNavigate(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        if (input.x != 0)
            CurrentSelector?.Select(input.x > 0 ? 1 : -1);
    }

    public void Validate()
    {
        m_ship.BuildFromComponents(RetrieveComponents());
        Close();
    }

    GameObject[] RetrieveComponents()
    {
        var result = new GameObject[m_selectors.Length];
        for (int i = 0; i < m_selectors.Length; i++)
        {
            result[i] = m_selectors[i].Selected;
        }
        return result;
    }


    void Close()
    {
        m_canvas.enabled = false;
        CurrentSelector = null;
    }
}
