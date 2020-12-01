using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Selectable))]
public class ComponentMenuSelector : MonoBehaviour
{
    [SerializeField]
    Text m_textName;

    ComponentMenu m_menu;

    int m_selected = 0;
    GameObject[] m_options;

    public GameObject Selected
    {
        get
        {
            return m_options[m_selected];
        }
    }

    public void Initialize(ComponentMenu menu, ComponentMenu.SelectorConfiguration config)
    {
        m_menu = menu;
        m_options = config.Options;
        Select(0);
    }

    public void SetActive(bool active)
    {
        if (active)
            m_menu.CurrentSelector = this;
    }

    public void Select(int direction)
    {
        m_selected = (m_selected + direction) % m_options.Length;
        if (m_selected < 0)
        {
            m_selected += m_options.Length;
        }
        m_textName.text = Selected.name;
    }
}
