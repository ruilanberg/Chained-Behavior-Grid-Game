using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace ChainedBehavior.Editor
{
    /// <summary>
    /// Representa a visualização de um nó na GraphView.
    /// Exibe seus campos e suas portas de entrada/saída.
    /// </summary>
    public class NodeView : UnityEditor.Experimental.GraphView.Node
    {
        // Referência lógica ao nó subjacente.
        public Node Node;

        // Porta de entrada do nó (usada para conexões de ativação).
        public Port Input { get; private set; }

        // Porta de saída do nó (usada para conexões encadeadas).
        public Port Output { get; private set; }

        /// <summary>
        /// Constrói uma visualização de nó a partir do nó lógico.
        /// </summary>
        /// <param name="node">Instância do nó a ser exibido.</param>
        public NodeView(Node node)
        {
            this.Node = node;
            title = node.name;
            viewDataKey = node.Guid;

            style.left = node.Position.x;
            style.top = node.Position.y;

            CreateInputPort();
            CreateOutputPort();
            CreateFields();
        }

        /// <summary>
        /// Cria campos visuais baseados em atributos privados declarados no nó com reflection.
        /// Apenas enums são suportados no momento.
        /// </summary>
        public void CreateFields()
        {
            var fields = Node.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.DeclaredOnly | BindingFlags.Instance);

            foreach (var field in fields)
            {
                if (field.FieldType.IsEnum)
                {
                    Array enumValues = Enum.GetValues(field.FieldType);
                    var enumNames = new List<string>();

                    foreach (var val in enumValues)
                        enumNames.Add(val.ToString());

                    var dropdown = new DropdownField(field.Name, enumNames, 0);

                    int currentIndex = (int)field.GetValue(Node);
                    dropdown.index = currentIndex;

                    dropdown.RegisterValueChangedCallback(evt =>
                    {
                        Node.SetValueField(field, dropdown.index);
                    });

                    this.Add(dropdown);
                }
            }
        }

        /// <summary>
        /// Atualiza a posição lógica do nó quando o usuário o move na interface.
        /// </summary>
        /// <param name="newPos">Nova posição da janela do nó.</param>
        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);
            Node.Position.x = newPos.xMin;
            Node.Position.y = newPos.yMin;
        }

        /// <summary>
        /// Cria a porta de entrada, exceto para nós do tipo TriggerNode.
        /// </summary>
        private void CreateInputPort()
        {
            if (Node.GetType() == typeof(TriggerNode))
                return;

            Input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));
            Input.portName = "Activation";

            inputContainer.Add(Input);
        }

        /// <summary>
        /// Cria a porta de saída do nó.
        /// </summary>
        private void CreateOutputPort()
        {
            Output = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
            Output.portName = "Chain";

            outputContainer.Add(Output);
        }
    }
}
