using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TextFormatter : MonoBehaviour
{
    Text m_text;
    void Awake()
    {
        m_text = GetComponent<Text>();
    }

    public void Format(int value)
    {
        m_text.text = string.Format(m_text.text, value);
    }
}
