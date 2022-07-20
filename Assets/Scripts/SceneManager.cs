using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public enum State : ushort
{
    Intro,
    Mannequin,
    Head,
    Face,
    Shoulders,
    Arms,
    Chest,
    Stomach,
    Body,
    Emojis,
    Eword,
    Resize,
    End,
}

public class SceneManager : MonoBehaviour
{
    public AudioClip[] audioClips;
    private AudioSource audioSource;

    private State prevState = 0;
    private State currState = 0;
    private bool clipIsPlaying = false;

    void Start(){
        audioSource = this.GetComponent<AudioSource>();
        
        Debug.Log("STATE: "  + currState); // STATE: Intro
        //Add delay to in front of audio clip
        audioSource.clip = audioClips[0];
        // audioSource.Play();
    }

    void Update(){
        clipIsPlaying = (audioSource.isPlaying) ? true : false;

        if(currState == State.Intro && !audioSource.isPlaying){
            currState = State.Mannequin;
            Debug.Log("STATE: "  + currState); // STATE: Mannequin
            prevState = currState;
            //turn mannequin on
            //Add delay to in front of audio clip
            audioSource.clip = audioClips[1];
            audioSource.Play();

            Thread thr = new Thread(MannequinStuff);
            thr.Start();
        }
        else if(prevState == State.Mannequin && currState == State.Head){
            Debug.Log("STATE: "  + currState); // STATE: Head
            prevState = currState;
        }
        else if(prevState == State.Head && currState == State.Face){
            Debug.Log("STATE: "  + currState); // STATE: Face
            prevState = currState;
        }
        else if(prevState == State.Face && currState == State.Shoulders){
            Debug.Log("STATE: "  + currState); // STATE: Shoulders
            prevState = currState;
        }
        else if(prevState == State.Shoulders && currState == State.Arms){
            Debug.Log("STATE: "  + currState); // STATE: Arms
            prevState = currState;
        }
        else if(prevState == State.Arms && currState == State.Chest){
            Debug.Log("STATE: "  + currState); // STATE: Chest
            prevState = currState;
        }
        else if(prevState == State.Chest && currState == State.Stomach){
            Debug.Log("STATE: "  + currState); // STATE: Stomach
            prevState = currState;
        }
        else if(prevState == State.Stomach && currState == State.Body){
            Debug.Log("STATE: "  + currState); // STATE: Body
            prevState = currState;
        }
        else if(prevState == State.Body && currState == State.Emojis){
            Debug.Log("STATE: "  + currState); // STATE: Emojis
            prevState = currState;
        }
        else if(prevState == State.Emojis && currState == State.Eword){
            Debug.Log("STATE: "  + currState); // STATE: Eword
            prevState = currState;
        }
        else if(prevState == State.Eword && currState == State.Resize){
            Debug.Log("STATE: "  + currState); // STATE: Resize
            prevState = currState;
        }
        else if(prevState == State.Resize && currState == State.End){
            Debug.Log("STATE: "  + currState); // STATE: End
            prevState = currState;
        }
    }

    void MannequinStuff(){ 
        while(clipIsPlaying){
            Debug.Log("Running");
        }

        //TODO: end of audio put purple subtitles
        currState = State.Head;
        // thr.Abort();
    }
}
