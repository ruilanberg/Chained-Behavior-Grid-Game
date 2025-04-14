using UnityEngine;

namespace GridSystem
{
    /// <summary>
    /// Classe abstrata base para ScriptableObjects responsáveis por criar renderers para as células da grade.
    /// Define cores básicas e estados para os renderers.
    /// </summary>
    public abstract class ARendererCellScriptableObject : ScriptableObject
    {
        [SerializeField] protected Color _baseColor;
        [SerializeField] protected Color _offsetColor;
        [Space]
        [SerializeField] protected Color _highlitColor;
        [SerializeField] protected Color _selectedColor;
        [SerializeField] protected Color _previewColor;

        /// <summary>
        /// Cria e retorna a instância de <see cref="RendererCellBehaviour"/> para a célula.
        /// </summary>
        /// <param name="root">O transform pai para o objeto da célula.</param>
        /// <param name="size">Tamanho da célula.</param>
        /// <param name="x">Coordenada X da célula na grade.</param>
        /// <param name="y">Coordenada Y da célula na grade.</param>
        /// <returns>Instância configurada do renderer da célula.</returns>
        public abstract RendererCellBehaviour CreateCellRenderer(Transform root, float size, int x, int y);

        /// <summary>
        /// Determina se a célula com as coordenadas informadas deve usar a cor de offset.
        /// </summary>
        /// <param name="x">Coordenada X da célula.</param>
        /// <param name="y">Coordenada Y da célula.</param>
        /// <returns>True se a célula deve usar a cor de offset; caso contrário, false.</returns>
        protected bool IsOffset(int x, int y)
        {
            return (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
        }
    }
}
