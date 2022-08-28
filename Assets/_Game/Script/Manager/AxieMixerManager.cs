using AxieMixer.Unity;
using Newtonsoft.Json.Linq;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class AxieMixerManager : Singleton<AxieMixerManager>
{
    private Dictionary<string, Axie2dBuilderResult> Axie2dBuilderResults;
    public SkeletonDataAsset m_SlimeSkeletonDataAsset;
    public string slimeMoveAnimation;
    public string slimeMeleeAnimation;
    public string slimeRangeAnimation;
    public string slimeDefenceAnimation;
    protected override void Awake()
    {
        base.Awake();
        Mixer.Init();
        Axie2dBuilderResults = new Dictionary<string, Axie2dBuilderResult>();
    }
    public bool HasAxie2dBuilderResult(string axieId)
    {
        return Axie2dBuilderResults.ContainsKey(axieId);
    }
    public Axie2dBuilderResult GetAxie2DBuilderResult(string axieId)
    {
        return Axie2dBuilderResults[axieId];
    }
    public void AddAxie2dBuilderResult(string axieId, Axie2dBuilderResult axie2DBuilderResult)
    {
        if (Axie2dBuilderResults.ContainsKey(axieId)) return;
        Axie2dBuilderResults.Add(axieId, axie2DBuilderResult);
    }

}
