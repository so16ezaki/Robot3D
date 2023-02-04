using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class LockOnCursorUI : MonoBehaviour
{
    #region public field
    public GameObject enemyAp;//�G�A�[�}�[�|�C���g

    public Image LockOnImage;//���b�N�I���}�[�J�[�摜
    public Image childLockOnImage;//���b�N�I���}�[�J�[�t���摜
    public Image enemyApGaugeImage;//�G�A�[�}�[�Q�[�W
    public Text enemyDistanceText;//�G�Ƃ̋����\�L
    #endregion

    #region define
    #endregion

    #region field
    Vector3 startPosition;//�}�[�J�[�����ʒu
    PlayerBehaviour playerBehaviour;
    #endregion

    #region Unity function
    // Start is called before the first frame update
    void Start()
    {
        LockOnImage.enabled = false;
        startPosition = LockOnImage.transform.position;
        enemyAp.SetActive(false);

        playerBehaviour = GameObject.Find("azalea").GetComponent<PlayerBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        DisplayUIFunction();
    }
    #endregion

    #region private function
    /// <summary> Ui�\������ </summary>
    void DisplayUIFunction()
    {
        //���b�N�I�����[�h�ł���Ε\��
        LockOnImage.enabled = playerBehaviour.isLockOn;
        enemyAp.SetActive(playerBehaviour.isLockOn);

        if (playerBehaviour.isLockOn && playerBehaviour.target != null)
        {

            //�^�[�Q�b�g�\���ʒu�ɃJ�[�\�������킹��
            //���[���h���W����X�N���[�����W�ւ̕ϊ��̂Ƃ��A�J�����̔w��ɃI�u�W�F�N�g���������Ƃ����X�N���[����ɓ��e���Ă��܂�����Ώ����K�v
            //�J�����̌����x�N�g���A�J��������I�u�W�F�N�g�ւ̃x�N�g���̓��ς��Ƃ��Đ��ł���΃J�����O���ɃI�u�W�F�N�g�A���ł���ƃJ��������ɃI�u�W�F�N�g������ƕ�����
            //Dot�͓��ς̈�
            if (Vector3.Dot((playerBehaviour.target.transform.position - Camera.main.transform.position), Camera.main.transform.forward) > 0)
            {
                LockOnImage.transform.position = Camera.main.WorldToScreenPoint(playerBehaviour.target.transform.position);
                childLockOnImage.transform.position = Camera.main.WorldToScreenPoint(playerBehaviour.target.transform.position);
            }


            //�G�̗̑͂��Q�[�W�ɔ��f
            EnemyBehaviour targetScript = playerBehaviour.target.GetComponent<EnemyBehaviour>();
            enemyApGaugeImage.transform.localScale = new Vector3((float)targetScript.armorPoint / targetScript.armorPointMax, 1, 1);

            //�G�Ƃ̋�����\������
            enemyDistanceText.text = string.Format("{0:000}", (int)Vector3.Distance(playerBehaviour.target.transform.position, Camera.main.transform.parent.position));


            if (Vector3.Distance(playerBehaviour.target.transform.position, Camera.main.transform.parent.position) < playerBehaviour.LookAtEnemyDistance)
            {
                LockOnImage.transform.rotation = Quaternion.identity;
                childLockOnImage.transform.rotation = Quaternion.identity;
            }
            else
            {
                //�T�[�`���̓J�[�\����]
                LockOnImage.transform.Rotate(0, 0, Time.deltaTime * 200);
                childLockOnImage.transform.Rotate(0, 0, -Time.deltaTime * 200);

            }
        }
        else
        {

            LockOnImage.transform.position = startPosition;
            LockOnImage.transform.rotation = Quaternion.identity;
            childLockOnImage.transform.rotation = Quaternion.identity;
        }

    }
    #endregion
}
