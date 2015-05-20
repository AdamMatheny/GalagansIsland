using UnityEngine;
using System.Collections;

//NOTHING IN THIS SCRIPT IS GETTING USED OR CALLED ANYWHERE, WHY DO WE HAVE IT?

public class PlayerAnimationEvents : MonoBehaviour 
{
//
//    private Animator playerAnimator = null;
//    public GameObject bulletPrefab = null;
//    public Transform bulletSpawnTransform;
//
//    public void SetDashOff()
//    {
//        PlayerStateListener otherScript = GetComponent<PlayerStateListener>();
//        otherScript.SetWalkSpeed(1.0f);
//        playerAnimator = GetComponent<Animator>();
//        playerAnimator.SetBool("DashF", false);         
//    }
//
//    public void SetJumpOff()
//    {
//        playerAnimator = GetComponent<Animator>();
//        playerAnimator.SetBool("JumpF", false);
//        playerAnimator.SetBool("Falling", true);
//    }
//
//    public void SetShootOff(int value)
//    {
//        if (value == 0)
//        {
//            playerAnimator = GetComponent<Animator>();
//            playerAnimator.SetBool("Shoot", false);
//        }
//    }
//
//    public void CreateCapBullet()
//    {
//        // Make the bullet object
//        GameObject newBullet = (GameObject)Instantiate(bulletPrefab);
//
//
//        // Acquire the PlayerBulletController component on the new object so we can specify some data
//        PlayerBulletController bullCon = newBullet.GetComponent<PlayerBulletController>();
//
//        // Set the player object
//        bullCon.playerObject = gameObject;
//        // Setup the bullet’s starting position
//        newBullet.transform.position = bulletSpawnTransform.transform.position;
//
//        // Launch the bullet!
//        bullCon.launchBullet();
//
//        //playerAnimator = GetComponent<Animator>();
//        //playerAnimator.SetBool("Shield", false);
//        
//        //Let us move again
//        //PlayerStateListener otherScript = GetComponent<PlayerStateListener>();
//       // otherScript.SetWalkSpeed(1.0f);
//    }
}
