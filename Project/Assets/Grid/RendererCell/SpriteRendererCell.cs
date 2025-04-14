using UnityEngine;

namespace GridSystem
{
    /// <summary>
    /// ScriptableObject que cria um renderer para células utilizando um SpriteRenderer.
    /// Configura o sprite e as cores de acordo com as configurações definidas.
    /// </summary>
    [CreateAssetMenu(fileName = "SpriteRendererCell", menuName = "ScriptableObjects/SpriteRendererCell", order = 3)]
    public class SpriteRendererCell : ARendererCellScriptableObject
    {
        [SerializeField] private Sprite _sprite;
        private SpriteRenderer _spriteRenderer;

        /// <summary>
        /// Cria o renderer da célula utilizando um SpriteRenderer.
        /// </summary>
        /// <param name="root">O transform pai para a célula.</param>
        /// <param name="size">Tamanho da célula.</param>
        /// <param name="x">Coordenada X da célula na grade.</param>
        /// <param name="y">Coordenada Y da célula na grade.</param>
        /// <returns>A instância de <see cref="RendererCellBehaviour"/> configurada para a célula.</returns>
        public override RendererCellBehaviour CreateCellRenderer(Transform root, float size, int x, int y)
        {
            if (_sprite == null)
            {
                Debug.LogError("Sprite não definido em SpriteRendererCell.");
                return null;
            }
            if (root == null)
            {
                Debug.LogError("Transform raiz não pode ser nulo.");
                return null;
            }

            // Cria o GameObject para a célula e define seu parent
            GameObject cell = new GameObject($"({x}, {y})GridCell");
            cell.transform.parent = root;

            // Adiciona o SpriteRenderer e configura o sprite e a cor
            _spriteRenderer = cell.AddComponent<SpriteRenderer>();
            _spriteRenderer.sprite = _sprite;
            _spriteRenderer.material.color = IsOffset(x, y) ? _offsetColor : _baseColor;

            // Define a posição da célula na grade
            cell.transform.position = new Vector3(x * size, y * size, 0);

            // Adiciona o componente de comportamento e configura as cores
            var rendererBehaviour = cell.AddComponent<RendererCellBehaviour>();
            rendererBehaviour.SetData(_spriteRenderer.material.color, _highlitColor, _selectedColor, _previewColor);

            return rendererBehaviour;
        }
    }
}
