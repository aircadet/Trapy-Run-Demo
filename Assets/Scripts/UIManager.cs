using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using  UnityEngine.UI;
using  TMPro;
public class UIManager : MonoBehaviour
{
   [SerializeField] private TextMeshProUGUI _levelText;
   [SerializeField] private GameObject _preparingScreen, _playingScreen, _finishScreen, _failScreen;
  
    private void Start()
   {
      EditLevelText();
   }

   private void Update()
   {
      EditUIVisibility();
   }

   void EditUIVisibility()
   {
      if (GameManager.instance.playerState == GameManager.PlayerState.Preparing)
      {
         _preparingScreen.SetActive(true);
      }
      else
      {
         _preparingScreen.SetActive(false);
      }

      if (GameManager.instance.playerState == GameManager.PlayerState.Playing)
      {
         _playingScreen.SetActive(true);
      }
      else
      {
         _playingScreen.SetActive(false);
      }

      if ( GameManager.instance.playerState == GameManager.PlayerState.Finish)
      {
         _finishScreen.SetActive(true);
      }
      else
      {
         _finishScreen.SetActive(false);
      }
      
      if ( GameManager.instance.playerState == GameManager.PlayerState.Fail)
      {
         _failScreen.SetActive(true);
      }
      else
      {
         _failScreen.SetActive(false);
      }
   }
   void EditLevelText()
   {
      _levelText.text ="Level " +  GameManager.instance._level.ToString();
   }
   
   
   // BUTTON ANIM CONTROL
   public void MouseButtonDown(Animator animator)
   {
      animator.SetTrigger("Down");
      
   }

   public void MouseButtonUp(Animator animator)
   {
      animator.SetTrigger("Up");
   }

   
}
