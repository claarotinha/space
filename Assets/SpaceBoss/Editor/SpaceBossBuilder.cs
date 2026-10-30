using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem.UI;
#endif
using System.IO;

public static class SpaceBossBuilder
{
    const string Root="Assets/SpaceBoss";
    [MenuItem("Tools/Space Boss/Criar cena _Boot")]
    public static void Build()
    {
        Directory.CreateDirectory(Root+"/Generated");
        if (AssetDatabase.LoadAssetAtPath<AnimatorController>(Root+"/Generated/Boss.controller") != null)
            AssetDatabase.DeleteAsset(Root+"/Generated/Boss.controller");
        var scene=EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Single);
        var camera=new GameObject("Main Camera").AddComponent<Camera>();
        camera.tag="MainCamera"; camera.orthographic=true; camera.backgroundColor=new Color(.025f,.04f,.1f);
        camera.clearFlags=CameraClearFlags.SolidColor; camera.transform.position=new Vector3(0,0,-10);
        var canvas=new GameObject("Canvas",typeof(Canvas),typeof(CanvasScaler),typeof(GraphicRaycaster));
        canvas.GetComponent<Canvas>().renderMode=RenderMode.ScreenSpaceOverlay;
        var scaler=canvas.GetComponent<CanvasScaler>(); scaler.uiScaleMode=CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution=new Vector2(1000,700); scaler.screenMatchMode=CanvasScaler.ScreenMatchMode.MatchWidthOrHeight; scaler.matchWidthOrHeight=.5f;
        var field=Panel("Campo",canvas.transform,new Vector2(1000,700),Vector2.zero,new Color(.03f,.06f,.14f));
        var player=Panel("Navinha",field,new Vector2(42,42),new Vector2(0,-245),new Color(.25f,.75f,1f));
        var boss=Panel("Chefao",field,new Vector2(78,78),new Vector2(0,230),Color.red);
        var barBg=Panel("Barra de vida",field,new Vector2(600,20),new Vector2(0,313),new Color(.2f,.2f,.25f));
        var bar=Panel("Vida atual",barBg,new Vector2(600,20),Vector2.zero,Color.green);
        var barImage=bar.GetComponent<Image>(); barImage.type=Image.Type.Filled; barImage.fillMethod=Image.FillMethod.Horizontal;
        TextLabel("VIDAS: 4",field,new Vector2(-410,295),new Vector2(170,32),26);
        var lives=field.GetChild(field.childCount-1).GetComponent<Text>();
        var status=TextLabel("WASD / SETAS - MOVER   |   ESPACO - ATIRAR",field,new Vector2(0,-320),new Vector2(800,35),21);
        var result=Panel("Resultado",field,new Vector2(470,240),Vector2.zero,new Color(.08f,.1f,.2f,.96f));
        var resultText=TextLabel("VITORIA!",result, new Vector2(0,50),new Vector2(420,70),40);
        var buttonRect=Panel("Botao Reiniciar",result,new Vector2(240,60),new Vector2(0,-55),new Color(.2f,.45f,.7f));
        TextLabel("TENTAR NOVAMENTE",buttonRect,Vector2.zero,new Vector2(230,50),22);
        var button=buttonRect.gameObject.AddComponent<Button>();
        buttonRect.gameObject.AddComponent<SpaceBossRestartButton>();
        var shot=Panel("Projetil Prefab",field,new Vector2(10,22),new Vector2(0,-900),Color.cyan);
        string prefabPath=Root+"/Generated/Projectile.prefab";
        var prefab=PrefabUtility.SaveAsPrefabAsset(shot.gameObject,prefabPath);
        Object.DestroyImmediate(shot.gameObject);
        var go=new GameObject("GameManager");
        var game=go.AddComponent<SpaceBossGame>();
        game.field=field; game.player=player; game.boss=boss;
        game.playerImage=player.GetComponent<Image>(); game.bossImage=boss.GetComponent<Image>();
        game.bossHealthFill=barImage; game.livesText=lives; game.statusText=status;
        game.resultPanel=result.gameObject; game.resultText=resultText;
        game.projectilePrefab=prefab.GetComponent<RectTransform>();
        var events = new GameObject("EventSystem", typeof(EventSystem));
#if ENABLE_INPUT_SYSTEM
        events.AddComponent<InputSystemUIInputModule>();
#else
        events.AddComponent<StandaloneInputModule>();
#endif
        var controller=AnimatorController.CreateAnimatorControllerAtPath(Root+"/Generated/Boss.controller");
        controller.AddParameter("Behavior",AnimatorControllerParameterType.Int);
        var sm=controller.layers[0].stateMachine;
        var states=new AnimatorState[10];
        string[] names={"Alta - Mira","Alta - Leque","Alta - Onda","Media - Mira","Media - Leque","Media - Onda","Baixa - Mira","Baixa - Leque","Baixa - Onda","Transicao invulneravel"};
        for(int i=0;i<10;i++) {
            states[i]=sm.AddState(names[i]);
            states[i].writeDefaultValues=false;
            if(i<9) { var b=states[i].AddStateMachineBehaviour<BossBehavior>(); b.index=i; }
        }
        sm.defaultState=states[0];
        for(int i=0;i<10;i++) {
            var tr=sm.AddAnyStateTransition(states[i]);
            tr.hasExitTime=false; tr.duration=0; tr.canTransitionToSelf=false;
            tr.AddCondition(AnimatorConditionMode.Equals,i,"Behavior");
        }
        game.bossAnimator=go.AddComponent<Animator>();
        game.bossAnimator.runtimeAnimatorController=controller;
        EditorSceneManager.SaveScene(scene,Root+"/_Boot.unity");
        EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(Root+"/_Boot.unity", true) };
        AssetDatabase.SaveAssets(); AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("Space Boss","Cena _Boot criada! Abra Assets/SpaceBoss/_Boot.unity e aperte Play.","OK");
    }
    static RectTransform Panel(string name,Transform parent,Vector2 size,Vector2 pos,Color color)
    {
        var g=new GameObject(name,typeof(RectTransform),typeof(CanvasRenderer),typeof(Image));
        g.transform.SetParent(parent,false);
        var r=g.GetComponent<RectTransform>(); r.sizeDelta=size; r.anchoredPosition=pos;
        g.GetComponent<Image>().color=color;
        return r;
    }
    static Text TextLabel(string content,Transform parent,Vector2 pos,Vector2 size,int fontSize)
    {
        var g=new GameObject("Texto",typeof(RectTransform),typeof(CanvasRenderer),typeof(Text));
        g.transform.SetParent(parent,false);
        var r=g.GetComponent<RectTransform>(); r.sizeDelta=size; r.anchoredPosition=pos;
        var t=g.GetComponent<Text>(); t.text=content; t.fontSize=fontSize; t.color=Color.white;
        t.alignment=TextAnchor.MiddleCenter;
        t.font=Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        return t;
    }
}
