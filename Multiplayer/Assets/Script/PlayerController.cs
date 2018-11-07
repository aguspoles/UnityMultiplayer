using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour
{
    public GameObject bulletprefab;
    public Transform bulletSpawn;
    public float speed;

    void Update()
    {
        if(!isLocalPlayer)
        {
            return;
        }
        var x = Input.GetAxisRaw("Horizontal");
        var y = Input.GetAxisRaw("Vertical");

        transform.Translate(Vector3.forward * y * Time.deltaTime * speed);
        transform.Rotate(Vector3.up * x);

        if (Input.GetKeyDown(KeyCode.Space))
            CmdFire();
    }

    public override void OnStartLocalPlayer()
    {
        GetComponent<MeshRenderer>().material.color = Color.blue;
    }

    [Command]
    private void CmdFire()
    {
        GameObject bullet = Instantiate(bulletprefab, bulletSpawn.position, bulletSpawn.rotation);
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 6;
        NetworkServer.Spawn(bullet);

        Destroy(bullet, 2f);
    }
}
