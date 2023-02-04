using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityStandardAssets.ImageEffects;

public class AllSceneManager : MonoBehaviour
{
    #region public field
    public Text messageStart;//バトルスタート表記
    public Text messageWin;//バトルの勝利表記
    public Text messageLose;//バトル敗北表記
    public Image backGroud;//背景

    public static int score;//
    public static bool isEnd = false;//

    public Camera resultCamera;//リザルト位置のカメラ
    public GameObject resultCameraObject;//リザルト位置のカメラのオブジェクト
    public GameObject GameUIs;//プレイ画面のUI
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

        //敵の最大生成数をクリア値にする
        Clearscore = enemyInstantiateManager.instantiateValeu;


        messageWin.enabled = false;
        messageLose.enabled = false;
        backGroud.enabled = true;

        //開始時にリザルトカメラオフ
        resultCamera.enabled = false;

        //ゲーム開始時にリザルトカメラの効果無効
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

        //音楽有効化
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
        //プレイ画面のUI表示
        GameUIs.transform.position += new Vector3(0f, 0f, 0f);

        //スコアが出現数に到達したらクリア
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
        //プレイ画面のUI非表示
        GameUIs.transform.position += new Vector3(0f, 1000f, 0f);

        //一定時間たつと遷移可能にする
        timer += Time.deltaTime;

        //勝利時にリザルトカメラオン
        ResultCameraEffect();

        isEnd = true;

        if (timer >= 3)
        {
            //音楽止める
            audioSource.SetActive(false);

            //敵の動きを止める
            Time.timeScale = 0;
            playerBehaviour.rotateSpeed = 0;

            if (Input.GetButtonDown("Fire1"))
            {
                SceneManager.LoadScene("Title");

                //動き再開
                Time.timeScale = 1;

            }

            //遷移可能になったらカメラ効果を有効にする
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
