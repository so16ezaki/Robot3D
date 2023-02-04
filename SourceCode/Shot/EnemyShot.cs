using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class EnemyShot : MonoBehaviour
{
    #region public field
    public GameObject explosion;//爆発エフェクト
    public GameObject hitEffect;//命中エフェクト
    public float shotVelocity = 5;//弾速
    public int damage = 100;//ダメージ量
    #endregion

    #region Unity function
    // Start is called before the first frame update
    void Start()
    {
        float destroyTime = 2.0f;
        Destroy(gameObject, destroyTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * 100 * shotVelocity;
    }
    #endregion

    #region private function
    private void OnCollisionEnter(Collision collider)
    {
        //着弾時のエフェクトをランダムなz軸まわりの方向に発生
        int num;
        Quaternion randomQ;
        num = Random.Range(-180, 180);
        randomQ = Quaternion.Euler(0, 0, num);
        Destroy(gameObject);

        if (collider.gameObject.tag == "Player")
            Instantiate(hitEffect, transform.position, randomQ);
        else
            Instantiate(explosion, transform.position, randomQ);


    }
    #endregion
}
