using Game.Piece;
using GridSystem;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    /// <summary>
    /// Gerencia a interação do jogador com a grade.
    /// Lida com os eventos de clique e movimento do mouse para selecionar e destacar células.
    /// </summary>
    public class InteractionGrid : MonoBehaviour
    {
        private InputAction _clickInputAction;
        private InputAction _moveInputAction;
        private Vector2 _mousePositionInWorld;

        [Inject] private Grid<APiece> _grid;
        [Inject] private BoardManager _boardManager;

        /// <summary>
        /// Inicializa as ações de input e registra os callbacks.
        /// </summary>
        public void Init()
        {
            _clickInputAction = InputSystem.actions.FindAction("Click");
            if (_clickInputAction != null)
                _clickInputAction.started += OnClicked;
            else
                Debug.LogError("Ação 'Click' não encontrada.");

            _moveInputAction = InputSystem.actions.FindAction("Move");
            if (_moveInputAction != null)
                _moveInputAction.performed += OnMove;
            else
                Debug.LogError("Ação 'Move' não encontrada.");
        }

        /// <summary>
        /// Remove os callbacks das ações de input.
        /// </summary>
        public void Stop()
        {
            if (_clickInputAction != null)
                _clickInputAction.started -= OnClicked;

            if (_moveInputAction != null)
                _moveInputAction.performed -= OnMove;
        }

        /// <summary>
        /// Callback acionado quando ocorre o clique.
        /// Seleciona a célula correspondente à posição do mouse.
        /// </summary>
        /// <param name="context">Contexto do callback de input.</param>
        private void OnClicked(InputAction.CallbackContext context)
        {
            var cell = _grid.GetCell(_mousePositionInWorld);
            _boardManager.SelectCell(cell);
        }

        /// <summary>
        /// Callback acionado quando o mouse se move.
        /// Atualiza a posição do mouse em coordenadas do mundo e destaca a célula correspondente.
        /// </summary>
        /// <param name="context">Contexto do callback de input.</param>
        private void OnMove(InputAction.CallbackContext context)
        {
            _mousePositionInWorld = context.ReadValue<Vector2>();

            if (Camera.main != null)
            {
                Vector3 screenPoint = new Vector3(_mousePositionInWorld.x, _mousePositionInWorld.y, 0f);
                _mousePositionInWorld = Camera.main.ScreenToWorldPoint(screenPoint);
            }
            else
            {
                Debug.LogError("Camera.main não foi encontrada.");
            }

            var cell = _grid.GetCell(_mousePositionInWorld);
            _boardManager.OverMouseCell(cell);
        }
    }
}
