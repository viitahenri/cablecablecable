using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorationLines : MonoBehaviour
{
    void Start()
    {
        var lineRenderers = GetComponentsInChildren<LineRenderer>();
        var queue = lineRenderers[0].material.renderQueue - 10;
        for (int i = 0; i < lineRenderers.Length; i++)
        {
            for (int j = 0; j < lineRenderers[i].materials.Length; j++)
            {
                lineRenderers[i].materials[j].renderQueue = queue + i;
                lineRenderers[i].materials[j].color = Random.ColorHSV(0f, 1f, .8f, .8f, .8f, .8f);
            }
        }
    }
}
