### Mecânicas


##### Movimentação

O personagem poderá se mover nas 8 direções usando qualquer combinação das teclas WASD.

##### Ataque

O personagem poderá desferir um ataque com o botão direito do mouse.
A direção do ataque será a direção em que o personagem estiver olhando e o ataque formará um arco de 180° para cobrir toda a direção.

##### Empurrão do ataque

Ao receber um ataque, o alvo é empurrado um pouco para trás, a força do empurrão deve variar com a "força" do ataque. Se a "força" foi muito alta, o empurrão deve jogar o alvo a uma distância maior, de forma rápida, e com velocidade linear.

##### Força do ataque

Todo ataque terá um valor de força ao acertar o alvo. A força será a base da força da arma + uma variação definida por qual posição do hitbox do ataque atingiu o alvo. Se for nas pontas, a força adicional será baixa, se for no meio, será alta.

##### Invencibilidade

Ao receber um ataque, o alvo fica com invencibilidade por um curto período.

##### Ondas de inimigos

Cada onda é formada por uma lista de inimigos que serão criados nessa onda.

Os inimigos serão criados na ordem em que aparecem na lista.

Quando a lista estiver vazia e não tiver nenhum inimigo vivo, a onda acaba.

O inimigo deverá ser criado em um "ponto de aparição de inimigos" aleatório no mapa, mas que não seja igual ao último ponto usado.

Se houver 7 inimigos vivos, não será criado outro inimigo até que um morra.

Deve ter um intervalo de pelo menos 3 segundos para aparição de um novo inimigo.


### Estrutura

O jogador enfrentará pequenas ondas de inimigos em arenas fechadas.

A cada três ondas vencidas, a arena muda.

As arenas podem ser nos formatos:
- duas ondas de inimigos + uma onda com apenas um chefe
- três ondas de inimigos

### Vida

Cada inimigo tem uma chance de deixar uma vida caso a vida do jogador não esteja completa.

A chance de deixar uma vida é:

chance_base * ((11 - vida_atual) * 0.1)

chances:
- orc: 20%
- esqueleto: 20%
- chort: 50%
- demônio grande: 100%