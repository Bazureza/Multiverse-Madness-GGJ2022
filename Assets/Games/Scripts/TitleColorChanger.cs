using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TitleColorChanger : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private Color whiteSideColor;
    [SerializeField] private Color blackSideColor;

    public void UpdateToWhiteSide()
    {
        titleText.color = whiteSideColor;
    }

    public void UpdateToBlackSide()
    {
        titleText.color = blackSideColor;
    }
}
