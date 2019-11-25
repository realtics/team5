using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Actor
{
    public Status status;
    Status statusWithItem;

<<<<<<< HEAD
[System.Serializable]
public enum Team
{
    Player, Enemy
}

public enum State
{
    Idle, Attack, Dead
}

public class Character : MonoBehaviour
{
    protected Rigidbody mRigidbody;
    protected CapsuleCollider mCollider;

    public Team team;
    public float speed;
    public int maxHp;

    public string SpriteName;
    public SpriteIndex standingSpriteIndex;
    public float standingSpriteInterval;
    public SpriteIndex attackSpriteIndex;
    public float attackSpriteInterval;
    public SpriteIndex moveSpriteIndex;
    public float moveSpriteInterval;
    public SpriteIndex deathSpriteIndex;
    public float deathSpriteInterval;
    float spriteInterval;
    List<Sprite> sprites;
    public SpriteRenderer spriteRenderer;
    public int spriteDirectionCount;
    int currentSpriteIndex;
    float prevSpriteTime;

    Vector3 velocity;
    protected float characterDirectionAngle;
    Vector3 scale;

    int currentHp;

    GameObject canvas;
    public DamageText damageText;
    public HPBar hpBar;

    protected State state;

    

    // Start is called before the first frame update
    protected virtual void Start()
=======
    protected override void Start()
>>>>>>> master
    {
        base.Start();

        for (int i = 0; i < skillArray.Length; i++)
        {
            skillArray[i].MakeTargetRangeMesh();
            lastSkillActionTime[i] = Time.time - skillArray[i].cooldown;
        }

        EquipItem();
    }

    protected override void Update()
    {
        base.Update();
    }

    public Skill GetSkill(int index)
    {
        if (index >= skillArray.Length)
            return null;
        else
            return skillArray[index];
    }

    public void EquipItem()
    {
        statusWithItem = status;
        statusWithItem.attackDamage += GameManager.GetInstance().itemStatus.attackDamage;
        statusWithItem.armor += GameManager.GetInstance().itemStatus.armor;
        statusWithItem.hp += GameManager.GetInstance().itemStatus.hp;
        statusWithItem.hpRecovery += GameManager.GetInstance().itemStatus.hpRecovery;
        statusWithItem.moveSpeed += GameManager.GetInstance().itemStatus.moveSpeed;
    }
}