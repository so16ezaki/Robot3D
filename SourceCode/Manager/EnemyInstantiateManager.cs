using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInstantiateManager : MonoBehaviour
{
    #region public field
    public float instantiateInterval = 3; //生成間隔
    public int instantiateValeu = 3; //生成する数
    public GameObject enemy;
    #endregion

    #region field
    float countDownTimer = 3;//カウントダウンするタイマー
    int currentInstantiateValeu;//残りの生成数
    #endregion

    #region Unity function
    // Start is called before the first frame update
    void Start()
    {
        currentInstantiateValeu = instantiateValeu;
    }

    // Update is called once per frame
    void Update()
    {
        countDownTimer -= Time.deltaTime;

        
        //敵を生成
        InstantiateEnemy();
    }
    #endregion

    #region private function
    /// <summary> 敵を生成する処理 </summary>
    void InstantiateEnemy()
    {
        if (countDownTimer < 0)
        {
            if (currentInstantiateValeu > 0)
            {
                float instantiateRange = 100;//水平方向の生成範囲の絶対値
                float instantiateHeightRangeMax = 50;//高さの生成範囲の上限
                float instantiateHeightRangeMin = 10;//高さの生成範囲の下限
                //敵をランダムな位置に配置
                Instantiate(enemy, new Vector3(Random.Range(-instantiateRange, instantiateRange), Random.Range(instantiateHeightRangeMin, instantiateHeightRangeMax), Random.Range(-instantiateRange, instantiateRange)), Quaternion.identity);

                //生成するごとに生成する数を減らす
                currentInstantiateValeu--;
            }
            //生成間隔を減らしていく
            instantiateInterval -= 0.1f;
            instantiateInterval = Mathf.Clamp(instantiateInterval, 1.0f, float.MaxValue);

            countDownTimer = instantiateInterval;
        }
    }
    #endregion
}
