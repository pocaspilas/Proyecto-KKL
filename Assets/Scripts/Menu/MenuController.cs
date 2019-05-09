using Mati36.Utility;
using Mati36.VR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    void Start()
    {
        FadeUtility.FadeFromBlack(2);

        //ShowPanel("Fade es null : " + (FadeUtility.FadeObj == null));
    }
}
