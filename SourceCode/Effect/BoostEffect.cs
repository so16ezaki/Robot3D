using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostEffect : MonoBehaviour
{
    #region serialize field
    [SerializeField]
    GameObject player;//�v���C���[�̃I�u�W�F�N�g
    [SerializeField]
    GameObject childBoostLightEffect;
    #endregion

    #region field
    GameObject boostLight;//�u�[�X�g���̃G�t�F�N�g
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
       
        //�u�[�X�g���ɃG�t�F�N�g�K�p
        if (playerBehaviour.isBoost)
            boostLight.SetActive(true);
        else
            boostLight.SetActive(false);
    }
    #endregion
}
