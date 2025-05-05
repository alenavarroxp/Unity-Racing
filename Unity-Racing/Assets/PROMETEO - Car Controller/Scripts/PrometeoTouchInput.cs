using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrometeoTouchInput : MonoBehaviour
{

    public bool changeScaleOnPressed = false;
    [HideInInspector]
    public bool buttonPressed = false;
    RectTransform rectTransform;
    Vector3 initialScale;
    float scaleDownMultiplier = 0.85f;

    bool isOutOfFuel = false;
    private AudioSource audioSource;

    void Start() {
      rectTransform = GetComponent<RectTransform>();
      initialScale = rectTransform.localScale;

      GameObject soundObject = GameObject.Find("No Starting Sound");
      if (soundObject != null) {
          audioSource = soundObject.GetComponent<AudioSource>();
      } else {
          Debug.LogWarning("No Starting Sound GameObject not found.");
      }
    }

    public void SetIsOutOfFuel(bool value) {
      isOutOfFuel = value;

      GameObject engineSoundObject = GameObject.Find("Car Engine Sound");
      if (engineSoundObject != null) {
          AudioSource engineAudio = engineSoundObject.GetComponent<AudioSource>();
          if (engineAudio != null) {
              Debug.Log("Engine audio found and set to " + (isOutOfFuel ? "stopped" : "playing"));
              if (isOutOfFuel && engineAudio.isPlaying) {
                Debug.Log("Stopping engine audio due to out of fuel.");
                engineAudio.Stop();
              } else if (!isOutOfFuel) {
                Debug.Log("Starting engine audio.");
                engineAudio.time = 0f;
                engineAudio.Play();
              }
          }
      } else {
          Debug.LogWarning("No se encontró el GameObject 'Car Engine Sound'");
      }
    }



    public void ButtonDown(){
      if(changeScaleOnPressed){
        rectTransform.localScale = initialScale * scaleDownMultiplier;
      }
      if (isOutOfFuel) {
        if (audioSource != null && !audioSource.isPlaying) {
            audioSource.Play();
        }
        return;
      }

      buttonPressed = true;
      
    }

    public void ButtonUp(){
      buttonPressed = false;
      if(changeScaleOnPressed){
        rectTransform.localScale = initialScale;
      }
    }

    public void ForceButtonUp() {
      buttonPressed = false;
      if (changeScaleOnPressed) {
          rectTransform.localScale = initialScale;
      }
    }

}
