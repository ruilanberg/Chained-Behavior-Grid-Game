using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChainedBehavior.Editor
{
    /// <summary>
    /// Visualização gráfica principal do editor de comportamento encadeado.
    /// </summary>
    [UxmlElement("ChainedBehavior")]
    public partial class ChainedBehaviorEditorView : GraphView
    {
        /// <summary>
        /// Dados atualmente carregados no grafo.
        /// </summary>
        private static ChainedNodeData _chainedNodeData;

        /// <summary>
        /// Construtor da visualização. Adiciona manipuladores e estilo.
        /// </summary>
        public ChainedBehaviorEditorView()
        {
            var gridBackground = new GridBackground();
            Insert(0, gridBackground);

            var style = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/ChainedBehavior/Editor/ChainedBehaviorEditor.uss");
            if (style != null)
            {
                styleSheets.Add(style);
            }
            else
            {
                Debug.LogWarning("StyleSheet não encontrado em 'Assets/ChainedBehavior/Editor/ChainedBehaviorEditor.uss'.");
            }

            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
        }

        /// <summary>
        /// Popula o editor com os dados do grafo de comportamento encadeado.
        /// </summary>
        /// <param name="chainedNodeData">Dados a serem exibidos.</param>
        public void Populate(ChainedNodeData chainedNodeData)
        {
            _chainedNodeData = chainedNodeData;

            graphViewChanged -= OnGraphViewChanged;
            DeleteElements(graphElements);
            graphViewChanged += OnGraphViewChanged;

            if (_chainedNodeData.Root == null)
                _chainedNodeData.Root = _chainedNodeData.CreateNode(typeof(TriggerNode)) as TriggerNode;

            _chainedNodeData.Nodes.ForEach(n => CreateNodeView(n));

            _chainedNodeData.Nodes.ForEach(n =>
            {
                if (n.ChildNode == null)
                    return;

                NodeView parentView = FindNodeView(n);
                NodeView childView = FindNodeView(n.ChildNode);

                if (parentView?.Output != null && childView?.Input != null)
                {
                    Edge edge = parentView.Output.ConnectTo(childView.Input);
                    AddElement(edge);
                }
            });
        }

        /// <summary>
        /// Encontra a visualização de nó correspondente ao nó lógico.
        /// </summary>
        /// <param name="node">Nó lógico.</param>
        /// <returns>Visual do nó (NodeView).</returns>
        private NodeView FindNodeView(Node node)
        {
            return GetNodeByGuid(node.Guid) as NodeView;
        }

        /// <summary>
        /// Define quais portas podem ser conectadas entre si.
        /// </summary>
        /// <param name="startPort">Porta inicial da conexão.</param>
        /// <param name="nodeAdapter">Adaptador de nó (não utilizado).</param>
        /// <returns>Lista de portas compatíveis.</returns>
        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            return ports.ToList().Where(endPort =>
                endPort.direction != startPort.direction &&
                endPort.node != startPort.node).ToList();
        }

        /// <summary>
        /// Trata mudanças na visualização do grafo (remoções e adições de nós/conexões).
        /// </summary>
        /// <param name="graphViewChange">Mudanças detectadas.</param>
        /// <returns>Mesmas mudanças, após processadas.</returns>
        private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
        {
            if (graphViewChange.elementsToRemove != null)
            {
                graphViewChange.elementsToRemove.ForEach(elem =>
                {
                    if (elem is NodeView nodeView)
                    {
                        _chainedNodeData.DeleteNode(nodeView.Node);
                    }

                    if (elem is Edge edge)
                    {
                        NodeView parent = edge.output.node as NodeView;
                        NodeView child = edge.input.node as NodeView;
                        _chainedNodeData.RemoveChild(parent.Node, child.Node);
                    }
                });
            }

            if (graphViewChange.edgesToCreate != null)
            {
                graphViewChange.edgesToCreate.ForEach(edge =>
                {
                    NodeView parentView = edge.output.node as NodeView;
                    NodeView childView = edge.input.node as NodeView;
                    _chainedNodeData.AddChild(parentView.Node, childView.Node);
                });
            }

            return graphViewChange;
        }

        /// <summary>
        /// Cria menu de contexto com as opções de tipos de nós que podem ser adicionados.
        /// </summary>
        /// <param name="evt">Evento do menu de contexto.</param>
        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            var types = TypeCache.GetTypesDerivedFrom<Node>();
            foreach (var type in types)
            {
                if (type == typeof(TriggerNode))
                    continue;

                string nameOpt = $"{type.Name}";
                evt.menu.AppendAction(nameOpt, (a) => CreateNode(type));
            }
        }

        /// <summary>
        /// Cria um novo nó do tipo especificado.
        /// </summary>
        /// <param name="type">Tipo do nó.</param>
        private void CreateNode(Type type)
        {
            Node node = _chainedNodeData.CreateNode(type);
            CreateNodeView(node);
        }

        /// <summary>
        /// Cria e adiciona à visualização um NodeView baseado no Node fornecido.
        /// </summary>
        /// <param name="node">Nó lógico a ser exibido.</param>
        private void CreateNodeView(Node node)
        {
            NodeView nodeView = new NodeView(node);
            AddElement(nodeView);
        }
    }
}
