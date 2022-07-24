using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPartHighlight : MonoBehaviour
{

    private Outline _outline;

    void Start()
    {
        _outline = GetComponent<Outline>();
        StartCoroutine(HighlightSequence());
    }

    public void HighlightBodyPart(bool canHighlight) {
        _outline.enabled = canHighlight;
    }

    // Demo purposes only
    IEnumerator HighlightSequence() {
        bool _canHighlight = false;
        while(true) {
            HighlightBodyPart(_canHighlight);
            _canHighlight = !_canHighlight;
            yield return new WaitForSecondsRealtime(1f);
        }
    }
}
