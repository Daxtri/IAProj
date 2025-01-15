## Projeto IA

### Algoritmo A* (A Estrela)

O algoritmo A* é um algoritmo de busca usado em jogos e aplicações para calcular a menor distância possivel entre dois pontos. Ele procura um caminho num grafo desde um vértice inicial até um vértice final. A* combina o algoritmo de Dijkstra com uma função heurística de forma a encontrar o nó mais perto da meta.

Controlos:

- As teclas WASD movimentam o objeto *target*. Alternativamente, é possivel arrastá-lo diretamente na scene do Unity.

A IA controla 3 unidades diferentes que perseguem o objeto controlado pelo jogador, evitando os obstáculos. 

##
### Implementação


Existem vários objetos na cena. As paredes e obstáculos (laranja) não são percorríveis pela IA. O jogador é capaz de atravesar os obstáculos sem impedimentos de forma a observar melhor o comportamento da IA. 

![image](https://github.com/user-attachments/assets/11274e0e-0ac9-4c81-90d4-df55f92c0b14)


Este algoritmo avalia os custos dos nós vizinhos ao nó inicial, seleciona o nó de menor custo estimado, adiciona-o a um hashset e atualiza o valor dos nós adjacentes. O algoritmo continua até que o nó removido seja o nó final.
Quando o grafo encontra o nó final, esse nó aponta para o nó pai anterior até que volte para o nó inicial, formando um caminho. 

![Pseudocode-for-A-search-algorithm](https://github.com/user-attachments/assets/d111364d-9748-40a1-b023-9b995a27df4f)
![image](https://github.com/user-attachments/assets/3a4d6c53-e8fc-4f62-ad12-22ce0f066b71)
![image](https://github.com/user-attachments/assets/dd19789c-596f-4d9d-b7b3-1ddc88393b5c)


Os custos dos nós não visitados(Openset) estão guardados numa estrutura de árvore binária min heap, onde o valor menor, ou seja, o nó de menor custo, estará na raiz da àrvore.

![image](https://github.com/user-attachments/assets/adc985a7-7e71-4bbe-aa24-15e6800e6fe6)

##

Para além dos obstáculos e paredes, existem também zonas de relva e àgua (verde e azul) que adicionam diferentes pesos aos nós. Estes valores estão guardados num dictionary e faz uso de um raycast que verifica a colisão com essas zonas e adiciona o peso associado ao custo do nó na grelha. 

![image](https://github.com/user-attachments/assets/870050c6-423b-46fa-ad81-2827099bdbaa)

![image](https://github.com/user-attachments/assets/76b5d124-52ea-48b9-822b-03b950c4cf1b)

Para que os objetos não se desloquem pelos limites dos obstáculos, foi aplicado um blur entre o custo de um nó com os nós adjacentes, de forma a que o caminho resultante pareça mais natural. Também foi adicionado um peso aos nós ocupados por um obstáculo para que a IA evite tocar nas paredes.

![image](https://github.com/user-attachments/assets/b3f7b622-db2c-4d2f-bfc5-78f3e5221a85)

##

 O algoritmo é capaz de calcular o caminho para múltiplos objetos ao iniciar o programa e re-calcula-os cada vez que o jogador se move.

![image](https://github.com/user-attachments/assets/3ed38dda-ee3c-447d-aa15-684598f99985)

![image](https://github.com/user-attachments/assets/0ad4f900-9f12-444d-a6a6-e25b0b550b05)

##
### Máquina de Estados Finita

A máquina de estados finita é uma máquina abstrata que pode estar em exatamente um de entre um número finito de estados a qualquer momento. Cada estado descreve uma ação a ser realizada por um NPC e as condições de transição. Uma transição indica uma mudança de estado e é descrita por uma condição que precisa ser realizada para que a transição ocorra.

Neste projeto, a transição ocorre quando o jogador entra dentro da àrea de deteção (branco) à volta do NPC.

![image](https://github.com/user-attachments/assets/7120899c-676f-4b70-aca2-e65467dac5b0)
 

Enquanto o jogador se encontrar fora desta área, o objeto movimenta-se aleatoriamente sobre o mapa, em estado de patrulha. 

![image](https://github.com/user-attachments/assets/b3e488c0-f03c-4d2e-9915-067dc86ce29b)

Assim que o jogador entra dentro da àrea de deteçâo do objeto, a màquina de estados transiciona para o estado de perseguição, seguindo o jogador até que este volte a sair da àrea de deteção do NPC.

![image](https://github.com/user-attachments/assets/97275233-50fc-45d3-9b16-a06262d3340a)

##

Ana Rita Matos nº11556
