## Projeto IA

### Algoritmo A* (A Estrela)

O algoritmo A* é um algoritmo de busca usado em jogos e aplicações para calcular a menor distância possivel entre dois pontos. Ele procura um caminho num grafo desde um vértice inicial até um vértice final. A* combina o algoritmo de Dijkstra com uma função heurística de forma a encontrar o nó mais perto da meta.

Controlos:

As teclas WASD movimentam o objeto *target*. Alternativamente, é possivel arrastá-lo diretamente na scene do Unity. 


### Implementação

Este algoritmo avalia os custos dos nós vizinhos ao nó inicial, seleciona o nó de menor custo estimado, adiciona-o a uma lista e atualiza o valor dos nós adjacentes. O algoritmo continua até que o nó removido seja o nó final.
Quando o grafo encontra o nó final, esse nó aponta para o nó pai anterior até que volte para o nó inicial, formando um caminho.

Existem vários objetos na cena. As paredes e obstáculos (laranja) não são percorríveis pela IA. Existem também zonas de relva e àgua (verde e azul) que adicionam diferentes pesos aos nós associados.

![image](https://github.com/user-attachments/assets/85f90d54-5f8f-404b-bfc2-1b43bd51f37f)

O target é capaz de atravesar os obstáculos sem impedimentos de forma a observar melhor o comportamento da IA. 

Para que os objetos não andem colados aos obstáculos, foi aplicado um blur entre o custo de um nó com os nós adjacentes, de forma a que o caminho resultante pareça mais natural.

![image](https://github.com/user-attachments/assets/afb03e04-25f7-49e3-bf25-4d818b060479)

Fazendo uso dos Gizmos é possivel ver os nós em forma de células numa grelha, a área ocupada pelos obstáculos (vermelho), o custo ou peso de cada nó (gradiente preto e branco) e o caminho calculado pelo algoritmo (verde). O algoritmo calcula o caminho ao iniciar o programa e re-calcula cada vez que o target se move.

Ana Rita Matos nº11556
