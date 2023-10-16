using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpriteHandler : MonoBehaviour
{
    //sword art references
    [SerializeField] SpriteRenderer neutralSwordArt;
    [SerializeField] SpriteRenderer preAttackArt;
    [SerializeField] SpriteRenderer postAttackArt;
    [SerializeField] SpriteRenderer currentSwordArt;
    [SerializeField] SpriteRenderer dodgeSwordArt;
    [SerializeField] SpriteRenderer counterShine;


    //shield art references
    [SerializeField] SpriteRenderer neutralShieldArt;
    [SerializeField] SpriteRenderer shieldBlockArt;
    [SerializeField] SpriteRenderer dodgeShieldArt;
    [SerializeField] SpriteRenderer shieldBashArt;
    [SerializeField] SpriteRenderer currentShieldArt;

    private void Start()
    {
        currentSwordArt = neutralSwordArt;
        currentShieldArt = neutralShieldArt;
    }

    //Attack art
    public void NeutralSwordArt()
    {
        currentSwordArt.enabled = false;
        neutralSwordArt.enabled = true;
        currentSwordArt = neutralSwordArt;
    }

    public void PreAttackArt()
    {
        currentSwordArt.enabled = false;
        preAttackArt.enabled = true;
        currentSwordArt = preAttackArt;
    }

    public void PostAttackArt()
    {
        currentSwordArt.enabled = false;
        postAttackArt.enabled = true;
        currentSwordArt = postAttackArt;
    }

    public void DodgeSwordArt()
    {
        currentSwordArt.enabled = false;
        dodgeSwordArt.enabled = true;
        currentSwordArt = dodgeSwordArt;
    }

    public void CounterAttackShineArt()
    {
        if (!counterShine.enabled)
        {
            counterShine.enabled = true;
        }
        else
        {
            counterShine.enabled = false;
        }

    }

    //Block art
    public void NeutralShieldArt()
    {
        currentShieldArt.enabled = false;
        neutralShieldArt.enabled = true;
        currentShieldArt = neutralShieldArt;
    }

    public void BlockShieldArt()
    {
        currentShieldArt.enabled = false;
        shieldBlockArt.enabled = true;
        currentShieldArt = shieldBlockArt;
    }

    public void DodgeShieldArt()
    {
        currentShieldArt.enabled = false;
        dodgeShieldArt.enabled = true;
        currentShieldArt = dodgeShieldArt;
    }

    public void ShieldBashArt()
    {
        currentShieldArt.enabled = false;
        shieldBashArt.enabled = true;
        currentShieldArt = shieldBashArt;
    }
}
