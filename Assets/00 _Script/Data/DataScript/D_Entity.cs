using UnityEngine;

[CreateAssetMenu(fileName = "newEntityData", menuName = "Data/Entity Data/Base Entity Data")]
public class D_Entity : ScriptableObject
{
    //Handle Collision
    public LayerMask whatIsPlayer;

    [Tooltip("撞到會不會扣血")]
    public bool collideDamage = true;

    public float gravityScale = 8f;
    public Sound damagedSFX;
}
