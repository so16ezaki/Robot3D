using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotion : MonoBehaviour
{
    #region field
    Animator animetor;
    PlayerBehaviour playerMove;
    #endregion

    #region Unity function
    // Start is called before the first frame update
    void Start()
    {
        animetor = GetComponent<Animator>();
      
        playerMove = GetComponent<PlayerBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        //左右
        if (Input.GetAxis("Horizontal") == 0)
        {
            animetor.SetInteger("Horizontal", 0);
        }

        if (Input.GetAxis("Horizontal") > 0)
        {
            animetor.SetInteger("Horizontal", 1);
        }

        if (Input.GetAxis("Horizontal") < 0)
        {
            animetor.SetInteger("Horizontal", -1);
        }

        //前後
        if (Input.GetAxis("Vertical") == 0)
        {
            animetor.SetInteger("Vertical", 0);
        }

        if (Input.GetAxis("Vertical") > 0)
        {
            animetor.SetInteger("Vertical", 1);
        }

        if (Input.GetAxis("Vertical") < 0)
        {
            animetor.SetInteger("Vertical", -1);
        }

        if (!playerMove.isOverHeat)
        {
           
            //ジャンプ
            animetor.SetBool("Jump", Input.GetButton("Jump"));

            //ブーストパラメータを切り替え
            animetor.SetBool("Boost", Input.GetButton("Boost"));
        }
        else
        {
            animetor.SetBool("Jump", false);
            animetor.SetBool("Boost", false);
        }

    }
    #endregion
}
