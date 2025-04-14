using UnityEngine;

namespace GridSystem
{
    /// <summary>
    /// Componente que gerencia o comportamento visual das células na grade.
    /// Permite alternar entre diferentes estados visuais (idle, mouse over, selecionado, preview).
    /// </summary>
    public class RendererCellBehaviour : MonoBehaviour
    {
        private Renderer _renderer;

        private Color _baseColor;
        private Color _highlitColor;
        private Color _selectedColor;
        private Color _previewColor;

        /// <summary>
        /// Inicializa o componente, tentando obter o Renderer associado.
        /// </summary>
        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
            if (_renderer == null)
            {
                Debug.LogError("Renderer não encontrado em RendererCellBehaviour.");
            }
        }

        /// <summary>
        /// Configura os dados de cor para os diferentes estados da célula.
        /// </summary>
        /// <param name="baseColor">Cor padrão da célula.</param>
        /// <param name="highlitColor">Cor para o estado de mouse over (highlight).</param>
        /// <param name="selectedColor">Cor para o estado selecionado.</param>
        /// <param name="previewColor">Cor para o estado de preview.</param>
        public void SetData(Color baseColor, Color highlitColor, Color selectedColor, Color previewColor)
        {
            _baseColor = baseColor;
            _selectedColor = selectedColor;
            _previewColor = previewColor;
            // Mescla a cor base com a cor de highlight para o estado de mouse over.
            _highlitColor = baseColor + highlitColor;
        }

        /// <summary>
        /// Define a célula para o estado idle (normal).
        /// </summary>
        public void SetIdleState()
        {
            if (_renderer != null)
                _renderer.material.color = _baseColor;
        }

        /// <summary>
        /// Define a célula para o estado de mouse over (quando o cursor está sobre ela).
        /// </summary>
        public void SetMouseOverState()
        {
            if (_renderer != null)
                _renderer.material.color = _highlitColor;
        }

        /// <summary>
        /// Define a célula para o estado selecionado.
        /// </summary>
        public void SetSelectedState()
        {
            if (_renderer != null)
                _renderer.material.color = _selectedColor;
        }

        /// <summary>
        /// Define a célula para o estado de preview.
        /// </summary>
        public void SetPreviewState()
        {
            if (_renderer != null)
                _renderer.material.color = _previewColor;
        }
    }
}
