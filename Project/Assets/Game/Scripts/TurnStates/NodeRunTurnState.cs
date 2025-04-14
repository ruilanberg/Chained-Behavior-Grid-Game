using Game.Piece;
using StateMachine;
using UnityEngine;

namespace Game.TurnState
{
    /// <summary>
    /// Estado de turno que executa o nó associado a uma peça e, após a execução,
    /// adiciona o estado de turno do jogador ao state machine.
    /// </summary>
    public class NodeRunTurnState : State
    {
        private APiece _piece;
        private TurnMachine _turnMachine;
        private InteractionGrid _interactionGrid;
        private BoardManager _boardManager;

        /// <summary>
        /// Construtor que inicializa o estado com as dependências necessárias.
        /// </summary>
        /// <param name="interactionGrid">Referência ao gerenciador de interação com o grid.</param>
        /// <param name="boardManager">Gerenciador do tabuleiro.</param>
        /// <param name="piece">Peça para a qual o nó será executado.</param>
        /// <param name="turnMachine">Máquina de estados de turno.</param>
        public NodeRunTurnState(InteractionGrid interactionGrid, BoardManager boardManager, APiece piece, TurnMachine turnMachine)
        {
            _interactionGrid = interactionGrid;
            _boardManager = boardManager;
            _piece = piece;
            _turnMachine = turnMachine;
        }

        /// <summary>
        /// Executa o nó da peça de forma assíncrona e, ao completar, adiciona o estado do turno do jogador.
        /// Registra a peça na célula correspondente e finaliza o estado.
        /// </summary>
        public override async void Enter()
        {
            // Validação mínima para evitar exceções em tempo de execução.
            if (_piece == null || _piece.ChainedNode == null || _piece.ChainedNode.Root == null)
            {
                Debug.LogError("Peça ou seus nós não estão configurados corretamente.");
                EndEvent();
                return;
            }

            // Executa o nó principal da peça de forma assíncrona.
            var result = await _piece.ChainedNode.Root.Run();

            // Adiciona o próximo estado de turno (turno do jogador) e atualiza a posição da peça.
            _turnMachine.AddEvent(new PlayerTurnState(_interactionGrid, _boardManager, _turnMachine));
            _boardManager.SetPieceInCell(ref _piece);
            EndEvent();
        }

        /// <summary>
        /// Método de saída do estado que reseta as referências no BoardManager.
        /// </summary>
        public override void Exit()
        {
            _boardManager.ResetReferences();
        }

        /// <summary>
        /// Atualiza o estado a cada frame. Neste estado não há lógica de atualização.
        /// </summary>
        /// <param name="deltaTime">Tempo decorrido desde a última atualização.</param>
        public override void Update(float deltaTime)
        {
            // Não há lógica de atualização neste estado.
        }
    }
}
