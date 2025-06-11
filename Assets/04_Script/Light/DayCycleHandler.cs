using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UIElements;
#endif


    /// <summary>
    /// Handle the cycle of Day and Night. Everything that need to change across time will register itself to this handler
    /// which will update it when it update (e.g. ShadowInstance, Interpolator etc.).
    /// The ticking of that system can be stopped, this is useful e.g. if the game is put in pause (or need to do cutscene
    /// etc..)
    /// </summary>
    [DefaultExecutionOrder(10)]
    public class DayCycleHandler : MonoBehaviour
    {
        public Transform LightsRoot;
        
        [Header("Day Light")]
        public Light2D DayLight;
        public Gradient DayLightGradient;

        [Header("Night Light")] 
        public Light2D NightLight;
        public Gradient NightLightGradient;

        [Header("Ambient Light")] 
        public Light2D AmbientLight;
        public Gradient AmbientLightGradient;

        [Header("RimLights")] 
        public Light2D SunRimLight;
        public Gradient SunRimLightGradient;
        public Light2D MoonRimLight;
        public Gradient MoonRimLightGradient;

        public void UpdateLight(float ratio)
        {
            DayLight.color = DayLightGradient.Evaluate(ratio);
            NightLight.color = NightLightGradient.Evaluate(ratio);

#if UNITY_EDITOR
            if(AmbientLight != null)
#endif
                AmbientLight.color = AmbientLightGradient.Evaluate(ratio);

#if UNITY_EDITOR
            if(SunRimLight != null)
#endif
                SunRimLight.color = SunRimLightGradient.Evaluate(ratio);
            
#if UNITY_EDITOR
            if(MoonRimLight != null)
#endif
                MoonRimLight.color = MoonRimLightGradient.Evaluate(ratio);
            
            LightsRoot.rotation = Quaternion.Euler(0,0, 360.0f * ratio);
        }
    }

    
    
#if UNITY_EDITOR
    [CustomEditor(typeof(DayCycleHandler))]
    class DayCycleEditor : Editor
    {
        private DayCycleHandler m_Target;

        public override VisualElement CreateInspectorGUI()
        {
            m_Target = target as DayCycleHandler;

            var root = new VisualElement();
            
            InspectorElement.FillDefaultInspector(root, serializedObject, this);
            
            var slider = new Slider(0.0f, 1.0f);
            slider.label = "Time";
            slider.RegisterValueChangedCallback(evt =>
            {
                m_Target.UpdateLight(evt.newValue);
                SceneView.RepaintAll();
            });
            
            //registering click event, it's very catch all but not way to do a change check for control change
            root.RegisterCallback<ClickEvent>(evt =>
            {
                m_Target.UpdateLight(slider.value);
                SceneView.RepaintAll();
            });
            
            root.Add(slider);

            return root;
        }
    }
#endif

