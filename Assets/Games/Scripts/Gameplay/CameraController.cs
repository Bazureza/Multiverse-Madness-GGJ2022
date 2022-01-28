using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera whiteSideCam;
    [SerializeField] private Camera blackSideCam;

    [SerializeField, ReadOnly] private SideType currentSide;

    public void ChangeWhiteSide()
    {
        ChangeSide(SideType.White);
    }

    public void ChangeBlackSide()
    {
        ChangeSide(SideType.Black);
    }

    public void ChangeSide(SideType sideType)
    {
        currentSide = sideType;
        ChangeCamSide();
    }

    private void ChangeCamSide()
    {
        switch (currentSide)
        {
            case SideType.White:
                whiteSideCam.enabled = true;
                blackSideCam.enabled = false;
                break;
            case SideType.Black:
                whiteSideCam.enabled = false;
                blackSideCam.enabled = true;
                break;
        }
    }

    public enum SideType { White, Black }
}
