using Reflex.Core;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// Instalador do projeto.
    /// Este script pode ser utilizado para registrar outras dependências ou realizar configurações de inicialização específicas.
    /// </summary>
    public class ProjectInstaller : MonoBehaviour, IInstaller
    {
        /// <summary>
        /// Método responsável por instalar as dependências no contêiner.
        /// Atualmente, não realiza nenhuma operação, mas pode ser expandido conforme necessário.
        /// </summary>
        /// <param name="containerBuilder">O construtor de contêiner para adicionar as dependências.</param>
        public void InstallBindings(ContainerBuilder containerBuilder)
        {
            // Implementar registro de dependências adicionais, se necessário.
        }
    }
}
