using Records;
using UnityEngine;

public abstract class BrickView : MonoBehaviour
{
    /// <summary>
    ///     Applies changes in the <see cref="BrickState"/>
    /// </summary>
    /// <param name="state"></param>
    public virtual void ApplyBrickState(BrickState state)
    {
    }
}