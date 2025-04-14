using ChainedBehavior;
using UnityEngine;

namespace Game.Piece
{
    /// <summary>
    /// Implementação de peça do jogador.
    /// Responsável por criar seu renderer e calcular posições de movimento para pré-visualização.
    /// </summary>
    public class PlayerPiece : APiece
    {
        private ARendererPieceScriptableObject _rendererPieceScriptableObject;

        /// <summary>
        /// Configura os dados do renderer para a peça do jogador.
        /// </summary>
        /// <param name="rendererPiece">Renderer a ser utilizado.</param>
        public override void SetData(ARendererPieceScriptableObject rendererPiece)
        {
            _rendererPieceScriptableObject = rendererPiece;
        }

        /// <summary>
        /// Método Start que cria o renderer específico para a peça do jogador.
        /// </summary>
        private void Start()
        {
            if (_rendererPieceScriptableObject == null)
            {
                Debug.LogError("RendererPiece não definido para a PlayerPiece.");
                return;
            }
            _rendererPieceScriptableObject.CreatePlayerPieceRenderer(this);
        }

        /// <summary>
        /// Calcula e retorna a posição de destino para a peça,
        /// levando em conta os nós de movimento encadeados para pré-visualizar a movimentação.
        /// </summary>
        /// <returns>Posição no mundo para a movimentação da peça.</returns>
        public Vector3 PreviewPositionsToMove()
        {
            Vector3 position = transform.position;

            if (ChainedNode == null)
            {
                Debug.LogWarning("ChainedNode não configurado para PlayerPiece " + gameObject.name);
                return position;
            }

            position = WorldPositionMove(ref position, ChainedNode.Root);
            return position;
        }

        /// <summary>
        /// Método recursivo que percorre os nós de movimento e acumula os deslocamentos.
        /// </summary>
        /// <param name="position">Posição atual acumulada.</param>
        /// <param name="node">Nó atual da cadeia.</param>
        /// <returns>Posição final após aplicar os movimentos definidos nos nós.</returns>
        private Vector3 WorldPositionMove(ref Vector3 position, Node node)
        {
            if (node == null)
                return position;

            if (node is MovePieceBehavior moveBehavior)
            {
                position += moveBehavior.GetPositionToMove();
            }

            if (node.ChildNode != null)
                return WorldPositionMove(ref position, node.ChildNode);

            return position;
        }
    }
}
