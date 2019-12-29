using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
	public string skillCode;
	public string skillName;
    public Sprite icon;
	public SkillType type;

    public float castingDelay;
    public float startupTime;
    public float recoveryTime;

    public int attackPercentage;
    protected int damage;
    public int damageCount;
    public float damageInterval;

    public float cooldown;
    protected float lastUsedTime;

    protected Actor owner;
    protected Mesh rangeMesh;

    protected Status status;

    public string tooltip;
    public bool isActivatedInPlayerPosition;
	public Material rangeMaterial;

    public void StartSkill(Actor user, Vector3 position, float yRotationEuler)
    {
		Skill actionSkill = ObjectPool.GetInstance().GetObject(gameObject).GetComponent<Skill>();
		actionSkill.gameObject.SetActive(true);
        actionSkill.Init(user, position);
        actionSkill.Action(yRotationEuler);
    }

    public abstract void Action(float yRotationEuler);
    public abstract void MakeTargetRangeMesh();
    public abstract Vector3 GetPosition(Vector2 stickMove, float maxMoveLength = 0);
    public abstract Quaternion GetRotation(Vector2 stickMove);
	public abstract bool IsTargetInRange(Actor target, Vector3 origin);

	public void Init(Actor user, Vector3 position)
    {
        transform.position = position;
        owner = user;
        status = user.GetFinalStatus();
        damage = status.attackDamage;
    }

    public Mesh GetTargetRangeMesh()
    {
        return rangeMesh;
    }

    public string GetTooltip()
    {
        string result = tooltip;
        result = result.Replace("[[DAMAGE]]", attackPercentage.ToString() + "%");
        result = result.Replace("[[COUNT]]", damageCount.ToString());
        result = result.Replace("[[INTERVAL]]", damageInterval.ToString());
        result = result.Replace("[[TIME]]", (damageInterval * damageCount).ToString());
        result = result.Replace("\\n", "\n");
        return result;
    }
}
