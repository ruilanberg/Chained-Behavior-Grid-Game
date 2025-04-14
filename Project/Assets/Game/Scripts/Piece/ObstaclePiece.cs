using UnityEngine;

namespace Game.Piece
{
    /// <summary>
    /// Implementação de peça de obstáculo.
    /// Responsável por criar seu renderer ao iniciar.
    /// </summary>
    public class ObstaclePiece : APiece
    {
        private ARendererPieceScriptableObject _rendererPieceScriptableObject;

        /// <summary>
        /// Configura os dados do renderer para a peça de obstáculo.
        /// </summary>
        /// <param name="rendererPiece">Renderer a ser utilizado.</param>
        public override void SetData(ARendererPieceScriptableObject rendererPiece)
        {
            _rendererPieceScriptableObject = rendererPiece;
        }

        /// <summary>
        /// Método Start que cria o renderer específico para a peça de obstáculo.
        /// </summary>
        private void Start()
        {
            if (_rendererPieceScriptableObject == null)
            {
                Debug.LogError("RendererPiece não definido para a ObstaclePiece.");
                return;
            }
            _rendererPieceScriptableObject.CreateObstaclePieceRenderer(this);
        }
    }
}
