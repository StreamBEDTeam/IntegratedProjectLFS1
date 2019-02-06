using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummaryBoard : MonoBehaviour
{
    SummaryItem[] items;
    public float maxWidth=10f;
    // Start is called before the first frame update
    void Start()
    {
        items = GetComponentsInChildren<SummaryItem>();
    }

    // Update is called once per frame
    void Update()
    {
        int max = 0;
        foreach(var item in items)
        {
            if(item.tagcount > max)
            {
                max = item.tagcount;
            }
        }
        foreach(var item in items)
        {
            item.maxcount = max;
            item.maxwidth = maxWidth;
        }
    }
}
