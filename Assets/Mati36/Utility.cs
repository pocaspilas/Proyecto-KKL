﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class Utility 
{
    static private Helper helper;

    static private Helper Helper
    {
        get
        {
            if (helper == null)
            {
                helper = new GameObject("Helper", typeof(Helper)).GetComponent<Helper>();
                helper.gameObject.hideFlags = HideFlags.HideAndDontSave;
            }
            return helper;
        }
    }

    static public void ExecuteAfterSeconds(float seconds, Action action)
    {
        Helper.Execute(action, seconds);
    }

}
