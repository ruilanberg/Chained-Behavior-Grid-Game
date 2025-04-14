using UnityEngine;
using ChainedBehavior;
using Reflex.Attributes;
using DG.Tweening;
using GridSystem;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;

namespace Game.Piece
{
    /// <summary>
    /// Nó que gerencia o movimento de uma peça na grade.
    /// Executa a animação do movimento e trata colisões, como a destruição de peças ou reversão do movimento.
    /// </summary>
    public class MovePieceBehavior : Node, IMoveBehaviour
    {
        [Inject] private BoardManager BoardManager { get; set; }
        [Inject] private Grid<APiece> Grid { get; set; }

        [SerializeField] private DirectionGrid _direction;

        private APiece _piece;

        /// <summary>
        /// Inicializa o movimento definindo a peça selecionada.
        /// </summary>
        protected override void OnStart()
        {
            if (BoardManager == null || BoardManager.SelectedCell == null)
            {
                Debug.LogError("BoardManager ou célula selecionada não estão configurados.");
                return;
            }

            _piece = BoardManager.SelectedCell.Value;

            if (_piece == null)
            {
                Debug.LogError("Nenhuma peça encontrada na célula selecionada.");
            }
        }

        /// <summary>
        /// Método de parada do nó.
        /// Pode ser utilizado para operações de limpeza, se necessário.
        /// </summary>
        protected override void OnStop()
        {
            // Operações de limpeza, se necessário.
        }

        /// <summary>
        /// Executa a animação de movimento da peça e processa colisões.
        /// Caso a peça colida com outra peça inimiga ou do jogador, ela é destruída.
        /// Se houver colisão com um obstáculo, o movimento é revertido.
        /// </summary>
        /// <returns>Retorna o resultado do estado (Success ou Failure).</returns>
        protected override async UniTask<StateResult> OnUpdate()
        {
            if (_piece == null)
            {
                Debug.LogError("Peça não definida para o movimento.");
                return StateResult.Failure;
            }

            Vector3 originPos = _piece.transform.position;
            Vector3 targetPos = originPos + GetPositionToMove();

            // Realiza a animação do movimento até a posição-alvo
            await MovePieceTo(targetPos);

            // Processa colisões e retorna o resultado final
            return await ProcessCollision(originPos);
        }

        /// <summary>
        /// Realiza a animação de movimento da peça até a posição definida.
        /// </summary>
        /// <param name="targetPos">Posição alvo para onde mover a peça.</param>
        private async UniTask MovePieceTo(Vector3 targetPos)
        {
            // Utiliza DOTween para animar o movimento durante 1 segundo
            await _piece.transform.DOMove(targetPos, 1f);
        }

        /// <summary>
        /// Processa a célula onde a peça se encontra após o movimento e trata colisões.
        /// Se colidir com uma peça inimiga ou do jogador, a peça colidida é destruída;
        /// se colidir com um obstáculo, o movimento é revertido.
        /// </summary>
        /// <param name="originPos">Posição original da peça antes do movimento.</param>
        /// <returns>O resultado do estado após o processamento da colisão.</returns>
        private async UniTask<StateResult> ProcessCollision(Vector3 originPos)
        {
            var currentCell = Grid.GetCell(_piece.transform.position);
            if (currentCell == null)
            {
                Debug.LogWarning("Nenhuma célula encontrada após o movimento.");
                return StateResult.Failure;
            }

            if (currentCell.Value != null)
            {
                // Se a célula contém uma peça inimiga ou do jogador, destrói a peça
                if (currentCell.Value is EnemyPiece || currentCell.Value is PlayerPiece)
                {
                    Destroy(currentCell.Value.gameObject);
                    currentCell.Value = null;
                    return StateResult.Success;
                }
                // Se a célula contém um obstáculo, reverte o movimento
                else if (currentCell.Value is ObstaclePiece)
                {
                    await _piece.transform.DOMove(originPos, 1f);
                    return StateResult.Failure;
                }
            }
            return StateResult.Success;
        }

        /// <summary>
        /// Retorna a direção configurada para o movimento.
        /// </summary>
        /// <returns>A direção do movimento definida pelo enum DirectionGrid.</returns>
        public DirectionGrid GetDirection()
        {
            return _direction;
        }

        /// <summary>
        /// Calcula e retorna o vetor de movimento com base na direção definida.
        /// </summary>
        /// <returns>Vetor de deslocamento representando o movimento.</returns>
        public Vector3 GetPositionToMove()
        {
            switch (_direction)
            {
                case DirectionGrid.Left: return new Vector3(-1, 0, 0);
                case DirectionGrid.Right: return new Vector3(1, 0, 0);
                case DirectionGrid.Up: return new Vector3(0, 1, 0);
                case DirectionGrid.Down: return new Vector3(0, -1, 0);
                case DirectionGrid.UpLeft: return new Vector3(-1, 1, 0);
                case DirectionGrid.UpRight: return new Vector3(1, 1, 0);
                case DirectionGrid.DownLeft: return new Vector3(-1, -1, 0);
                case DirectionGrid.DownRight: return new Vector3(1, -1, 0);
                default: return Vector3.zero;
            }
        }
    }
}
