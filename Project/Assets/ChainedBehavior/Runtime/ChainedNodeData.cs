using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Reflection;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace ChainedBehavior
{
    /// <summary>
    /// ScriptableObject que gerencia a cadeia de execução dos nós.
    /// Armazena uma coleção de nós e gerencia a hierarquia através de um nó raiz.
    /// </summary>
    public class ChainedNodeData : ScriptableObject
    {
        /// <summary>
        /// Nó raiz que inicia a execução da cadeia.
        /// </summary>
        public TriggerNode Root;

        /// <summary>
        /// Lista de todos os nós presentes na cadeia.
        /// </summary>
        public List<Node> Nodes = new List<Node>();

        /// <summary>
        /// Atualiza a cadeia de nós começando pelo nó raiz.
        /// </summary>
        /// <returns>O resultado da execução do nó raiz e seus descendentes.</returns>
        public async UniTask<Node.StateResult> Update()
        {
            if (Root == null)
            {
                Debug.LogWarning("O nó raiz (Root) está indefinido.");
                return Node.StateResult.Failure;
            }
            return await Root.Run();
        }

        /// <summary>
        /// Cria e registra um novo nó do tipo especificado.
        /// </summary>
        /// <param name="type">Tipo do nó a ser criado.</param>
        /// <returns>Instância do nó criado.</returns>
        public Node CreateNode(System.Type type)
        {
            Node node = ScriptableObject.CreateInstance(type) as Node;
            node.name = type.Name;
            node.Guid = System.Guid.NewGuid().ToString();

            Nodes.Add(node);
#if UNITY_EDITOR
            AssetDatabase.AddObjectToAsset(node, this);
            AssetDatabase.SaveAssets();
#endif
            return node;
        }

        /// <summary>
        /// Remove um nó da cadeia, tanto da lista quanto do asset.
        /// </summary>
        /// <param name="node">Nó a ser removido.</param>
        public void DeleteNode(Node node)
        {
            if (Nodes.Contains(node))
            {
                Nodes.Remove(node);
#if UNITY_EDITOR
                AssetDatabase.RemoveObjectFromAsset(node);
                AssetDatabase.SaveAssets();
#endif
            }
            else
            {
                Debug.LogWarning("Tentativa de remover um nó que não existe na lista.");
            }
        }

        /// <summary>
        /// Define um nó filho para um nó pai.
        /// </summary>
        /// <param name="parent">Nó pai.</param>
        /// <param name="child">Nó filho a ser adicionado.</param>
        public void AddChild(Node parent, Node child)
        {
            if (parent != null)
            {
                parent.ChildNode = child;
            }
            else
            {
                Debug.LogWarning("Nó pai é nulo na adição de um filho.");
            }
        }

        /// <summary>
        /// Remove o nó filho de um nó pai, caso corresponda.
        /// </summary>
        /// <param name="parent">Nó pai.</param>
        /// <param name="child">Nó filho a ser removido.</param>
        public void RemoveChild(Node parent, Node child)
        {
            if (parent != null && parent.ChildNode == child)
            {
                parent.ChildNode = null;
            }
            else
            {
                Debug.LogWarning("O nó filho fornecido não corresponde ao nó filho atual do pai.");
            }
        }

        /// <summary>
        /// Obtém o nó filho de um nó pai.
        /// </summary>
        /// <param name="parent">Nó pai.</param>
        /// <returns>O nó filho ou null se não houver.</returns>
        public Node GetChild(Node parent)
        {
            return parent?.ChildNode;
        }
    }
}
