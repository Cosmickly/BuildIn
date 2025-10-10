using Extensions;
using UnityEngine;

namespace Records
{
    /// <summary>
    ///     The state of a <see cref="PlayingBrickView"/> or <see cref="ProtoBrickView"/>
    /// </summary>
    public record BrickState
    {
        public int Id;

        public bool Active;

        public Sprite Sprite;

        public BrickColorPalette.BrickColor BrickColor;
    }
}