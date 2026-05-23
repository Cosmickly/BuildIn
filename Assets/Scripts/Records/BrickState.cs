using Extensions;
using UnityEngine;

namespace Records
{
    /// <summary>
    ///     The state of a <see cref="PlayingBrickView"/> or <see cref="ProtoBrickView"/>
    /// </summary>
    public record BrickState
    {
        /// <summary>
        ///     Unique identifier for brick. TODO not set
        /// </summary>
        public int Id;

        /// <summary>
        ///     Controls Brick visibility and tangibility
        /// </summary>
        public bool Active;

        public Sprite Sprite;

        public BrickColorPalette.BrickColor BrickColor;
    }
}