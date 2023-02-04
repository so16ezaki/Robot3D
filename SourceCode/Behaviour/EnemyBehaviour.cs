using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    #region public field
    //�A�[�}�[�n
    public int armorPoint;//���݂̃A�[�}�[�|�C���g
    public int armorPointMax = 1000;//�A�[�}�[�ő�l


    //�ˌ��n
    public GameObject shot;//�G�̒e
    public GameObject explosion;//�����G�t�F�N�g
    public float shotIntervalMax = 1.0f;//���ˊԊu
    public int EnemySerchDistance = 30;//�v���C���[��������

    public AudioSource shootAudioSource;//���ˉ�
    #endregion

    #region define
    #endregion

    #region field
    float shotIntervalTimer = 0;//�ˌ��Ԋu�^�C�}�[

    GameObject target;
    #endregion

    #region Unity function
    // Start is called before the first frame update
    void Start()
    {
        //�^�[�Q�b�g���v���C���[��
        target = GameObject.Find("PlayerTarget");

        armorPoint = armorPointMax;
    }

    // Update is called once per frame
    void Update()
    {
        //�^�[�Q�b�g�Ƃ̋���
        float targetDistance = Vector3.Distance(target.transform.position, transform.position);

        if (targetDistance <= EnemySerchDistance)
        {
            LookAtPlayerFunction();

            ShotIntervalFunction();
            
        }
    }
    #endregion

    #region private function
    /// <summary> ��e���� </summary>
    private void OnCollisionEnter(Collision collider)
    {
        if(collider.gameObject.tag == "Shot")
        {
            //��e�_���[�W
            int damage = 0;
            //�v���C���[�e����_���[�W�̒l�����炤
            damage = collider.gameObject.GetComponent<PlayerShot>().damage;
            

            //�v���C���[�̒e�ƏՓ˂�����_���[�W
            armorPoint -= damage;
           

            //�̗�0�ȉ��ŏ���
            if(armorPoint <= 0)
            {
                Destroy(gameObject);
                Instantiate(explosion, transform.position, transform.rotation);

                //���U���g�p�ɉ��Z
                AllSceneManager.score ++;
            }
        }
    }
    
    /// <summary> �v���C���[�֎����Ō������� </summary>
    void LookAtPlayerFunction()
    {
        //�^�[�Q�b�g���ނ�
        Quaternion targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);

        targetRotation.x += Random.Range(-0.01f, 0.01f);
        targetRotation.y += Random.Range(-0.01f, 0.01f);
        targetRotation.z += Random.Range(-0.01f, 0.01f);


        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10);
    }

    /// <summary> ���e���ˏ��� </summary>
    void ShotIntervalFunction()
    {
        shotIntervalTimer += Time.deltaTime;
        //����܂Ŏ��Ԃ���������V���b�g�𐶐�
        if (shotIntervalTimer > shotIntervalMax)
        {
            Instantiate(shot, transform.position, transform.rotation);
            shotIntervalTimer = 0;

            //���ˉ�
            shootAudioSource.PlayOneShot(shootAudioSource.clip);
        }
    }

    #endregion
}
