# Jogo de Naves — Unity 6000.5.5f1

Projeto 2D com Canvas e um chefão de nove comportamentos.

## Pastas

```text
Assets/
├── Scenes/        _Boot.unity (criada no Editor)
├── Scripts/       Scripts de jogo, chefão e interface
├── Editor/        Ferramentas para gerar ou recuperar a cena
└── Recursos/      Prefab de projétil e Animator (gerados)
```

## Executar

1. Abra o projeto na Unity 6000.5.5f1.
2. Aguarde compilar os scripts e instalar o pacote Unity UI.
3. Use **Ferramentas > Jogo de Naves > Abrir ou recriar _Boot**.
4. Abra `Assets/Scenes/_Boot.unity` e pressione Play.

WASD ou setas movem a nave; Espaço atira.

O projeto usa `UnityEngine.Pool.ObjectPool`, Animator Controller com `StateMachineBehaviour`, 240 HP no chefão e quatro pontos de vida para o jogador (derrota no quarto dano).

**Observação:** a cena é gerada localmente pelo Editor; o funcionamento ainda não foi validado dentro da Unity neste ambiente.
