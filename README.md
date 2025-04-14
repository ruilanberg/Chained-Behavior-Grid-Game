# Chained Behavior Grid Game Framework

Um framework baseada em Unity para jogos de turno que utiliza um sistema de grade (grid) e um mecanismo de nós encadeados (*chained behavior nodes*) para controlar o comportamento das peças. Inclui também ferramentas de editor para visualizar e modificar as cadeias de comportamento de forma gráfica.

## Visão Geral

Este projeto tem como objetivo fornecer uma arquitetura modular e escalável para desenvolver jogos de turno baseados em grid. Ele divide as responsabilidades em diversos módulos:

- **Grid System:** Gerencia a criação e manipulação de grades e células, permitindo o posicionamento preciso de peças.
- **Game Pieces:** Define classes base e especializadas para peças (player, enemy, obstacles) e suas renderizações, utilizando renderizadores personalizáveis (SpriteRenderer e MeshRenderer).
- **Chained Behavior:** Implementa um sistema baseado em nós para determinar o comportamento das peças e a execução de ações em cadeia. Os nós podem ser do tipo de comportamento simples, gatilho ou de movimento.
- **Turn State Machine:** Gerencia os estados de turno (setup, turno do jogador, execução do movimento) usando uma máquina de estados que integra os nós de comportamento para atualizar a lógica do jogo.
- **Editor Tools:** Extensões para o editor do Unity que permitem visualizar e editar graficamente a cadeia de comportamento, facilitando a criação e modificação dos nós.

## Recursos Principais

- **Sistema de Grade:** Criação dinâmica de células com base em configurações (largura, altura e tamanho das células) para posicionamento de peças.
- **Peças Customizadas:** Implementações para peças do jogador, inimigas e obstáculos, cada uma com métodos próprios de renderização.
- **Renderizadores Modulares:** Utiliza classes abstratas para definir renderizadores de peças, possibilitando o uso de MeshRenderer (malha 3D) ou SpriteRenderer conforme a necessidade.
- **Nó Encadeado de Comportamento:** Permite definir e encadear ações para execução de comportamentos complexos de forma assíncrona.
- **Editor Visual:** Ferramenta customizada no editor do Unity que possibilita a edição gráfica da cadeia de comportamento, com visualização de conexões, campos editáveis e menus contextuais.
- **Injeção de Dependências:** Utiliza o container *Reflex* para facilitar a injeção de dependências, promovendo um código mais limpo e modular.
- **Animações com DOTween:** Animação de movimentos de peças com interpolação suave para transições e movimentações.

## Tecnologias e Dependências

- **Unity 6000.0.39f1 LTS** (ou superior)
- **C#**
- **DOTween:** Para animações e interpolação de movimentos.
- **Cysharp.Threading.Tasks (UniTask):** Execução assíncrona e gerenciamento de tarefas.
- **Reflex Framework:** Para injeção de dependências.


## Instalação e Configuração

1. **Requisitos:**
   - Instale o Unity (recomenda-se a versão 6000.0.39f1 LTS ou superior).
   - Certifique-se de ter os pacotes DOTween, Cysharp.Threading.Tasks e Reflex Framework instalados no seu projeto.  
   - (Opcional) Ferramentas para edição e depuração do editor, como o Visual Studio Code ou Visual Studio.

2. **Importação:**
   - Clone ou importe o projeto para a pasta `Assets` do seu projeto Unity.
   - Configure os ScriptableObjects necessários (por exemplo, GridSettings, GameSettings, renderizadores) através do menu *Create > ScriptableObjects* conforme definido nos atributos `[CreateAssetMenu]`.

3. **Configuração da Cena:**
   - Crie uma cena e adicione um objeto com o componente `BoardManager`.
   - Adicione componentes ou contêiner para injeção de dependências conforme a utilização do Reflex.
   - Configure os valores de grid (largura, altura, tamanho) e de jogo (arrays de peças) através dos ScriptableObjects.

## Uso

- **Spawning de Peças:**  
  O `BoardManager` é responsável por instanciar as peças do jogo (inimigos, obstáculos, jogadores) e posicioná-las em células aleatórias do grid. Certifique-se de que as referências de `GameSettings` e `RendererPiece` estejam configuradas.

- **Turnos e Comportamento:**  
  A máquina de estados (Turn State Machine) define o fluxo do jogo:
  - `SetupTurnState` instancia e posiciona as peças.
  - `PlayerTurnState` permite ao jogador selecionar e movimentar suas peças.
  - `NodeRunTurnState` executa a cadeia de comportamento associada à peça selecionada, utilizando os nós encadeados.
  
- **Editor de Cadeia de Comportamento:**  
  Selecione uma peça que derive de `APiece` e, no Inspector, clique no botão "Edit Piece Behavior Chain" para abrir a janela do *ChainedBehaviorEditor*. Esta ferramenta permite visualizar graficamente os nós, criar e conectar novos nós, além de editar parâmetros via campos e menus contextuais.

## Arquitetura e Extensão

- **Sistema de Nós:**  
  A classe abstrata `Node` define a estrutura básica para nós, incluindo métodos assíncronos (`OnStart`, `OnUpdate`, `OnStop`) que podem ser sobrepostos para criar comportamentos personalizados.  
  Para adicionar novos tipos de comportamento, crie uma classe que herde de `Node` e implemente os métodos abstratos.

- **Renderizadores de Peças:**  
  As classes `ARendererPieceScriptableObject` e suas implementações (como `SpriteRendererPiece` e `MeshRendererPiece`) permitem configurar a aparência das peças. Para criar um novo renderizador, estenda `ARendererPieceScriptableObject` e implemente os métodos de criação de renderizadores para cada tipo de peça.

- **Injeção de Dependências:**  
  Utilize o container Reflex para injetar dependências nos componentes que precisarem (por exemplo, `BoardManager`, `Grid`, `GameSettings`). Isso facilita a manutenção e desacoplamento do código.
