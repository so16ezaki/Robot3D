using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;

public class PlayerAPUI : MonoBehaviour
{


    #region public field
    public Text armorText;//�A�[�}�[�|�C���g�\�L�e�L�X�g
    public Image gaugeImage;//�A�[�}�[�|�C���g�Q�[�W
    public Color fifteenPer;//�A�[�}�[50�p�[�Z���g�ȏ㎞�̐F
    public Color thrteenPer;//�A�[�}�[30�p�[�Z���g�ȏ㎞�̐F
    public Color underThrteenPer;//�A�[�}�[30�p�[�Z���g�������̐F

    public GameObject damageAudio;//������

    public GameObject player;
    #endregion

    #region define
    #endregion

    #region field
    int displayArmorPoint;//�\�������A�[�}�[�|�C���g

    PlayerBehaviour playerBehaviour;
    #endregion

    #region Unity function
    // Start is called before the first frame update
    void Start()
    {
        playerBehaviour = player.GetComponent<PlayerBehaviour>();  
        displayArmorPoint = playerBehaviour.armorPointMax;


        //�Q�[���J�n���Ƀm�C�Y�𖳌���
        Camera.main.GetComponent<NoiseAndScratches>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

        DecreaseArmorPointFunction();

        //�̗͂�UItext�ɕ\��
        armorText.text = string.Format("{0:0000}/{1:0000}", displayArmorPoint, playerBehaviour.armorPointMax);


        float armorPointRate = (float)playerBehaviour._ArmorPoint / playerBehaviour.armorPointMax;//�A�[�}�[�̊���
        ChangeColorFunction(armorPointRate);

        //�Q�[�W���A�[�}�[�|�C���g�ƑΉ�
        gaugeImage.transform.localScale = new Vector3(armorPointRate, 1, 1);
    }
    #endregion

    #region private function
    /// <summary> �̗͂����炷���� </summary>
    void DecreaseArmorPointFunction()
    {
        if (displayArmorPoint != playerBehaviour._ArmorPoint)
            displayArmorPoint = (int)Mathf.Lerp(displayArmorPoint, playerBehaviour._ArmorPoint, 0.1f);
    }
    /// <summary> �����ɂ���ĐF��ς��鏈�� </summary>
    void ChangeColorFunction(float armorPointRate)
    {
        

        if (armorPointRate >= 0.5f)//�̗�5���ȏ�
        {
            armorText.color = fifteenPer;
            gaugeImage.color = new Color(0.25f, 0.7f, 0.6f);
            //������
            damageAudio.SetActive(false);
        }
        else if (armorPointRate >= 0.3f)//�̗�3���ȏ�
        {
            armorText.color = thrteenPer;
            gaugeImage.color = thrteenPer;
            //������
            damageAudio.SetActive(false);
        }
        else//�̗�3������
        {
            armorText.color = underThrteenPer;
            gaugeImage.color = underThrteenPer;

            //�v���C���[�̗̑͂�3���؂�ƃm�C�Y�L��
            Camera.main.GetComponent<NoiseAndScratches>().enabled = true;

            //������
            damageAudio.SetActive(true);

        }
    }
    #endregion
}
