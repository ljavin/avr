﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;

[RequireComponent(typeof(Animator))]
public class MenuButton : MonoBehaviour {

    private const string STATE_HOVERED = "hovered";
    private const string STATE_CLICKED = "clicked";
    private const string STATE_IDLE = "idle";

    private bool isClicked = false;

    private Animator animator;
    public AudioSource clickSound;
    
	void Start () {
        animator = GetComponent<Animator>();
	}

    void OnTriggerEnter(Collider other) {
        if (other.tag == TagConstants.FINGER_TIP && !isClicked) {
            HandModel hand = other.transform.GetComponentInParent<HandModel>();
            Hand leapHand = hand.GetLeapHand();

            if (leapHand.IsRight) {
                animator.SetTrigger(STATE_CLICKED);
                animator.ResetTrigger(STATE_IDLE);

                isClicked = true;

                if (transform.name == "ButtonRestart" && TutorialManager.instance.tutorialCompleted) {
                    EventManager.TriggerEvent("ReloadGame");
                }
                else if (transform.name == "ButtonReset") {
                    EventManager.TriggerEvent("ResetTools");
                    EventManager.TriggerEvent("TutorialReset");
                }

                if (clickSound != null) {
                    clickSound.Play();
                }
            }
        }
    }

    void OnTriggerExit(Collider collider) {
        if (collider.tag == TagConstants.FINGER_TIP) {
            animator.SetTrigger(STATE_IDLE);
            animator.ResetTrigger(STATE_CLICKED);

            isClicked = false;
        }
    }
}
