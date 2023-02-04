using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShot : MonoBehaviour
{
    #region public field
    public GameObject explosion;//�����G�t�F�N�g
    public GameObject hitEffect;//�����G�t�F�N�g
    public float shotVelocity = 8;//�e��
    public int damage = 200;//�_���[�W��
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
    /// <summary> �������� </summary>
    private void OnCollisionEnter(Collision collider)
    {
        //���e���̃G�t�F�N�g�������_����z���܂��̕����ɔ���
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

    /// <summary> �З͌������� </summary>
    void DecreaseDamage()
    {
        int minDamage = 50;//�Œ�З͒l
        //�З͌�������
        damage --;
        if (damage <= minDamage)
        {
            damage = minDamage;
        }
    }
    #endregion
}
