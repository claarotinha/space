# Space Boss — Unity 6000.5.5f1

Shooter 2D vertical com Canvas, ObjectPool da Unity e 9 estados de comportamento do chefão no Animator.

## Abrir o jogo
1. Instale **Unity 6000.5.5f1** pelo Unity Hub.
2. Baixe este repositório em **Code > Download ZIP** e extraia.
3. Abra a pasta extraída pelo Unity Hub com **Add project from disk**.
4. Aguarde o Unity importar os pacotes e compilar.
5. O script `SpaceBossAutoSetup` deve gerar e abrir `Assets/SpaceBoss/_Boot.unity` automaticamente.
6. Se não gerar, use **Tools > Space Boss > Criar cena _Boot**.
7. Abra `Assets/SpaceBoss/_Boot.unity` e pressione Play.

**Importante:** a cena é gerada pelo Editor na primeira abertura. Ela não está versionada como YAML no repositório. Este código não foi testado em uma instalação real da Unity e pode exigir correções conforme o Console.

## Controles
- WASD / setas: movimentação livre
- Espaço: tiro contínuo a cada 0,35 s
- Tentar Novamente: reinicia após vitória ou derrota

## Mecânicas
- Um jogador e um chefão, câmera fixa
- 4 pontos de vida do jogador (perde no quarto dano)
- Chefão com 240 HP, barra de vida e 3 fases
- 9 estados de comportamento no Animator, cada um com StateMachineBehaviour
- 2,25 segundos de invulnerabilidade na mudança de fase
- Pools separadas com UnityEngine.Pool.ObjectPool para os projéteis
- Interface 2D usando Canvas / UGUI

## Se aparecer erro
Abra **Window > General > Console**, copie o primeiro erro vermelho e informe também a versão do Unity e os pacotes instalados.
