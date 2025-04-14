using Game.Piece;
using StateMachine;

namespace Game.TurnState
{
    /// <summary>
    /// Estado de turno onde o jogador está ativo e pode selecionar a movimentação de suas peças.
    /// Inicializa os controles de interação e define o comportamento ao selecionar uma peça.
    /// </summary>
    public class PlayerTurnState : State
    {
        private InteractionGrid _interactionGrid;
        private BoardManager _boardManager;
        private TurnMachine _turnMachine;

        /// <summary>
        /// Construtor que inicializa o estado do turno do jogador com as dependências necessárias.
        /// </summary>
        /// <param name="interactionGrid">Gerenciador da interação com o grid.</param>
        /// <param name="boardManager">Gerenciador do tabuleiro.</param>
        /// <param name="turnMachine">Máquina de estados de turno.</param>
        public PlayerTurnState(InteractionGrid interactionGrid, BoardManager boardManager, TurnMachine turnMachine)
        {
            _interactionGrid = interactionGrid;
            _boardManager = boardManager;
            _turnMachine = turnMachine;
        }

        /// <summary>
        /// Método chamado ao entrar no estado do turno do jogador.
        /// Inicializa as ações de input e define o delegate para selecionar a movimentação.
        /// </summary>
        public override void Enter()
        {
            _interactionGrid.Init();
            _boardManager.OnRunBehaviourPiece = SelectMovement;
        }

        /// <summary>
        /// Executa a seleção de movimentação para a peça.
        /// Adiciona o estado NodeRunTurnState e finaliza o estado atual.
        /// </summary>
        /// <param name="piece">Peça selecionada pelo jogador.</param>
        public void SelectMovement(APiece piece)
        {
            _turnMachine.AddEvent(new NodeRunTurnState(_interactionGrid, _boardManager, piece, _turnMachine));
            EndEvent();
        }

        /// <summary>
        /// Método chamado ao sair do estado do turno do jogador.
        /// Interrompe as ações de input e reseta a exibição do tabuleiro.
        /// </summary>
        public override void Exit()
        {
            _interactionGrid.Stop();
            _boardManager.ResetViewBoard();
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
