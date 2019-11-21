using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Type
{
    WEAPON, ARMOR, ACCESSORY, SUB, ETC
}

public class Item : MonoBehaviour
{
    public Sprite icon;
<<<<<<< Updated upstream
    public Type type;

    public Stat stat;
=======
    public Character player;
>>>>>>> Stashed changes

    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.GetInstance().player;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject == player.gameObject)
        {
            Debug.Log(this.gameObject.name);
            Destroy(this.gameObject);
        }
    }
}
