using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;

public class PlayerAPUI : MonoBehaviour
{


    #region public field
    public Text armorText;//アーマーポイント表記テキスト
    public Image gaugeImage;//アーマーポイントゲージ
    public Color fifteenPer;//アーマー50パーセント以上時の色
    public Color thrteenPer;//アーマー30パーセント以上時の色
    public Color underThrteenPer;//アーマー30パーセント未満時の色

    public GameObject damageAudio;//損傷音

    public GameObject player;
    #endregion

    #region define
    #endregion

    #region field
    int displayArmorPoint;//表示されるアーマーポイント

    PlayerBehaviour playerBehaviour;
    #endregion

    #region Unity function
    // Start is called before the first frame update
    void Start()
    {
        playerBehaviour = player.GetComponent<PlayerBehaviour>();  
        displayArmorPoint = playerBehaviour.armorPointMax;


        //ゲーム開始時にノイズを無効に
        Camera.main.GetComponent<NoiseAndScratches>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

        DecreaseArmorPointFunction();

        //体力をUItextに表示
        armorText.text = string.Format("{0:0000}/{1:0000}", displayArmorPoint, playerBehaviour.armorPointMax);


        float armorPointRate = (float)playerBehaviour._ArmorPoint / playerBehaviour.armorPointMax;//アーマーの割合
        ChangeColorFunction(armorPointRate);

        //ゲージをアーマーポイントと対応
        gaugeImage.transform.localScale = new Vector3(armorPointRate, 1, 1);
    }
    #endregion

    #region private function
    /// <summary> 体力を減らす処理 </summary>
    void DecreaseArmorPointFunction()
    {
        if (displayArmorPoint != playerBehaviour._ArmorPoint)
            displayArmorPoint = (int)Mathf.Lerp(displayArmorPoint, playerBehaviour._ArmorPoint, 0.1f);
    }
    /// <summary> 割合によって色を変える処理 </summary>
    void ChangeColorFunction(float armorPointRate)
    {
        

        if (armorPointRate >= 0.5f)//体力5割以上
        {
            armorText.color = fifteenPer;
            gaugeImage.color = new Color(0.25f, 0.7f, 0.6f);
            //損傷音
            damageAudio.SetActive(false);
        }
        else if (armorPointRate >= 0.3f)//体力3割以上
        {
            armorText.color = thrteenPer;
            gaugeImage.color = thrteenPer;
            //損傷音
            damageAudio.SetActive(false);
        }
        else//体力3割未満
        {
            armorText.color = underThrteenPer;
            gaugeImage.color = underThrteenPer;

            //プレイヤーの体力が3割切るとノイズ有効
            Camera.main.GetComponent<NoiseAndScratches>().enabled = true;

            //損傷音
            damageAudio.SetActive(true);

        }
    }
    #endregion
}
