using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShot : MonoBehaviour
{
    #region public field
    public GameObject explosion;//爆発エフェクト
    public GameObject hitEffect;//命中エフェクト
    public float shotVelocity = 8;//弾速
    public int damage = 200;//ダメージ量
    #endregion

    #region Unity function
    // Start is called before the first frame update
    void Start()
    {
        float destroyTime = 3.0f;
        Destroy(gameObject, destroyTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * 100 * shotVelocity;

        DecreaseDamage();
    }
    #endregion

    #region private function
    /// <summary> 命中処理 </summary>
    private void OnCollisionEnter(Collision collider)
    {
        //着弾時のエフェクトをランダムなz軸まわりの方向に発生
        int num;
        Quaternion randomQ;
        num = Random.Range(-180, 180);
        randomQ = Quaternion.Euler(0, 0, num);
        Destroy(gameObject);

        if (collider.gameObject.tag == "Enemy")
            Instantiate(hitEffect, transform.position, randomQ);
        else
            Instantiate(explosion, transform.position, randomQ);
        

    }

    /// <summary> 威力減衰処理 </summary>
    void DecreaseDamage()
    {
        int minDamage = 50;//最低威力値
        //威力減衰処理
        damage --;
        if (damage <= minDamage)
        {
            damage = minDamage;
        }
    }
    #endregion
}
