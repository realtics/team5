using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Actor : MonoBehaviour
{
    protected Rigidbody mRigidbody;
    protected CapsuleCollider mCollider;

    public Team team;
    public Status status;
    protected Status finalStatus;
    protected int currentHp;

    public string SpriteName;
    public SpriteIndex standingSpriteIndex;
    public float standingSpriteInterval;
    public SpriteIndex attackSpriteIndex;
    public float attackSpriteInterval;
    public SpriteIndex moveSpriteIndex;
    public float moveSpriteInterval;
    public SpriteIndex deathSpriteIndex;
    public float deathSpriteInterval;
    protected float spriteInterval;

	public SpriteAtlas atlas;
    List<Sprite> sprites;
    public SpriteRenderer spriteRenderer;
    public int spriteDirectionCount;
    protected int currentSpriteIndex;
    float prevSpriteTime;

    Vector3 velocity;
    protected float characterDirectionAngle;
    Vector3 scale;

    GameObject canvas;
    public DamageText damageText;
    public HPBar hpBar;

    public State state;

    public Skill[] skillArray;
    protected float[] lastSkillActionTime;

	protected virtual void Awake()
	{
		lastSkillActionTime = new float[skillArray.Length];
	}

	// Start is called before the first frame update
	protected virtual void Start()
    {
        prevSpriteTime = Time.time;
        canvas = BattleManager.GetInstance().worldCanvas;
        finalStatus = status;

        hpBar = Instantiate(hpBar);
        hpBar.transform.SetParent(canvas.transform);

        mRigidbody = GetComponent<Rigidbody>();
        mCollider = GetComponent<CapsuleCollider>();
        scale = transform.localScale;
        currentSpriteIndex = 0;

        sprites = new List<Sprite>();
        int i = 0;
        while (true)
        {
            Sprite sprite = atlas.GetSprite(SpriteName + "" + i++);
            if (sprite == null)
                break;
            sprites.Add(sprite);
        }
        
        Alive();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (state == State.Dead)
        {
            spriteInterval = deathSpriteInterval;
            SpriteUpdate(deathSpriteIndex, true, true);
            if (currentSpriteIndex >= (deathSpriteIndex.end - deathSpriteIndex.start))
            {
                gameObject.SetActive(false);
            }
        }
        else if (state == State.Attack)
        {
            spriteInterval = attackSpriteInterval;
            SpriteUpdate(attackSpriteIndex, false, false);
        }
        else if (velocity.sqrMagnitude != 0)
        {
            spriteInterval = moveSpriteInterval;
            SpriteUpdate(moveSpriteIndex, true, false);
        }
        else
        {
            spriteInterval = standingSpriteInterval;
            SpriteUpdate(standingSpriteIndex, true, false);
        }

        if (Time.time > prevSpriteTime + spriteInterval)
        {
            currentSpriteIndex++;
            prevSpriteTime = Time.time;
        }
    }

    protected virtual void FixedUpdate()
    {
        if (velocity.sqrMagnitude > 0)
        {
            mRigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
			mRigidbody.MovePosition(mRigidbody.position + velocity * Time.fixedDeltaTime);
		}
        else
        {
            mRigidbody.constraints = mRigidbody.constraints | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        }

        if (hpBar != null)
        {
            Vector3 hpBarPosition = transform.position;
            hpBarPosition.y += mCollider.height;
            hpBar.transform.position = Camera.main.WorldToScreenPoint(hpBarPosition);
        }
    }

    public void Move(Vector3 direction)
    {
        if (state == State.Idle)
            velocity = direction.normalized * status.moveSpeed;
    }

    public void Stop()
    {
        velocity = new Vector3(0, 0, 0);
    }

    void SpriteUpdate(SpriteIndex index, bool isLoop, bool isDirectionFixed)
    {
        int spriteLength = (index.end - index.start + 1);
        if (!isDirectionFixed)
            spriteLength /= spriteDirectionCount;

        if (currentSpriteIndex >= spriteLength)
            currentSpriteIndex = 0;

        int spriteIndex = index.start + GetDirectionIndex() * spriteLength + currentSpriteIndex;
        spriteRenderer.sprite = sprites[spriteIndex];
    }

    int GetDirectionIndex()
    {
        if (state == State.Dead)
        {
            transform.localScale = new Vector3(scale.x, scale.y, scale.z);
            return Global.downFrontIndex;
        }

        if (velocity.sqrMagnitude != 0)
            characterDirectionAngle = Mathf.Atan2(-velocity.z, velocity.x);

        float angleBasedZAxis = Global.ConvertIn2PI(characterDirectionAngle + Mathf.PI / 2, -Mathf.PI);

        if (angleBasedZAxis > 0)
        {
            transform.localScale = new Vector3(-scale.x, scale.y, scale.z);
        }
        else
        {
            transform.localScale = new Vector3(scale.x, scale.y, scale.z);
        }

		if (angleBasedZAxis < -Mathf.PI * 7 / 8 || angleBasedZAxis > Mathf.PI * 7 / 8)
        {
            return Global.downIndex;
        }
        else if (angleBasedZAxis < -Mathf.PI * 5 / 8 || angleBasedZAxis > Mathf.PI * 5 / 8)
        {
            return Global.downFrontIndex;
        }
        else if (angleBasedZAxis < -Mathf.PI * 3 / 8 || angleBasedZAxis > Mathf.PI * 3 / 8)
        {
            return Global.frontIndex;
        }
        else if (angleBasedZAxis < -Mathf.PI * 1 / 8 || angleBasedZAxis > Mathf.PI * 1 / 8)
        {
            return Global.upFrontIndex;
        }
        else
        {
            return Global.upIndex;
        }
    }

    public virtual void SetHp(int _hp)
    {
        currentHp = _hp;
		if (currentHp < 0)
			currentHp = 0;
        hpBar.SetHp(currentHp);
    }

    public void TakeDamage(int damage)
    {
        if (state == State.Dead)
            return;

        DamageText damageTextObject = Instantiate(damageText, transform.position, Quaternion.identity);
        damageTextObject.Init(transform.position, damage);
        damageTextObject.transform.SetParent(canvas.transform);

        SetHp(currentHp - damage);

        if (currentHp > 0)
        {
            Color color = spriteRenderer.material.GetColor("_Color");
            color.a = 0.5f;
            spriteRenderer.material.SetColor("_Color", color);

            StartCoroutine(TakeDamageCoroutine());
        }
        else
        {
            Death();
        }
    }

    public virtual void Alive()
    {
        gameObject.SetActive(true);

        hpBar.gameObject.SetActive(true);
        currentHp = status.hp;
        hpBar.SetMaxHp(status.hp);
        hpBar.SetHp(currentHp);

        mCollider.enabled = true;

        characterDirectionAngle = Mathf.Atan2(1, -1);
        state = State.Idle;
        currentSpriteIndex = 0;

		BattleManager.GetInstance().AddActorOnManager(this);
    }

    protected virtual void Death()
    {
        Stop();
        hpBar.gameObject.SetActive(false);
        currentSpriteIndex = 0;
        state = State.Dead;
        mCollider.enabled = false;
		BattleManager.GetInstance().DeleteActorFromManager(this);
	}

    IEnumerator TakeDamageCoroutine()
    {
        yield return new WaitForSeconds(0.1f);

        Color color = spriteRenderer.material.GetColor("_Color");
        color.a = 1f;
        spriteRenderer.material.SetColor("_Color", color);
    }

    public void AttackProcess(int index, Vector3 targetPosition, float yRotationEuler)
    {
        if (state == State.Idle)
        {
            state = State.Attack;
            Stop();
            currentSpriteIndex = 0;

            characterDirectionAngle = Global.ConvertIn2PI(yRotationEuler * Mathf.Deg2Rad, -Mathf.PI);

            StartCoroutine(WaitCastingDelay(index, targetPosition, yRotationEuler));
            lastSkillActionTime[index] = Time.time;
        }
    }

    IEnumerator WaitCastingDelay(int index, Vector3 targetPosition, float yRotationEuler)
    {
        yield return new WaitForSeconds(skillArray[index].castingDelay);
        skillArray[index].StartSkill(this, targetPosition, yRotationEuler);
        yield return new WaitForSeconds(skillArray[index].recoveryTime);
        state = State.Idle;
    }

    private void OnDestroy()
    {
        if (hpBar != null)
            Destroy(hpBar.gameObject);
    }

    public float GetRemainSkillCooldown(int index)
    {
		return skillArray[index].cooldown - (Time.time - lastSkillActionTime[index]);
    }

    public Status GetFinalStatus()
    {
        return finalStatus;
    }

	public float GetCollisionRadius()
	{
		return mCollider.radius;
	}
}
