using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostEffect : MonoBehaviour
{
    #region serialize field
    [SerializeField]
    GameObject player;//プレイヤーのオブジェクト
    [SerializeField]
    GameObject childBoostLightEffect;
    #endregion

    #region field
    GameObject boostLight;//ブースト時のエフェクト
    PlayerBehaviour playerBehaviour;//
    #endregion

    #region Unity function
    // Start is called before the first frame update
    void Start()
    {
        playerBehaviour = player.GetComponent<PlayerBehaviour>();

        boostLight = childBoostLightEffect;
        boostLight.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
       
        //ブースト時にエフェクト適用
        if (playerBehaviour.isBoost)
            boostLight.SetActive(true);
        else
            boostLight.SetActive(false);
    }
    #endregion
}
