using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
   private Animator anim;
   
   private float horizontal;
   private float vertical;

   private void Start()
   {
      anim = GetComponent<Animator>();
   }

   private void Update()
   {
      horizontal = Input.GetAxis("Horizontal");
      vertical = Input.GetAxis("Vertical");
      if (horizontal  != 0 || vertical != 0 )
      {
         anim.SetBool("isWalk",true);
      }
      else
      {
         anim.SetBool("isWalk",false);

      }      
      
      
   }
}
