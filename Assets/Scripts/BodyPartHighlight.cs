using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPartHighlight : MonoBehaviour
{

    private Outline _outline;

    void Start()
    {
        _outline = GetComponent<Outline>();
        HighlightBodyPart(true);
    }

    public void HighlightBodyPart(bool canHighlight) {
        _outline.enabled = canHighlight;
    }
}
