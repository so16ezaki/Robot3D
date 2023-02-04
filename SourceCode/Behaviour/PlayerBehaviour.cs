using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;  
using UnityStandardAssets.ImageEffects;

public class PlayerBehaviour : MonoBehaviour
{
    #region public field
    //�A�[�}�[�n
    public int _ArmorPoint;//�A�[�}�[�|�C���g
    public int armorPointMax = 5000;//�A�[�}�[�|�C���g�ő�l


    //�u�[�X�g�n
    public float boostPoint;//���݂̃u�[�X�g��
    public float boostPointMax = 100;//�u�[�X�g�e��
    public float overHeatTime = 5;//�I�[�o�[�q�[�g����
    public float boostCoolingSpeed = 1.5f;//�u�[�X�g�񕜑��x
    public float boostConsumSpeed = 1;//�u�[�X�g����x

    public bool isBoost;//�u�[�X�g���
    public bool isOverHeat;//�I�[�o�[�q�[�g���

    public float boostTimer;//�u�[�X�g���Ԍv���^�C�}�[
    public float overHeatTimer;//�I�[�o�[�q�[�g���Ԍv���^�C�}�[

    public GameObject audioSoruceWarning;//�I�[�o�[�q�[�g���̌x����


    //����n
    public int rotateSpeed = 2;//���񑬓x


    //���b�N�I���n
    public float LookAtEnemyDistance = 100;//�G�������Ō������E����
    public float LockOnDistance = 500;//�G�����b�N�I��������E����

    public GameObject target = null;//�^�[�Q�b�g

    public bool isLockOn;//���b�N�I�����[�h���


    //�ˌ��n
    public GameObject shot;//�e
    public GameObject muzzle;//�e���I�u�W�F�N�g
    public GameObject muzzleFlash;//�}�Y���t���b�V���G�t�F�N�gsss
    
    public float shotIntervalMax = 0.4f;//�ˌ��Ԋu

    public AudioSource shootAudioSource;//���ˉ�

    public GameObject cameraPivot;
    #endregion

    #region define
    const float gravity = 20f;//�d��
    const float addNormalSpeed = 1; //�ʏ펞�̉��Z���x
    const float addBoostSpeed = 2; //�u�[�X�g���̉��Z���x
    const float normalMaxSpeed = 20; //�ʏ펞�̍ő呬�x
    const float boostMaxSpeed = 40; //�u�[�X�g���̍ő呬�x
    #endregion

    #region field
    //���x�x�N�g��
    float shotIntervalTimer = 0;//�ˌ��Ԋu�^�C�}�[

    Vector3 moveDirection = Vector3.zero;
    Vector3 moveSpeed = Vector3.zero;
    CharacterController controller;//�v���C���[�R���g���[���[
    #endregion

    #region Unity function
    // Start is called before the first frame update
    void Start()
    {
        _ArmorPoint = armorPointMax;//�A�[�}�[�|�C���g�����ݒ�
        boostPoint = boostPointMax;//�u�[�X�g�|�C���g�����ݒ�
        controller = GetComponent<CharacterController>();

        isLockOn = false;//
        shootAudioSource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //�����ړ�����
        MoveFunction();
        //��]����
        RotateFunction();
        //�W�����v����
        JumpFunction();
     
        //���[�J�����W�̃x�N�g��moveDirection�����[���h���W�n�ɕϊ�
        moveDirection = transform.TransformDirection(moveDirection);
        //�@�̂̃R���g���[���[�Ɉړ��w�N�g����K�p
        controller.Move(moveDirection * Time.deltaTime);

        //�u�[�X�g����
        BoostFunction();
        //�I�[�o�[�q�[�g����
        OverHeatFunction();

        //���[�V�����u���[����
        motionBlurFunction();
        //�J�����U������
        CameraVibrationFunction();

     
        //���b�N���[�h�ؑ֏���
        ChangeLockModeFunction();

        //���b�N���[�h�œG������ΓG�̕���������
        if (isLockOn == true)
        {
            if (target != null)
                LookAtEnemyFunction();
            else
                target = FindClosestEnemy();//���b�N�I�����[�h�łȂ���ΓG��T��
        }

        //�ˌ�����
        ShootFunction();

    }
    #endregion

    #region private function
    /// <summary> ��e���� </summary>
    private void OnCollisionEnter(Collision collider)
    {
        //�G�̒e�ƏՓ�    
        if (collider.gameObject.tag == "ShotEnemy")
        {
            //��e�_���[�W
            int damage = 0;
            //�G�e����_���[�W�̒l�����炤
            damage = collider.gameObject.GetComponent<EnemyShot>().damage;

            _ArmorPoint -= damage;
            //Mathf.Clamp(�Ώۂ̕ϐ�,����,���)�Ŏw��̒l�Ɍ��x�𒲐�����
            _ArmorPoint = Mathf.Clamp(_ArmorPoint, 0, armorPointMax);
        }
    }

    /// <summary> �����ړ����� </summary>
    private void MoveFunction ()
    {
        float acceleration;//�����x
        Vector3 maxSpeed = Vector3.zero;//���E���x
       
        

        

        maxSpeed = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));//�L�[�ɂ����E���x�̃x�N�g���擾
        maxSpeed = maxSpeed.normalized;//���E���x���K��

        //�u�[�X�g���Ă���Ƃ��Ɓ@���Ă��Ȃ��Ƃ��ŉ����x�ƌ��E���x��ς���
        if (isBoost)
        {
            acceleration = addBoostSpeed;
            maxSpeed *= boostMaxSpeed; 
        }
        else
        {
            acceleration = addNormalSpeed;
            maxSpeed *= normalMaxSpeed;
        }

        //���E���x�։����x����������
        moveSpeed.x = Mathf.MoveTowards(moveSpeed.x, maxSpeed.x, acceleration);
        moveSpeed.z = Mathf.MoveTowards(moveSpeed.z, maxSpeed.z, acceleration);
        

        moveDirection = new Vector3(moveSpeed.x, moveDirection.y, moveSpeed.z);
        
    }

    /// <summary> ��]���� </summary>
    void RotateFunction()
    {
        //�����}�E�X����
        transform.Rotate(0, Input.GetAxis("Mouse X") * rotateSpeed, 0);

        //�����}�E�X����
        cameraPivot.transform.Rotate(-Input.GetAxis("Mouse Y") * rotateSpeed, 0, 0);
    }

    /// <summary> �W�����v���� </summary>
    void JumpFunction()
    {
        float maxAltitude = 100;//���E���x

        //�n��ɂ���Ƃ��d�͂𔭐������Ȃ�
        if (controller.isGrounded)
            moveDirection.y = 0;

        //�W�����v�L�[�������ꂽ�Ƃ��@���@�I�[�o�[�q�[�g�łȂ��Ƃ��W�����v
        if (Input.GetButton("Jump") && !isOverHeat)
        {
            if (transform.position.y > maxAltitude)
                moveDirection.y = 0;
            else
                moveDirection.y += gravity * Time.deltaTime;

            isBoost = true;

        }
        else
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }
    }

    /// <summary> �u�[�X�g���� </summary>
    void BoostFunction()
    {
        //�u�[�X�g�̏�Ԃ�isBoost�ɒ�`
        if ((Input.GetButton("Boost") || Input.GetButton("Jump")) && !isOverHeat)
        {
            isBoost = true;
        }
        else
        {
            isBoost = false;
        }



        //�u�[�X�g����Ɖ񕜏���
        if (!isBoost && !isOverHeat)
            boostPoint += boostCoolingSpeed;
        else
            boostPoint -= boostConsumSpeed;

        boostPoint = Mathf.Clamp(boostPoint, 0, boostPointMax);
        
    }

    /// <summary> �I�[�o�[�q�[�g���� </summary>
    void OverHeatFunction()
    {
        //�I�[�o�[�q�[�g��Ԃ�isOverHeat�ɒ�`
        if (boostPoint <= 1)
        {
            isOverHeat = true;
        }
        else
        {
            isOverHeat = false;
        }


        float warnigBoostRate = 0.3f;//�x�������o���n�߂�u�[�X�g����
        //�I�[�o�[�q�[�g�x����
        if (isOverHeat || boostPoint / boostPointMax < warnigBoostRate)
            audioSoruceWarning.SetActive(true);
        else
            audioSoruceWarning.SetActive(false);


        if (isOverHeat)
        {
            overHeatTimer += Time.deltaTime;
            //�I�[�o�[�q�[�g��������
            if (overHeatTimer >= overHeatTime)
            {
                isOverHeat = false;
                overHeatTimer = 0f;
                boostPoint = boostPointMax;
            }
        }
    }

    /// <summary> ���[�V�����u���[���� </summary>
    void motionBlurFunction()
    {

        //�ړ����x�ɍ��킹�ă��[�V�����u���[�̒l��ς���
        float motionBlurValue = Mathf.Max(Mathf.Abs(moveSpeed.x), Mathf.Abs(moveSpeed.z)) / 20;
        motionBlurValue = Mathf.Clamp(motionBlurValue, 0, 5) * 0.5f;


        Camera.main.GetComponent<CameraMotionBlur>().velocityScale = motionBlurValue;

    }

    /// <summary> �J�����U������ </summary>
    void CameraVibrationFunction()
    {

        //�u�[�X�g����ʂ�h�炷����
        if (isBoost)
        {
            boostTimer += Time.deltaTime;

            //vibration����
            CamVibrationManager.vibration = Mathf.Clamp(0.5f - (boostTimer), 0.05f, 0.3f);

        }
        else
            boostTimer = 0f;


    }

    /// <summary> �ߋ����̓G���^�[�Q�b�g�ɂ��鏈�� </summary>
    GameObject FindClosestEnemy()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;

        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;

            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;

            }
        }



        return closest;
    }

    /// <summary> ���b�N�I�����[�h�ؑ֏��� </summary>
    void ChangeLockModeFunction()
    {
        //�L�[�ɂ�郍�b�N�؂�ւ�
        if (Input.GetButtonDown("Lock"))
        {
            //���b�N���[�h�ؑ�
            isLockOn = !isLockOn;

            //���b�N����
            if (!isLockOn)
                target = null;

        }



        //�G�\���̋����ɂ�郍�b�N����
        if (target != null)
        {
            //�^�[�Q�b�g�Ƃ̋���
            float targetDistance = Vector3.Distance(target.transform.position, transform.position);

            if (targetDistance > LockOnDistance)
            {
                target = null;
                isLockOn = false;

            }
        }

    }

    /// <summary> �G�֎����Ō������� </summary>
    void LookAtEnemyFunction()
    {
        if (Vector3.Distance(target.transform.position, transform.position) < LookAtEnemyDistance)
        {
            //�^�[�Q�b�g�Ɍ���



            Quaternion targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);



            //�@�̂𒆐S�Ƀ}�Y�����G�������悤�ɉ���]������
            var addRotationY = Mathf.Asin(3.63f / (Vector3.Distance(target.transform.position, transform.position)));
            Quaternion QAddRotation = Quaternion.AngleAxis(-addRotationY * Mathf.Rad2Deg, Vector3.up);
            targetRotation *= QAddRotation;



            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10);

            transform.rotation = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w);

            //�J�����s�{�b�g�𒆐S�ɓG�������悤�ɏc��]
            Quaternion targetRotation2 = Quaternion.LookRotation(target.transform.position - cameraPivot.transform.position);
            cameraPivot.transform.localRotation = Quaternion.Slerp(cameraPivot.transform.localRotation, targetRotation2, Time.deltaTime * 10);

            cameraPivot.transform.localRotation = new Quaternion(cameraPivot.transform.localRotation.x, 0, 0, cameraPivot.transform.localRotation.w);
        }
    }

    /// <summary> �ˌ����� </summary>
    void ShootFunction()
    {
        shotIntervalTimer += Time.deltaTime;

        if (Input.GetButton("Fire1") && !AllSceneManager.isEnd)
        {
            if (shotIntervalTimer > shotIntervalMax)
            {
                //�e������
                Instantiate(shot, muzzle.transform.position, Camera.main.transform.rotation);
                shotIntervalTimer = 0;

                //�܂���t���b�V������
                Instantiate(muzzleFlash, muzzle.transform.position, muzzle.transform.rotation);

                //���ˉ�
                shootAudioSource.PlayOneShot(shootAudioSource.clip);
            }
        }
    }

    #endregion
}
