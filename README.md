## Projeto IA

### Algoritmo A* (A Estrela)

O algoritmo A* é um algoritmo de busca usado em jogos e aplicações para calcular a menor distância possivel entre dois pontos. Ele procura um caminho num grafo desde um vértice inicial até um vértice final. A* combina o algoritmo de Dijkstra com uma função heurística de forma a encontrar o nó mais perto da meta.

Controlos:

- As teclas WASD movimentam o objeto *target*. Alternativamente, é possivel arrastá-lo diretamente na scene do Unity.

A IA segue o objeto controlado pelo jogador, evitando os obstáculos. 

##
### Implementação


Este algoritmo avalia os custos dos nós vizinhos ao nó inicial, seleciona o nó de menor custo estimado, adiciona-o a uma lista e atualiza o valor dos nós adjacentes. O algoritmo continua até que o nó removido seja o nó final.
Quando o grafo encontra o nó final, esse nó aponta para o nó pai anterior até que volte para o nó inicial, formando um caminho. Existem vários objetos na cena. As paredes e obstáculos (laranja) não são percorríveis pela IA. O jogador é capaz de atravesar os obstáculos sem impedimentos de forma a observar melhor o comportamento da IA. 

![image](https://github.com/user-attachments/assets/85f90d54-5f8f-404b-bfc2-1b43bd51f37f)
- heap

Os custos dos nós não visitados(Openset) estão guardados numa estrutura de árvore binária min heap, onde o valor menor, ou seja, o nó de menor custo, estará na raiz da àrvore.

![image](https://github.com/user-attachments/assets/adc985a7-7e71-4bbe-aa24-15e6800e6fe6)

##
- weights

Para além dos obstáculos e paredes, existem também zonas de relva e àgua (verde e azul) que adicionam diferentes pesos aos nós. Estes valores estão guardados num dictionary e através de um raycast que verifica se colide com um e adiciona o peso associado ao custo do nó na grelha. 

![image](https://github.com/user-attachments/assets/76b5d124-52ea-48b9-822b-03b950c4cf1b)

- blur

Para que os objetos não andem colados aos obstáculos, foi aplicado um blur entre o custo de um nó com os nós adjacentes, de forma a que o caminho resultante pareça mais natural.

![image](https://github.com/user-attachments/assets/b3f7b622-db2c-4d2f-bfc5-78f3e5221a85)

##
- update

 O algoritmo é capaz de calcular o caminho para múltiplos objetos ao iniciar o programa e re-calcula-os cada vez que o jogador se move.

![image](https://github.com/user-attachments/assets/3ed38dda-ee3c-447d-aa15-684598f99985)

##

Ana Rita Matos nº11556
