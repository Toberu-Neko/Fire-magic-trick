using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;
using System;
using System.Collections.Generic;
using System.Linq;
public class Test :MonoBehaviour
{
    private List<string> options;
    private Resolution[] resolutions;

    private void Start()
    {
        resolutions = Screen.resolutions;

        options = new List<string>();

        foreach (Resolution res in resolutions)
        {
            if (!options.Contains(res.width + "x" + res.height))
            {
                options.Add(res.width + "x" + res.height);
            }
        }
    }
}