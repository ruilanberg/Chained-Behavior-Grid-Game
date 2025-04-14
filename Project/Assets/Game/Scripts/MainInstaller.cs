using Game.Piece;
using Game.TurnState;
using GridSystem;
using Reflex.Core;
using StateMachine;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// Instalador principal que gerencia a injeção de dependências do jogo.
    /// Configura o grid, as configurações do jogo, o board manager e o state machine.
    /// </summary>
    public class MainInstaller : MonoBehaviour, IInstaller
    {
        [SerializeField] private GridSettings _gridSettings;
        [SerializeField] private ARendererCellScriptableObject _rendererCell;
        [SerializeField] private ARendererPieceScriptableObject _rendererPiece;
        [Space]
        [SerializeField] private GameSettings _gameSettings;
        [Space]
        [SerializeField] private InteractionGrid _interactionGrid;
        [SerializeField] private BoardManager _boardManager;
        [Space]
        [SerializeField] private TurnMachine _stateMachine;

        /// <summary>
        /// Registra as dependências necessárias no contêiner.
        /// </summary>
        /// <param name="builder">O construtor de contêiner utilizado para adicionar as dependências.</param>
        public void InstallBindings(ContainerBuilder builder)
        {
            if (_gridSettings == null)
            {
                Debug.LogError("GridSettings não está definido no MainInstaller.");
                return;
            }
            if (_rendererCell == null)
            {
                Debug.LogError("RendererCell não está definido no MainInstaller.");
                return;
            }
            if (_gameSettings == null)
            {
                Debug.LogError("GameSettings não está definido no MainInstaller.");
                return;
            }
            if (_boardManager == null)
            {
                Debug.LogError("BoardManager não está definido no MainInstaller.");
                return;
            }
            if (_stateMachine == null)
            {
                Debug.LogError("TurnMachine não está definido no MainInstaller.");
                return;
            }

            builder.AddSingleton(_gridSettings);
            InstallGrid(builder);
            builder.AddSingleton(_gameSettings);

            _boardManager.SetRenderer(_rendererPiece);
            builder.AddSingleton(_boardManager);

            builder.AddSingleton(_stateMachine);
        }

        /// <summary>
        /// Inicia o state machine.
        /// </summary>
        private void Start()
        {
            _stateMachine.AddEvent(new SetupTurnState(_boardManager));
            _stateMachine.AddEvent(new PlayerTurnState(_interactionGrid, _boardManager, _stateMachine));

            _stateMachine.Init();
        }

        /// <summary>
        /// Cria a instância do grid e a registra no contêiner.
        /// </summary>
        /// <param name="builder">O construtor de contêiner utilizado para adicionar as dependências.</param>
        private void InstallGrid(ContainerBuilder builder)
        {
            GameObject gridRoot = new GameObject("GridRoot");
            Grid<APiece> grid = new Grid<APiece>(gridRoot.transform, _gridSettings, _rendererCell);
            builder.AddSingleton(grid);
        }
    }
}
