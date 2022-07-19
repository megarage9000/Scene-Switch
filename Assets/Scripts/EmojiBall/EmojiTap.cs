using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmojiTap : MonoBehaviour
{
    public List<GameObject> subWords;

    // Start is called before the first frame update
    void Start() {
        SpawnSubWords();
    }

    private void SpawnSubWords() {
        int num = subWords.Count;
        float radius = 0.25f;
        Vector3 center = transform.position;

        for(int i = 0; i < num; i++) {

            /* Distance around the circle */
            var radians = 2 * Mathf.PI / num * i;

            /* Get the vector direction */
            var vertical = Mathf.Sin(radians);
            var horizontal = Mathf.Cos(radians);

            var spawnDir = new Vector3(0, horizontal, vertical);
             

            /* Get the spawn position */
            var spawnPos = center + spawnDir * radius; // Radius is just the distance away from the point

            /* Now spawn */
            var subword = Instantiate(subWords[i], spawnPos, Quaternion.identity) as GameObject;

            /* Rotate the enemy to face towards player */
            subword.transform.rotation = Quaternion.LookRotation(Vector3.forward);

            /* Adjust height */
            subword.transform.Translate(new Vector3(0, subword.transform.localScale.y / 2, 0));
        }
    }
}
