using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletLogic : MonoBehaviour
{
    [SerializeField]
    private float m_speed;



    private Vector3 dir;



    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += (dir * m_speed * Time.deltaTime);
    }

    public void setDir(Vector3 thedir)
    {
        dir = thedir;
    }

    public void setSpeed(float spd) {
        m_speed = spd;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") Destroy(gameObject);
    }


}
