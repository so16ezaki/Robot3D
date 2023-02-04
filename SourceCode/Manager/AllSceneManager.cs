using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityStandardAssets.ImageEffects;

public class AllSceneManager : MonoBehaviour
{
    #region public field
    public Text messageStart;//�o�g���X�^�[�g�\�L
    public Text messageWin;//�o�g���̏����\�L
    public Text messageLose;//�o�g���s�k�\�L
    public Image backGroud;//�w�i

    public static int score;//
    public static bool isEnd = false;//

    public Camera resultCamera;//���U���g�ʒu�̃J����
    public GameObject resultCameraObject;//���U���g�ʒu�̃J�����̃I�u�W�F�N�g
    public GameObject GameUIs;//�v���C��ʂ�UI
    #endregion

    #region define
    const int BATTLE_START = 0;
    const int BATTLE_PLAY = 1;
    const int BATTLE_END = 2;
    #endregion

    #region field
    int Clearscore;//
    int battleStatus;//
    float timer = 0;//
    PlayerBehaviour playerBehaviour;
    EnemyInstantiateManager enemyInstantiateManager;
    GameObject audioSource;
    #endregion

    #region Unity function
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GameObject.Find("AudioSource");

        enemyInstantiateManager = GetComponent<EnemyInstantiateManager>();

        playerBehaviour = GameObject.Find("azalea").GetComponent<PlayerBehaviour>();

        battleStatus = BATTLE_START;
        messageStart.enabled = true;

        score = 0;

        //�G�̍ő吶�������N���A�l�ɂ���
        Clearscore = enemyInstantiateManager.instantiateValeu;


        messageWin.enabled = false;
        messageLose.enabled = false;
        backGroud.enabled = true;

        //�J�n���Ƀ��U���g�J�����I�t
        resultCamera.enabled = false;

        //�Q�[���J�n���Ƀ��U���g�J�����̌��ʖ���
        Camera.main.GetComponent<ColorCorrectionCurves>().enabled = false;
        Camera.main.GetComponent<DepthOfField>().enabled = false;

        resultCameraObject.GetComponent<ColorCorrectionCurves>().enabled = false;
        resultCameraObject.GetComponent<DepthOfField>().enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        
        switch (battleStatus)
        {
            case BATTLE_START:
                Battle_StartFunction();
                break;

            case BATTLE_PLAY:
                Battle_PlayFunction();
                break;

            case BATTLE_END:
                Battle_EndFunction();
                break;

            default:
                break;
        }
       
    }
    #endregion

    #region private function
    void Battle_StartFunction()
    {
        timer += Time.deltaTime;

        //���y�L����
        audioSource.SetActive(true);

        if (timer > 3)
        {
            messageStart.enabled = false;
            backGroud.enabled = false;

            battleStatus = BATTLE_PLAY;

            timer = 0;
        }
    }

    void Battle_PlayFunction()
    {
        //�v���C��ʂ�UI�\��
        GameUIs.transform.position += new Vector3(0f, 0f, 0f);

        //�X�R�A���o�����ɓ��B������N���A
        playerBehaviour.rotateSpeed = 2;
        isEnd = false;

        if (score >= Clearscore)
        {
            battleStatus = BATTLE_END;
            messageWin.enabled = true;
            backGroud.enabled = true;

            

        }

        if (playerBehaviour._ArmorPoint <= 0)
        {
            battleStatus = BATTLE_END;
            messageLose.enabled = true;
            backGroud.enabled = true;
        }

    }

    void Battle_EndFunction()
    {
        //�v���C��ʂ�UI��\��
        GameUIs.transform.position += new Vector3(0f, 1000f, 0f);

        //��莞�Ԃ��ƑJ�ډ\�ɂ���
        timer += Time.deltaTime;

        //�������Ƀ��U���g�J�����I��
        ResultCameraEffect();

        isEnd = true;

        if (timer >= 3)
        {
            //���y�~�߂�
            audioSource.SetActive(false);

            //�G�̓������~�߂�
            Time.timeScale = 0;
            playerBehaviour.rotateSpeed = 0;

            if (Input.GetButtonDown("Fire1"))
            {
                SceneManager.LoadScene("Title");

                //�����ĊJ
                Time.timeScale = 1;

            }

            //�J�ډ\�ɂȂ�����J�������ʂ�L���ɂ���
            if (messageWin.enabled == true)
            {
                resultCameraObject.GetComponent<ColorCorrectionCurves>().enabled = true;
                resultCameraObject.GetComponent<DepthOfField>().enabled = true;
            }
            else
            {
                Camera.main.GetComponent<ColorCorrectionCurves>().enabled = true;
                Camera.main.GetComponent<DepthOfField>().enabled = true;
            }

        }
    }
    void ResultCameraEffect()
    {

        Vector3 resultCameraPosition = new Vector3(-3.5f, 7.5f, 5f);
        Quaternion resultCameraRotation = Quaternion.Euler(16, 155, 0);

        resultCamera.enabled = true;

        resultCameraObject.transform.localPosition = Vector3.MoveTowards(resultCameraObject.transform.localPosition, resultCameraPosition, 2);
        resultCameraObject.transform.localRotation = Quaternion.RotateTowards(resultCameraObject.transform.localRotation, resultCameraRotation, 10);
    }
    #endregion
}
