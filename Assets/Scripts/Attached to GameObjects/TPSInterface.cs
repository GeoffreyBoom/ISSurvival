using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.ThirdPerson;

public class TPSInterface : MonoBehaviour {
    public Slider healthBar;
    public Slider staminaBar;
    public ThirdPersonCharacter player;

    public int currentHealth;
    int currentStamina, amount = 2;
    float nextMove, moveRate = 0.1f;

	// Use this for initialization
	void Start () {
        currentHealth = 100;
        currentStamina = 100;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey("d"))
        {
            setHealthUI();
        }

        if(Input.GetKey(KeyCode.LeftShift) && staminaBar.value > 0 && Time.time > nextMove)
        {
            nextMove = Time.time + moveRate;
            setStaminaUI();
        }

    }

    private void setHealthUI()
    {
        currentHealth -= amount;

        healthBar.value = currentHealth;
    }

    private void setStaminaUI()
    {
        currentStamina -= amount;

        staminaBar.value = currentStamina;
    }
}
