using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Health : NetworkBehaviour {

    public const int maxHealth = 100;
    public RectTransform healthBar;

    [SyncVar(hook = "OnChangeHealth")]
    public int currentHealth = maxHealth;

	public void TakeDamage(int amount)
    {
        if(!isServer)//Solo quiero que el server actualice las vidas de los players y despues los sincronice.
        {
            return;
        }

        currentHealth -= amount;

        if(currentHealth <= 0)
        {
            currentHealth = maxHealth;

            RpcRespawn();
        }
        //El hook se llama al actualizarse incluso en el server, asi que esta linea está de más
        //healthBar.sizeDelta = new Vector2(currentHealth, healthBar.sizeDelta.y);
    }

    void OnChangeHealth(int health)
    {
        healthBar.sizeDelta = new Vector2(health, healthBar.sizeDelta.y);
    }

    [ClientRpc]
    void RpcRespawn()
    {
        if(isLocalPlayer)//solo la instancia original tiene autoridad de modificar su transform.
        {
            transform.position = Vector3.zero;
        }
    }
}
