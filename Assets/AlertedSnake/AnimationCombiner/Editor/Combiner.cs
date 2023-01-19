/*

Animation Combiner - a simple Unity script to combine animation clips

Copyright (c) 2023 Michael Stella

*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Animations;

namespace AlertedSnake.AnimationCombiner
{

    [ExecuteInEditMode]
    public class Combiner
    {

        private AnimationClip _targetAnimation;
        private List<AnimationClip> _sourceClips;

        public Combiner() {
            _sourceClips = new List<AnimationClip>();
            Undo.undoRedoPerformed += AssetDatabase.SaveAssets;
        }

        // setters
        public void setTarget(AnimationClip target) {
            _targetAnimation = target;
        }
        public void setSources(List<AnimationClip> sources) {
            _sourceClips = sources;
        }

        private void combineClip(AnimationClip sourceClip) {
            if (sourceClip == null) {
                return;
            }

            Debug.Log("Found source clip " + sourceClip.name);

            foreach (var binding in AnimationUtility.GetObjectReferenceCurveBindings(sourceClip)) {
                Debug.Log("Found animated object " + binding.path + "/" + binding.propertyName);
                AnimationUtility.SetObjectReferenceCurve(
                    _targetAnimation, binding, AnimationUtility.GetObjectReferenceCurve(sourceClip, binding));
            }

            foreach (var binding in AnimationUtility.GetCurveBindings(sourceClip)) {
                Debug.Log("Found animated float " + binding.path + "/" + binding.propertyName);
                AnimationUtility.SetEditorCurve(
                    _targetAnimation, binding, AnimationUtility.GetEditorCurve(sourceClip, binding));
            }

            //SetAnimationEvents(_targetAnimation, sourceClip.events);
        }


        public void Combine() {
            if (_sourceClips.Count <= 0) {
                Debug.LogWarning("No source clips provided, aborting.");
                return;
            }

            Undo.IncrementCurrentGroup();
            Undo.SetCurrentGroupName("Animation Combiner");
            int undoGroupIndex = Undo.GetCurrentGroup();

            Undo.RegisterCompleteObjectUndo(_targetAnimation, "target");

            foreach (var clip in _sourceClips) {
                combineClip(clip);
            }

            AssetDatabase.SaveAssets();
            Undo.CollapseUndoOperations(undoGroupIndex);
        }
    }
}