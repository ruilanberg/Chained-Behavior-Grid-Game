using StateMachine;

namespace Game.TurnState
{
    /// <summary>
    /// Estado inicial que realiza a configuração do turno, instanciando as peças no tabuleiro.
    /// Após a configuração, finaliza o estado para permitir o início do turno do jogador.
    /// </summary>
    public class SetupTurnState : State
    {
        private BoardManager _boardManager;

        /// <summary>
        /// Construtor que inicializa o estado de configuração de turno com o BoardManager.
        /// </summary>
        /// <param name="boardManager">Gerenciador do tabuleiro.</param>
        public SetupTurnState(BoardManager boardManager)
        {
            _boardManager = boardManager;
        }

        /// <summary>
        /// Método chamado ao entrar no estado de configuração.
        /// Instancia as peças no tabuleiro e finaliza o estado.
        /// </summary>
        public override void Enter()
        {
            _boardManager.SpawnPieces();
            EndEvent();
        }

        /// <summary>
        /// Método chamado ao sair do estado de configuração.
        /// Neste caso, não há operações específicas de saída.
        /// </summary>
        public override void Exit()
        {
            // Não há operações de saída necessárias neste estado.
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
