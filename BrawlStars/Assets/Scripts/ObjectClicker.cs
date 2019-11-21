using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectClicker : MonoBehaviour
{
    new Camera camera;
    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            //일괄 습득
            //RaycastHit[] hits;
            //hits = Physics.RaycastAll(ray, 100.0f);

            //for (int i = 0; i < hits.Length; i++)
            //{
            //    if (hits[i].collider.gameObject.tag == "Item")
            //    {
            //        Debug.Log(hits[i].collider.gameObject.name);
            //        Destroy(hits[i].collider.gameObject);
            //    }
            //}

            //1개씩
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.tag == "Item")
                {
                    Debug.Log(hit.collider.gameObject.name);
                    Destroy(hit.collider.gameObject);
                }
            }
        }
    }
}
