using Photon.Deterministic;
using Quantum;
using System.Collections.Generic;
using UnityEngine;

namespace ARPG.Common
{
    public static class Extensions
    {
        #if UNITY_EDITOR

        // Taken from the Photon manual: https://doc.photonengine.com/quantum/current/concepts-and-patterns/animation-curves-baking
        public static FPAnimationCurve EDITOR_ToFPCurve(this AnimationCurve unityCurve)
        {
            var unityKeys = unityCurve.keys;
            var unityStartTime = 0.0f;
            var unityEndTime = 0.0f;
            if (unityKeys.Length > 0)
            {
                unityStartTime = unityCurve.keys[0].time;
                unityEndTime = unityCurve.keys[^1].time;
            }

            var fpCurve = new FPAnimationCurve();
            fpCurve.Keys = new FPAnimationCurve.Keyframe[unityKeys.Length];
            fpCurve.Resolution = 32;
            fpCurve.StartTime = FP.FromFloat_UNSAFE(unityStartTime);
            fpCurve.EndTime = FP.FromFloat_UNSAFE(unityEndTime);
            fpCurve.PreWrapMode = (int)unityCurve.preWrapMode;
            fpCurve.PostWrapMode = (int)unityCurve.postWrapMode;
            for (var i = 0; i < unityKeys.Length; i++)
            {
                fpCurve.Keys[i].Time = FP.FromFloat_UNSAFE(unityKeys[i].time);
                fpCurve.Keys[i].Value = FP.FromFloat_UNSAFE(unityKeys[i].value);

                if (float.IsInfinity(unityKeys[i].inTangent) == false)
                    fpCurve.Keys[i].InTangent = FP.FromFloat_UNSAFE(unityKeys[i].inTangent);
                else fpCurve.Keys[i].InTangent = FP.SmallestNonZero;

                if (float.IsInfinity(unityKeys[i].outTangent) == false)
                    fpCurve.Keys[i].OutTangent = FP.FromFloat_UNSAFE(unityKeys[i].outTangent);
                else fpCurve.Keys[i].OutTangent = FP.SmallestNonZero;

                fpCurve.Keys[i].TangentModeLeft = (byte)UnityEditor.AnimationUtility.GetKeyLeftTangentMode(unityCurve, i);
                fpCurve.Keys[i].TangentModeRight = (byte)UnityEditor.AnimationUtility.GetKeyRightTangentMode(unityCurve, i);
            }

            fpCurve.Samples = new FP[fpCurve.Resolution + 1];
            var deltaTime = (unityEndTime - unityStartTime) / fpCurve.Resolution;
            for (int i = 0; i < fpCurve.Samples.Length; i++)
            {
                var time = unityStartTime + deltaTime * i;
                var fp = FP.FromFloat_UNSAFE(unityCurve.Evaluate(time));
                fpCurve.Samples[i].RawValue = fp.RawValue;
            }

            return fpCurve;
        }

        #endif

        public static TValue? GetOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key) where TValue : struct
        {
            if (dictionary.TryGetValue(key, out var value))
                return value;

            return null;
        }

        public static bool TryFindChild(this Transform transform, string path, out Transform match)
        {
            var splitPath = path.Split('.');
            var current = transform;

            for (var i = 0; i < splitPath.Length; i++)
            {
                current = current.Find(splitPath[i]);
                if (current == null)
                {
                    match = null;
                    return false;
                }
            }

            match = current;
            return true;
        }
    }
}
