# Space Boss — Shooter 2D Unity 6

Trabalho acadêmico: shooter vertical com uma navinha e um único chefão.

## Como executar
1. Crie um projeto **2D** no Unity 6000.5.5f1.
2. Copie a pasta `Assets/SpaceBoss` deste repositório para `Assets` do projeto.
3. Aguarde a Unity compilar os scripts.
4. Clique em **Tools > Space Boss > Criar cena _Boot**.
5. Abra `Assets/SpaceBoss/_Boot.unity` e pressione **Play**.

## Controles
- WASD ou setas: mover.
- Espaço: atirar continuamente (0,35 segundo entre tiros).
- Botão Tentar Novamente: reiniciar após vitória ou derrota.

## Implementação
- Interface e objetos visuais com Canvas / UGUI.
- Projéteis do jogador e chefão com `UnityEngine.Pool.ObjectPool<T>`.
- Chefão com 240 HP e três fases: alta (>50%), média (>20% e <=50%) e baixa (<=20%).
- Nove estados reais no Animator Controller, cada um com StateMachineBehaviour (BossBehavior).
- Três padrões por fase: tiro direcionado, leque e onda. Velocidade e frequência aumentam em fases posteriores.
- Alternância de comportamento a cada cinco segundos.
- Invulnerabilidade de 2,25 segundos nas mudanças de fase, sem movimento e sem disparos.
- Jogador começa com 4 pontos de vida, sobrevivendo a 3 danos e perdendo no quarto.
- Câmera fixa, barra de vida, feedback visual, vitória e derrota.

## Observações
A cena e o Animator são **gerados pelo menu do Editor**, não vêm pré-gerados no repositório.
Este código ainda precisa ser validado em uma instalação real do Unity Editor.
Não há assets externos ou pacotes de terceiros.
