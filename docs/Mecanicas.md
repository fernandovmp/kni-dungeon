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

### Estrutura

O jogador enfrentará pequenas ondas de inimigos em arenas fechadas.

A cada três ondas vencidas, a arena muda.

As ondas podem ser nos formatos:
- duas ondas de inimigos + uma onda com apenas um chefe
- três ondas de inimigos

