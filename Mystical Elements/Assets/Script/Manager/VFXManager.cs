using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ElementTree;
using TMPro;

public class VFXManager : MonoBehaviour
{
    #region Serialized Variables
    [Header("Floating Text")]
    [SerializeField] private GameObject m_floatingText;
    [Space(10)]

    [Header("Hit VFX")]
    [SerializeField] private GameObject m_basicHit;
    [SerializeField] private GameObject m_plasmaHit;
    [SerializeField] private GameObject m_frostHit;
    [SerializeField] private GameObject m_chargeHit;
    [SerializeField] private GameObject m_floraHit;
    [Space(10)]

    [Header("Element Image")]
    [SerializeField] private GameObject m_plasmaImage;
    [SerializeField] private GameObject m_frostImage;
    [SerializeField] private GameObject m_chargeImage;
    [SerializeField] private GameObject m_floraImage;
    #endregion

    public void SummonHitEffect(Vector3 pos, Element element)
    {
        GameObject vfx;

        switch (element)
        {
            case Element.DEFAULT:
                vfx = m_basicHit;
                break;
            case Element.PLASMA:
                vfx = m_plasmaHit;
                break;
            case Element.FROST:
                vfx = m_frostHit;
                break;
            case Element.CHARGE:
                vfx = m_chargeHit;
                break;
            case Element.FLORA:
                vfx = m_floraHit;
                break;
            default:
                vfx = m_basicHit;
                break;
        }

        vfx = GameObject.Instantiate(vfx, pos, transform.rotation);
        Destroy(vfx, 1f);
    }

    public void SummonFloatingText(Vector3 pos, string txt, Color colour)
    {
        GameObject text = GameObject.Instantiate(m_floatingText, new Vector3(pos.x, pos.y, pos.z), transform.rotation);
        text.GetComponentInChildren<TextMeshProUGUI>().text = txt;
        text.GetComponentInChildren<TextMeshProUGUI>().color = colour; //Will be changed depending on element
        Destroy(text, 1f);
    }

    public Color GetElementColour(Element element)
    {
        switch (element)
        {
            case Element.DEFAULT:
                return Color.white;
            case Element.PLASMA:
                return Color.red; //TO DO: Have custom colours made for them
            case Element.FROST:
                return Color.blue;
            case Element.CHARGE:
                return Color.yellow;
            case Element.FLORA:
                return Color.green;
            default:
                return Color.white;
        }
    }

    public void LayerElementBillboard(Element layerElement, Transform transform)
    {
        GameObject billboard;

        switch (layerElement)
        {
            case Element.PLASMA:
                billboard = m_plasmaImage;
                break;
            case Element.FROST:
                billboard = m_frostImage;
                break;
            case Element.CHARGE:
                billboard = m_chargeImage;
                break;
            case Element.FLORA:
                billboard = m_floraImage;
                break;
            default:
                Debug.LogWarning("Billboard call recieved a null / default entry for layer element");
                return;
        }

        billboard = GameObject.Instantiate(billboard, transform);
        billboard.transform.parent = transform;
    }
}
