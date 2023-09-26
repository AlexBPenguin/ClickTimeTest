using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpriteHandler : MonoBehaviour
{

    public EnemyScriptableObject enemy;

    //Sprite
    public SpriteRenderer sprite;

    //sprite animation sizes
    [SerializeField] Vector3 spriteStartSize;
    [SerializeField] Vector3 spriteSmallestSize;
    [SerializeField] Vector3 spriteLargestSize;

    //multiplier to adjust spriteSamllestSize
    [SerializeField] float smallSpriteMultiplier;

    //multiplier to adjust pulse size
    [SerializeField] float pulseSize;

    //multipliers for sprite size return and shrink speed 
    [SerializeField] float returnSpeed;
    [SerializeField] float shrinkSpeed;
    [SerializeField] float growSpeed;

    //multipliers for sprite up speed and down speed
    [SerializeField] float upSpeed;
    [SerializeField] float downSpeed;
    [SerializeField] Vector3 startPos;

    //ground slam sprite reference
    [SerializeField] GameObject groundSlamSprite;

    //sets sprite to neutral sprite and sets spriteStartSize and spriteSmallestSize
    private void Start()
    {
        //set neutral transform position
        startPos = sprite.transform.position;

        //set neutral sprite
        sprite.sprite = enemy.neutralSprite;

        spriteStartSize = transform.localScale;
        spriteSmallestSize = transform.localScale / smallSpriteMultiplier;
    }

    //for later use when there are other sprites to swap to
    public void SpriteSwap(Sprite newSprite)
    {
        sprite.sprite = newSprite;
    }

    //Returns enemy sprite to original size when in update
    public void ReturnToOriginalSpriteSize()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, spriteStartSize, Time.deltaTime * returnSpeed);
    }

    //shrinks enemy sprite to the preset smallest size when in update
    public void ShrinkSpriteSize()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, spriteSmallestSize, Time.deltaTime * shrinkSpeed);
    }

    public void GrowSpriteSize()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, spriteLargestSize, Time.deltaTime * growSpeed);
    }

    //pulses sprite (used for enemy attacks)
    public void PulseSprite()
    {
        transform.localScale = spriteStartSize * pulseSize;
    }

    public IEnumerator GroundSlamSprite()
    {
        groundSlamSprite.SetActive(true);

        yield return new WaitForSeconds(0.2f);

        groundSlamSprite.SetActive(false);
    }

    public void MoveSpriteUp()
    {
        transform.Translate(Vector3.up * Time.deltaTime * upSpeed);
    }

    public void MoveSpriteDown()
    {
        //transform.position = Vector3.MoveTowards(transform.position, startPos, downSpeed);
        transform.position = startPos;
    }
}
