using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{

    [SerializeField]
    Transform Player = null;

    [SerializeField]
    Vector3 CamPosition = new Vector3(0, 0, -10);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Player != null)
        {
            this.transform.position = Player.position + CamPosition;
        }

        
    }
}
