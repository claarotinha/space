# Jogo de Naves — Unity 6000.5.5f1

Tudo fica em **Assets/JogoDeNaves**:

```text
Assets/
└── JogoDeNaves/
    ├── _Boot.unity                  (cena gerada no Editor)
    ├── SpaceBossGame.cs             (jogador, tiros e vida)
    ├── BossBehavior.cs              (9 comportamentos)
    ├── SpaceBossRestartButton.cs    (reiniciar)
    ├── Boss.controller              (gerado no Editor)
    ├── Projectile.prefab            (gerado no Editor)
    └── Editor/
        ├── CriadorDeCenas.cs
        └── RecuperarCenas.cs
```

A pasta Editor é necessária para a Unity separar scripts de edição dos scripts de jogo.

## Como abrir

1. Abra o projeto na Unity 6000.5.5f1 e espere a compilação.
2. Use **Ferramentas > Jogo de Naves > Criar cena _Boot** para aplicar as alterações de tela e vida, mesmo que exista uma cena antiga.
3. Abra `Assets/JogoDeNaves/_Boot.unity` e clique em Play.

Controles: **WASD/setas** para mover e **Espaço** para atirar.

Chefão: **60 HP** e três fases, nove comportamentos no Animator. Jogador perde no quarto dano. Os tiros usam `UnityEngine.Pool.ObjectPool`.

A tela usa Canvas ajustado à tela e Canvas Scaler em modo **Expand**, para preservar o enquadramento completo em diferentes proporções de janela.

A cena é gerada pelo Editor; não foi possível executar a Unity neste ambiente para validar visualmente.
