using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoostUI : MonoBehaviour
{
    #region serialize field
    [SerializeField] Image gaugeImage;//ブーストゲージ画像
    [SerializeField] Color overHeatColor;//オーバーヒート時の色
    #endregion

    #region field
    PlayerBehaviour playerBehaviour;
    #endregion

    #region Unity function
    // Start is called before the first frame update
    void Start()
    {
        playerBehaviour = GameObject.Find("azalea").GetComponent<PlayerBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {


        if (!playerBehaviour.isOverHeat)
        {
            //初期カラー
            gaugeImage.color = Color.white;
            //ブーストゲージの伸縮
            gaugeImage.transform.localScale = new Vector3(((float)playerBehaviour.boostPoint / playerBehaviour.boostPointMax), 1, 1);
        }
        else
        {
            gaugeImage.transform.localScale = new Vector3(1, 1, 1);


            //徐々に色をかえる処理
            float duration = playerBehaviour.overHeatTime;
            float rate = Mathf.Clamp01(playerBehaviour.overHeatTimer / duration);

            gaugeImage.color = Vector4.Lerp(overHeatColor, Color.white, rate);

            //点滅処理
            Color alpha = gaugeImage.color;
            alpha.a = Mathf.Sin(rate * 100);
            gaugeImage.color = alpha;
        }

    }
    #endregion
}
