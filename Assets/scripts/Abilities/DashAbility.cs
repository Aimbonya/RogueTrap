using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class DashAbility : Ability
{
    public override void Activate(GameObject p)
    {
        CONTROLplayer controlPlayer = p.GetComponent<CONTROLplayer>();
        if (controlPlayer != null)
        {
            controlPlayer.StartCoroutine(Dash(controlPlayer));
        }
    }

    private IEnumerator Dash(CONTROLplayer player)
    {
        GameController.setInvicible();

        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();   
        Vector2 dashDirection = rb.velocity.normalized;

        if (dashDirection == Vector2.zero)
        {            
            float horiz = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            dashDirection = new Vector2(horiz, vertical).normalized;
            Debug.Log($"Dash direction from input: {dashDirection}");
        }

        float dashEndTime = Time.time + activeTime;

        while (Time.time < dashEndTime)
        {
            rb.MovePosition(rb.position + dashDirection * 25 * Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate();
        }

        GameController.resetInvicible();
    }
}
