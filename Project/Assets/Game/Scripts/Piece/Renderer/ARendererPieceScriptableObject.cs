using UnityEngine;

namespace Game.Piece
{
    /// <summary>
    /// Classe base abstrata para ScriptableObjects que gerenciam a criação de renderizadores para peças.
    /// Define métodos abstratos para criação de renderizadores específicos para peças do jogador, inimigas e de obstáculo.
    /// </summary>
    public abstract class ARendererPieceScriptableObject : ScriptableObject
    {
        /// <summary>
        /// Cria e retorna o renderizador para uma peça do jogador.
        /// </summary>
        /// <param name="player">Instância da peça do jogador.</param>
        /// <returns>Renderizador associado à peça do jogador.</returns>
        public abstract Renderer CreatePlayerPieceRenderer(APiece player);

        /// <summary>
        /// Cria e retorna o renderizador para uma peça inimiga.
        /// </summary>
        /// <param name="enemy">Instância da peça inimiga.</param>
        /// <returns>Renderizador associado à peça inimiga.</returns>
        public abstract Renderer CreateEnemyPieceRenderer(APiece enemy);

        /// <summary>
        /// Cria e retorna o renderizador para uma peça de obstáculo.
        /// </summary>
        /// <param name="obstacle">Instância da peça de obstáculo.</param>
        /// <returns>Renderizador associado à peça de obstáculo.</returns>
        public abstract Renderer CreateObstaclePieceRenderer(APiece obstacle);
    }
}
