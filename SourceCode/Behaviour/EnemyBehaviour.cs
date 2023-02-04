using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    #region public field
    //アーマー系
    public int armorPoint;//現在のアーマーポイント
    public int armorPointMax = 1000;//アーマー最大値


    //射撃系
    public GameObject shot;//敵の弾
    public GameObject explosion;//爆発エフェクト
    public float shotIntervalMax = 1.0f;//発射間隔
    public int EnemySerchDistance = 30;//プレイヤー発見距離

    public AudioSource shootAudioSource;//発射音
    #endregion

    #region define
    #endregion

    #region field
    float shotIntervalTimer = 0;//射撃間隔タイマー

    GameObject target;
    #endregion

    #region Unity function
    // Start is called before the first frame update
    void Start()
    {
        //ターゲットをプレイヤーに
        target = GameObject.Find("PlayerTarget");

        armorPoint = armorPointMax;
    }

    // Update is called once per frame
    void Update()
    {
        //ターゲットとの距離
        float targetDistance = Vector3.Distance(target.transform.position, transform.position);

        if (targetDistance <= EnemySerchDistance)
        {
            LookAtPlayerFunction();

            ShotIntervalFunction();
            
        }
    }
    #endregion

    #region private function
    /// <summary> 被弾処理 </summary>
    private void OnCollisionEnter(Collision collider)
    {
        if(collider.gameObject.tag == "Shot")
        {
            //被弾ダメージ
            int damage = 0;
            //プレイヤー弾からダメージの値をもらう
            damage = collider.gameObject.GetComponent<PlayerShot>().damage;
            

            //プレイヤーの弾と衝突したらダメージ
            armorPoint -= damage;
           

            //体力0以下で消滅
            if(armorPoint <= 0)
            {
                Destroy(gameObject);
                Instantiate(explosion, transform.position, transform.rotation);

                //リザルト用に加算
                AllSceneManager.score ++;
            }
        }
    }
    
    /// <summary> プレイヤーへ自動で向く処理 </summary>
    void LookAtPlayerFunction()
    {
        //ターゲットをむく
        Quaternion targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);

        targetRotation.x += Random.Range(-0.01f, 0.01f);
        targetRotation.y += Random.Range(-0.01f, 0.01f);
        targetRotation.z += Random.Range(-0.01f, 0.01f);


        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10);
    }

    /// <summary> 次弾発射処理 </summary>
    void ShotIntervalFunction()
    {
        shotIntervalTimer += Time.deltaTime;
        //上限まで時間がたったらショットを生成
        if (shotIntervalTimer > shotIntervalMax)
        {
            Instantiate(shot, transform.position, transform.rotation);
            shotIntervalTimer = 0;

            //発射音
            shootAudioSource.PlayOneShot(shootAudioSource.clip);
        }
    }

    #endregion
}
