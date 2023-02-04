using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class LockOnCursorUI : MonoBehaviour
{
    #region public field
    public GameObject enemyAp;//敵アーマーポイント

    public Image LockOnImage;//ロックオンマーカー画像
    public Image childLockOnImage;//ロックオンマーカー付属画像
    public Image enemyApGaugeImage;//敵アーマーゲージ
    public Text enemyDistanceText;//敵との距離表記
    #endregion

    #region define
    #endregion

    #region field
    Vector3 startPosition;//マーカー初期位置
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
    /// <summary> Ui表示処理 </summary>
    void DisplayUIFunction()
    {
        //ロックオンモードであれば表示
        LockOnImage.enabled = playerBehaviour.isLockOn;
        enemyAp.SetActive(playerBehaviour.isLockOn);

        if (playerBehaviour.isLockOn && playerBehaviour.target != null)
        {

            //ターゲット表示位置にカーソルを合わせる
            //ワールド座標からスクリーン座標への変換のとき、カメラの背後にオブジェクトがあったときもスクリーン上に投影してしまうから対処が必要
            //カメラの向きベクトル、カメラからオブジェクトへのベクトルの内積をとって正であればカメラ前方にオブジェクト、負であるとカメラ後方にオブジェクトがあると分かる
            //Dotは内積の意
            if (Vector3.Dot((playerBehaviour.target.transform.position - Camera.main.transform.position), Camera.main.transform.forward) > 0)
            {
                LockOnImage.transform.position = Camera.main.WorldToScreenPoint(playerBehaviour.target.transform.position);
                childLockOnImage.transform.position = Camera.main.WorldToScreenPoint(playerBehaviour.target.transform.position);
            }


            //敵の体力をゲージに反映
            EnemyBehaviour targetScript = playerBehaviour.target.GetComponent<EnemyBehaviour>();
            enemyApGaugeImage.transform.localScale = new Vector3((float)targetScript.armorPoint / targetScript.armorPointMax, 1, 1);

            //敵との距離を表示する
            enemyDistanceText.text = string.Format("{0:000}", (int)Vector3.Distance(playerBehaviour.target.transform.position, Camera.main.transform.parent.position));


            if (Vector3.Distance(playerBehaviour.target.transform.position, Camera.main.transform.parent.position) < playerBehaviour.LookAtEnemyDistance)
            {
                LockOnImage.transform.rotation = Quaternion.identity;
                childLockOnImage.transform.rotation = Quaternion.identity;
            }
            else
            {
                //サーチ中はカーソル回転
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
