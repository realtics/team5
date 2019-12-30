using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class test :MonoBehaviour 
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    [ExecuteInEditMode]
    void Update()
    {
        UnityEngine.Debug.Log("sdfsdf");
    }

    [ContextMenu("myMeny")]
    private void Testfn()
    {
        Debug.Log("1111111111");
    }
}
