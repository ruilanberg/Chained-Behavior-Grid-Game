using UnityEngine;

namespace GridSystem
{
    /// <summary>
    /// ScriptableObject que armazena as configurações da grade.
    /// Permite definir a largura, altura e o tamanho de cada célula.
    /// </summary>
    [CreateAssetMenu(fileName = "GridSettings", menuName = "ScriptableObjects/GridSettings", order = 1)]
    public class GridSettings : ScriptableObject
    {
        /// <summary>
        /// Número de colunas da grade.
        /// </summary>
        [field: SerializeField] public int GridWidth { get; private set; } = 8;

        /// <summary>
        /// Número de linhas da grade.
        /// </summary>
        [field: SerializeField] public int GridHeight { get; private set; } = 8;

        /// <summary>
        /// Tamanho de cada célula da grade.
        /// </summary>
        [field: SerializeField] public float CellSize { get; private set; } = 1f;
    }
}
