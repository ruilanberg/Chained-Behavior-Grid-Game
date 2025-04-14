namespace Game.Piece
{
    /// <summary>
    /// Enumera os possíveis direcionamentos na grade.
    /// </summary>
    public enum DirectionGrid
    {
        Left,
        Right,
        Up,
        Down,
        UpLeft,
        UpRight,
        DownLeft,
        DownRight
    }

    /// <summary>
    /// Interface que define o comportamento de movimento em uma grade.
    /// </summary>
    public interface IMoveBehaviour
    {
        /// <summary>
        /// Retorna a direção do movimento.
        /// </summary>
        /// <returns>A direção representada pelo enum <see cref="DirectionGrid"/>.</returns>
        DirectionGrid GetDirection();
    }
}
