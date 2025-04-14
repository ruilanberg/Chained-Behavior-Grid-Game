using Game.Piece;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// ScriptableObject que contém as configurações do jogo, incluindo as peças do jogador, inimigos e obstáculos.
    /// </summary>
    [CreateAssetMenu(fileName = "GameSettings", menuName = "ScriptableObjects/GameSettings")]
    public class GameSettings : ScriptableObject
    {
        /// <summary>
        /// Array de peças do jogador.
        /// </summary>
        [field: SerializeField] public PlayerPiece[] PlayerPieces { get; private set; }

        /// <summary>
        /// Array de peças inimigas.
        /// </summary>
        [field: SerializeField] public EnemyPiece[] EnemyPieces { get; private set; }

        /// <summary>
        /// Array de peças de obstáculo.
        /// </summary>
        [field: SerializeField] public ObstaclePiece[] ObstaclePieces { get; private set; }
    }
}
