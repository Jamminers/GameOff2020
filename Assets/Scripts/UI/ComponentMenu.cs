using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ComponentMenu : MonoBehaviour
{
    static List<ComponentMenu> ActiveMenus = new List<ComponentMenu>();

    [Serializable]
    public class SelectorConfiguration
    {
        public string Name;
        public GameObject[] Options;
    }

    [SerializeField]
    EventSystem m_eventSystem;
    [SerializeField]
    Transform m_menuParent;
    [SerializeField]
    GameObject m_selectorPrefab;
    [SerializeField]
    UnityEvent m_onClose;

    [SerializeField]
    SelectorConfiguration[] m_configurations;

    [HideInInspector]
    public ComponentMenuSelector CurrentSelector;
    ComponentMenuSelector[] m_selectors;

    Ship m_ship;
    GameObject[] m_componentsSelection;

    bool m_completed;

    private void Awake()
    {
        InstanciateSelector();
        m_ship = GetComponent<Ship>();

        ActiveMenus.Add(this);
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

        Transform submitButton = m_menuParent.GetChild(0);
        submitButton.SetAsLastSibling();

        foreach (Transform child in m_menuParent)
        {
            int index = child.GetSiblingIndex();
            var selectable = child.GetComponent<Selectable>();
            var navigation = new Navigation() { mode = Navigation.Mode.Explicit };

            if (index != 0)
            {
                navigation.selectOnUp = m_menuParent.GetChild(index - 1).GetComponent<Selectable>();
            }

            if (index != m_menuParent.childCount - 1)
            {
                navigation.selectOnDown = m_menuParent.GetChild(index + 1).GetComponent<Selectable>();
            }

            selectable.navigation = navigation;
        }

        m_eventSystem.SetSelectedGameObject(m_selectors[0].gameObject);
    }

    public void OnNavigate(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        if (input.x != 0)
            CurrentSelector?.Select(input.x > 0 ? 1 : -1);
    }

    public void Validate()
    {
        m_componentsSelection = RetrieveComponents();
        m_menuParent.gameObject.SetActive(false);
        m_completed = true;

        foreach (var m in ActiveMenus)
        {
            if (!m.m_completed)
            {
                return;
            }
        }

        foreach (var m in ActiveMenus)
        {
            m.m_ship.BuildFromComponents(m_componentsSelection);
            m.Close();
        }

        GameManager.Instance.SetState(GameState.GameMain);
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
        m_onClose.Invoke();
        CurrentSelector = null;
    }
}
