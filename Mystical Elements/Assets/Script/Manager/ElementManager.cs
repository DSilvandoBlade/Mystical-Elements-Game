using System.Collections;
using System.Collections.Generic;
using ElementTree;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ElementManager : MonoBehaviour
{
    #region Private Variables
    private Player m_player;
    private PlayerInput m_playerInput;
    #endregion

    #region Serialized Variables
    [Header("UI References")]
    [SerializeField] private Image m_symbolImage;
    [SerializeField] private Image m_auraImage;
    [SerializeField] private Image m_energyRadialBar;
    [SerializeField] private float m_auraAlphaValue;
    [Space(10)]

    [Header("Sprites")]
    [SerializeField] private Sprite m_plasmaSymbolSprite;
    [SerializeField] private Sprite m_frostSymbolSprite;
    [SerializeField] private Sprite m_chargeSymbolSprite;
    [SerializeField] private Sprite m_floraSymbolSprite;
    [Space(10)]

    [Header("Colours")]
    [SerializeField] private Color m_plasmaColour;
    [SerializeField] private Color m_frostColour;
    [SerializeField] private Color m_chargeColour;
    [SerializeField] private Color m_floraColour;
    [Space(10)]

    [Header("Robe Materials")]
    [SerializeField] private Material m_plasmaMaterial;
    [SerializeField] private Material m_frostMaterial;
    [SerializeField] private Material m_chargeMaterial;
    [SerializeField] private Material m_floraMaterial;
    [SerializeField] private Material m_whiteRobeMat;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        m_player = FindObjectOfType<Player>();
        m_playerInput = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_playerInput.actions["Plasma"].triggered)
        {
            m_player.SwitchElement(Element.PLASMA);
            m_symbolImage.sprite = m_plasmaSymbolSprite;
            m_auraImage.color = new Color(m_plasmaColour.r, m_plasmaColour.g, m_plasmaColour.b, m_auraAlphaValue);
            m_energyRadialBar.color = m_plasmaColour;

            Material[] mat = new Material[2];
            mat[0] = m_plasmaMaterial;
            mat[1] = m_whiteRobeMat;
            m_player.ChangeRobeColour(mat);
        }

        else if (m_playerInput.actions["Frost"].triggered)
        {
            m_player.SwitchElement(Element.FROST);
            m_symbolImage.sprite = m_frostSymbolSprite;
            m_auraImage.color = new Color(m_frostColour.r, m_frostColour.g, m_frostColour.b, m_auraAlphaValue);
            m_energyRadialBar.color = m_frostColour;

            Material[] mat = new Material[2];
            mat[0] = m_frostMaterial;
            mat[1] = m_whiteRobeMat;
            m_player.ChangeRobeColour(mat);
        }

        else if (m_playerInput.actions["Charge"].triggered)
        {
            m_player.SwitchElement(Element.CHARGE);
            m_symbolImage.sprite = m_chargeSymbolSprite;
            m_auraImage.color = new Color(m_chargeColour.r, m_chargeColour.g, m_chargeColour.b, m_auraAlphaValue);
            m_energyRadialBar.color = m_chargeColour;

            Material[] mat = new Material[2];
            mat[0] = m_chargeMaterial;
            mat[1] = m_whiteRobeMat;
            m_player.ChangeRobeColour(mat);
        }

        else if (m_playerInput.actions["Flora"].triggered)
        {
            m_player.SwitchElement(Element.FLORA);
            m_symbolImage.sprite = m_floraSymbolSprite;
            m_auraImage.color = new Color(m_floraColour.r, m_floraColour.g, m_floraColour.b, m_auraAlphaValue);
            m_energyRadialBar.color = m_floraColour;

            Material[] mat = new Material[2];
            mat[0] = m_floraMaterial;
            mat[1] = m_whiteRobeMat;
            m_player.ChangeRobeColour(mat);
        }
    }
}
