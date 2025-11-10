using UnityEngine;

public class SurvivorAnimation : MonoBehaviour
{
private Animator animator;
    private SpriteRenderer spriteRenderer;
    
    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    public void SetWalking(bool walking)
    {
        animator.SetBool("IsWalking", walking);
    }
    
    public void SetWeaponType(int weaponType)
    {
        animator.SetInteger("WeaponType", weaponType);
    }
    
    // Para voltear sprite si es necesario
    public void FlipSprite(bool flip)
    {
        spriteRenderer.flipX = flip;
    }
}
