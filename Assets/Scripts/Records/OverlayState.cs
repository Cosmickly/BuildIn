namespace Records
{
    public record OverlayState
    {
        /// <summary>
        ///     The Overlay Id for this specific Overlay. Unlike Bricks, overlays do not shift.
        /// </summary>
        public int Id;

        /// <summary>
        ///     Whether this Overlay is currently being focused.
        /// </summary>
        public bool Focused;
    }
}