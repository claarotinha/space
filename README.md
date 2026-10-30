# Jogo de Naves — Space Boss

Projeto de shooter 2D para Unity **6000.5.5f1**.

## Organização das pastas

```text
Assets/
├── Scenes/
│   └── _Boot.unity          (gerada ao abrir o Editor)
└── SpaceBoss/
    ├── Scripts/
    │   ├── SpaceBossGame.cs
    │   ├── BossBehavior.cs
    │   └── SpaceBossRestartButton.cs
    ├── 02_Editor/
    │   └── Editor/
    │       ├── CriadorDeCenas.cs
    │       └── RecuperarCenas.cs
    └── 03_RecursosGerados/  (criada automaticamente)
        ├── Boss.controller
        └── Projectile.prefab
```

Os scripts principais mantêm seus nomes técnicos originais para não quebrar referências da Unity. Os nomes visíveis na cena e os menus do gerador foram traduzidos para português.

## Como abrir

1. Abra a pasta do repositório no Unity Hub usando a versão 6000.5.5f1.
2. Espere a importação dos pacotes e a compilação.
3. Se a cena não abrir automaticamente, use **Ferramentas > Jogo de Naves > Abrir ou recriar _Boot**.
4. Abra `Assets/Scenes/_Boot.unity` e clique em Play.

## Controles

- **WASD / Setas:** movimentar a nave.
- **Espaço:** disparar continuamente.
- **Tentar Novamente:** reiniciar após vitória ou derrota.

## Requisitos

- Câmera fixa, Canvas e UGUI.
- Jogador resiste a 3 danos e perde no quarto.
- Chefão com 240 HP e barra de vida.
- Nove estados no Animator, com StateMachineBehaviour.
- Mudanças de fase com 2,25 s de invulnerabilidade.
- ObjectPool da Unity para tiros do jogador e do chefão.

**Atenção:** a cena e os recursos são gerados no Editor, não estão versionados como arquivos `.unity` prontos. Ainda não houve teste executado no Editor Unity neste ambiente.
