using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace dchalrefactor.Scripts.Animations.PlayerMovement
{
    public class PlayerAnimations : MonoBehaviour
    {
        //Stores and handles all the animations related to the Door Challenges
        public Animator currentPlayerAnimator;

        //MOVEMENT - methods to initiate actions
        public void GetHit()
        {
            currentPlayerAnimator.SetTrigger("GetHit");
        }

        public void Idle()
        {
            currentPlayerAnimator.SetTrigger("IdleNormal");
        }

        public void GetReady()
        {
            currentPlayerAnimator.SetTrigger("IdleAction");
        }


        public void GetInStance()
        {
            currentPlayerAnimator.SetTrigger("IdleFight");
        }

        public void Jump()
        {
            currentPlayerAnimator.SetTrigger("Jump");
        }

        public void RunSlow()
        {
            currentPlayerAnimator.SetBool("RunSlow", true);
        }

        public void RunFast()
        {
            currentPlayerAnimator.SetBool("RunFast", true);
        }

        public void Attack()
        {
            currentPlayerAnimator.SetTrigger("SwordSlash");
        }

        public void AttackSpecial()
        {
            currentPlayerAnimator.SetTrigger("SwordSlash_Special");
        }

        //LOGIC methods to control boolean logic of actions
        public void EquipWeapon(bool state)
        {
            currentPlayerAnimator.SetBool("EquippedSword", state);
        }

        public void SetIdleState(bool state)
        {
            currentPlayerAnimator.SetBool("IsIdle", state);
        }

        public void SetReadyState(bool state)
        {
            currentPlayerAnimator.SetBool("IsAction", state);
        }

        public void SetStanceState(bool state)
        {
            currentPlayerAnimator.SetBool("IsFight", state);
        }

        public void SetSlowState(bool state)
        {
            currentPlayerAnimator.SetBool("IsRunSlow", state);
        }

        public void SetFastState(bool state)
        {
            currentPlayerAnimator.SetBool("IsRunFast", state);
        }

        public void SetUpcloseState(bool state)
        {
            currentPlayerAnimator.SetBool("IsUpclose", state);
        }
    }
}
