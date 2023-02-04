using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInstantiateManager : MonoBehaviour
{
    #region public field
    public float instantiateInterval = 3; //�����Ԋu
    public int instantiateValeu = 3; //�������鐔
    public GameObject enemy;
    #endregion

    #region field
    float countDownTimer = 3;//�J�E���g�_�E������^�C�}�[
    int currentInstantiateValeu;//�c��̐�����
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

        
        //�G�𐶐�
        InstantiateEnemy();
    }
    #endregion

    #region private function
    /// <summary> �G�𐶐����鏈�� </summary>
    void InstantiateEnemy()
    {
        if (countDownTimer < 0)
        {
            if (currentInstantiateValeu > 0)
            {
                float instantiateRange = 100;//���������̐����͈͂̐�Βl
                float instantiateHeightRangeMax = 50;//�����̐����͈͂̏��
                float instantiateHeightRangeMin = 10;//�����̐����͈͂̉���
                //�G�������_���Ȉʒu�ɔz�u
                Instantiate(enemy, new Vector3(Random.Range(-instantiateRange, instantiateRange), Random.Range(instantiateHeightRangeMin, instantiateHeightRangeMax), Random.Range(-instantiateRange, instantiateRange)), Quaternion.identity);

                //�������邲�Ƃɐ������鐔�����炷
                currentInstantiateValeu--;
            }
            //�����Ԋu�����炵�Ă���
            instantiateInterval -= 0.1f;
            instantiateInterval = Mathf.Clamp(instantiateInterval, 1.0f, float.MaxValue);

            countDownTimer = instantiateInterval;
        }
    }
    #endregion
}
