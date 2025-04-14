using Game.Piece;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace ChainedBehavior.Editor
{
    public class ChainedBehaviorEditor : EditorWindow
    {
        // Asset visual (UXML) usado para construir a interface do editor.
        [SerializeField] private VisualTreeAsset m_VisualTreeAsset = default;

        /// <summary>
        /// Visualização gráfica usada para editar o comportamento encadeado.
        /// </summary>
        private ChainedBehaviorEditorView _graphView;

        /// <summary>
        /// Abre a janela do editor ChainedBehaviorEditor e define o título.
        /// </summary>
        /// <param name="data">Dados do nó encadeado para carregar (opcional).</param>
        public static void ShowPanel(ChainedNodeData data = null)
        {
            var wnd = GetWindow<ChainedBehaviorEditor>();
            wnd.titleContent = new GUIContent("ChainedBehaviorEditor");

            if (data != null)
            {
                wnd.PopulateView(data);
            }
        }

        /// <summary>
        /// Inicializa a interface gráfica da janela do editor.
        /// Carrega o layout visual e aplica estilos.
        /// </summary>
        public void CreateGUI()
        {
            if (m_VisualTreeAsset == null)
            {
                Debug.LogError("VisualTreeAsset não atribuído no editor.");
                return;
            }

            VisualElement root = rootVisualElement;

            // Instancia o layout visual definido no UXML
            m_VisualTreeAsset.CloneTree(root);

            // Carrega e aplica o estilo visual (USS)
            var style = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/ChainedBehavior/Editor/ChainedBehaviorEditor.uss");
            if (style != null)
                root.styleSheets.Add(style);
            else
                Debug.LogWarning("StyleSheet não encontrado em 'Assets/ChainedBehavior/Editor/ChainedBehaviorEditor.uss'.");

            // Encontra a visualização personalizada do editor
            _graphView = root.Q<ChainedBehaviorEditorView>();

            OnSelectionChange();
        }

        /// <summary>
        /// Chamado automaticamente quando a seleção no editor do Unity muda.
        /// Atualiza a visualização com base no objeto selecionado.
        /// </summary>
        private void OnSelectionChange()
        {
            if (Selection.activeObject == null)
                return;

            TryHandleSelectedObject(Selection.activeObject);
        }

        /// <summary>
        /// Processa o objeto selecionado, verificando se é um GameObject com um componente APiece válido.
        /// </summary>
        /// <param name="obj">Objeto selecionado.</param>
        private void TryHandleSelectedObject(UnityEngine.Object obj)
        {
            if (TryGetPieceFromObject(obj, out APiece piece))
            {
                EnsureChainedNodeDataExists(piece);
                PopulateView(piece.ChainedNode);
            }
            else
                Debug.LogWarning("Selecione um GameObject com um componente do tipo APiece.");
        }

        /// <summary>
        /// Tenta obter um componente APiece a partir de um objeto Unity.
        /// </summary>
        /// <param name="obj">Objeto para verificação.</param>
        /// <param name="piece">Retorna o componente APiece se encontrado.</param>
        /// <returns>True se for um GameObject com APiece; caso contrário, false.</returns>
        private bool TryGetPieceFromObject(UnityEngine.Object obj, out APiece piece)
        {
            piece = null;

            if (obj is GameObject gameObject && gameObject.TryGetComponent(out APiece foundPiece))
            {
                piece = foundPiece;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Garante que o objeto APiece tenha um ScriptableObject ChainedNodeData associado.
        /// Cria um novo se necessário.
        /// </summary>
        /// <param name="piece">Componente APiece.</param>
        private void EnsureChainedNodeDataExists(APiece piece)
        {
            if (piece.ChainedNode == null)
            {
                piece.ChainedNode = ScriptableObject.CreateInstance<ChainedNodeData>();

                AssetDatabase.AddObjectToAsset(piece.ChainedNode, piece);
                AssetDatabase.SaveAssets();

                Debug.Log($"Novo ChainedNodeData criado e vinculado a '{piece.name}'.");
            }
        }

        /// <summary>
        /// Popula a visualização gráfica com os dados fornecidos.
        /// </summary>
        /// <param name="data">Dados encadeados a serem exibidos.</param>
        private void PopulateView(ChainedNodeData data)
        {
            if (_graphView == null)
            {
                Debug.LogError("GraphView não foi inicializado.");
                return;
            }

            _graphView.Populate(data);
        }
    }
}
