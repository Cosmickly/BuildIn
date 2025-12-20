namespace Records
{
    public record OverlayState
    {
        /// <summary>
        ///     The Overlay Id for this specific Overlay. Unlike Bricks, overlays do not shift.
        /// </summary>
        public int Id;

        /// <summary>
        ///     Whether or not this Overlay is currently being focused.
        /// </summary>
        public bool Focused;

        /// <summary>
        ///     Whether or not this Overlay is currently holding a <see cref="ProtoBrickView"/>
        /// </summary>
        public bool HasBrick;
    }
}