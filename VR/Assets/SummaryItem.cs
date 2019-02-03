using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummaryItem : MonoBehaviour
{
    public RectTransform Bar;
    public SummaryCount Count;
    public string FeatureName;
    public float maxwidth=10f;
    public int tagcount=0;
    public int maxcount=0;

    private GameStateHandle gameState;
    //TextAlignment 
    // Start is called before the first frame update
    void Start()
    {
        gameState = GameObject.FindObjectOfType<GameStateHandle>();
    }

    // Update is called once per frame
    void Update()
    {
        tagcount = gameState.Instance.getTagCount(FeatureName);
        int max = maxcount > 1 ? maxcount : 1;
        float widthPerCount = maxwidth / max;
        Bar.sizeDelta = new Vector2(tagcount * widthPerCount, Bar.sizeDelta.y);
        Count.Text.text = string.Format("{0}", tagcount);
        Count.RectTransform.localPosition = new Vector3(tagcount * widthPerCount, 0, 0);
    }
}
