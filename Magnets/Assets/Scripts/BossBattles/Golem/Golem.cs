using RootMotion.FinalIK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

public class Golem : MonoBehaviour
{
    [SerializeField] private Transform[] spineBones;
    [SerializeField] private Transform chainsawDistCheck;
    [SerializeField] private Transform shootPos;
    [SerializeField] private Transform chainsawCenter;
    [SerializeField] private ParticleSystem gunCharge;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject laser;
    private NavMeshAgent agent;
    private Animator anims;
    private AimIK leftArmAim;
    private AimIK rightArmAim;
    Vector2 animBlend;
    Vector2 animsCurrent = Vector2.zero;
    Vector3 chestForward;
    Vector3 feetForward;
    float x;
    float timer;
    float shootTimer;
    private enum AttackState { None = 0, Hand = 1, Laser = 2, Chainsaw = 3 }
    private AttackState attackState;
    private System.Random rand = new System.Random();
    Transform target;
    Quaternion[] prevSpineRots = new Quaternion[2];
    bool shooting = false;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anims = GetComponent<Animator>();
        anims.applyRootMotion = false;
        feetForward = transform.forward;
        leftArmAim = GetComponents<AimIK>()[0];
        rightArmAim = GetComponents<AimIK>()[1];
        AddAgent();
        leftArmAim.solver.target = target;
        rightArmAim.solver.target = target;
        StartCoroutine(ChooseAttackMode());
    }

    void Update()
    {
        if (target == null)
            return;
        if (agent != null)
            GoToTarget();

        CalculateChainSaw();

        float y = agent.velocity.magnitude > 0 ? 1 : 0;

        if (Vector3.Angle(chestForward, feetForward) >= 45)
            Turn(Vector3.SignedAngle(chestForward, feetForward, Vector3.up));

        if(timer > 0.5f)
        {
            feetForward = chestForward;
            Quaternion rotTo = Quaternion.LookRotation(Vector3.ProjectOnPlane(prevSpineRots[0] * Vector3.forward, Vector3.up), Vector3.up);
            float curY = transform.rotation.eulerAngles.y > 180 ? transform.rotation.eulerAngles.y - 360 : transform.rotation.eulerAngles.y;
            float newY = rotTo.eulerAngles.y > 180 ? rotTo.eulerAngles.y - 360 : rotTo.eulerAngles.y;

            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, Mathf.LerpAngle(curY, newY, 1 - timer), transform.rotation.eulerAngles.z);
            timer -= Time.deltaTime / 2.2f;
        }
        else
        {
            timer = 0;
            x = 0;
        }

        animBlend = new Vector2(x, y);
        animsCurrent = Vector2.Lerp(animsCurrent, animBlend, Time.deltaTime * 5);

        leftArmAim.solver.SetIKPositionWeight(Mathf.Lerp(leftArmAim.solver.GetIKPositionWeight(), (int)attackState == 1 ? 1 : 0, Time.deltaTime * 5));

        anims.SetFloat("MotionX", animsCurrent.x);
        anims.SetFloat("MotionY", animsCurrent.y);
        anims.SetInteger("AttackState", (int)attackState);

        if (attackState == AttackState.Hand)
        {
            if (!shooting)
            {
                gunCharge.Play();
                shooting = true;
            }
            else if (!gunCharge.isPlaying && shootTimer >= 0.1f)
            {
                Vector3 dir = target.position - shootPos.position;
                Instantiate(bullet, shootPos.position, shootPos.rotation).GetComponent<Bullet>().Shoot(dir.normalized);
                shootTimer = 0;
            }
            shootTimer += Time.deltaTime;
        }
        else
            shooting = false;
        if(attackState == AttackState.Chainsaw)
        {
            Collider[] cols = Physics.OverlapBox(chainsawCenter.position, new Vector3(0.875f, 2.5f, 0.5f), chainsawCenter.rotation);
            foreach (Collider col in cols)
            {
                if (col.TryGetComponent(out IPlayer player))
                {
                    player.TakeDamage(Time.deltaTime * 5);
                }
            }
        }
        laser.SetActive(attackState == AttackState.Laser);
    }
    private void LateUpdate()
    {
        if(target != null)
        {
            for (int i = 0; i < spineBones.Length; i++)
            {
                Quaternion spineLookAt = Quaternion.LookRotation(target.position - spineBones[i].position, Vector3.up);
                spineBones[i].rotation = Quaternion.Slerp(prevSpineRots[i], spineLookAt, Time.deltaTime * 10);
                prevSpineRots[i] = spineBones[i].rotation;
            }
        }
        chestForward = Vector3.ProjectOnPlane(spineBones[0].forward, Vector3.up).normalized;
    }
    void Turn(float turnVal)
    {
        timer = 1;
        x = Mathf.Clamp(turnVal / Mathf.Abs(turnVal), -1, 1);
    }
    void GoToTarget()
    {
        agent.SetDestination(target.position);
    }
    public void AddAgent()
    {
        agent.enabled = true;
        target = GameObject.Find("Player(Clone)").transform;
    }
    void CalculateChainSaw()
    {
        chainsawDistCheck.position = new Vector3(transform.position.x, target.transform.position.y, transform.position.z);
        chainsawDistCheck.position = new Vector3(chainsawDistCheck.position.x, Mathf.Clamp(chainsawDistCheck.position.y, transform.position.y, transform.position.y + 15), chainsawDistCheck.position.z);
        float playerDist = Vector3.Distance(chainsawDistCheck.position, target.position);

        float pDistLerp = playerDist < 10 ? 1 : 0;

        if (pDistLerp == 1)
            attackState = AttackState.Chainsaw;
        else if (attackState == AttackState.Chainsaw)
            attackState = AttackState.None;

        rightArmAim.solver.SetIKPositionWeight(Mathf.Lerp(rightArmAim.solver.GetIKPositionWeight(), pDistLerp, Time.deltaTime * 2));
    }
    IEnumerator ChooseAttackMode()
    {
        while (true)
        {
            if(attackState != AttackState.Chainsaw)
                attackState = (AttackState)rand.Next(0, 3);
            yield return new WaitForSeconds(5);
        }
    }
}