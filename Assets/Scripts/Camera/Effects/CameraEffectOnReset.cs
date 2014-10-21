using Assets.Scripts.Levels;
using UnityEngine;

namespace Assets.Scripts.Camera.Effects
{
    public class CameraEffectOnReset : LevelElement
    {
        public float VortexTime = 0.4f;
        public float VortexPower = 1.5f;

        private enum State
        {
            NotStarted,
            Forward,
            Backward
        }

        private State currentState;
        private VortexEffect vortex;

        private void Start()
        {
            currentState = State.NotStarted;

            if (GetComponent<VortexEffect>() != null)
                vortex = GetComponent<VortexEffect>();
            else vortex = gameObject.AddComponent<VortexEffect>();

            vortex.shader = Shader.Find("Hidden/Twist Effect");
            vortex.angle = 50;
            vortex.center = new Vector2(0.5f, 0.5f);
            vortex.radius = new Vector2(0, 0);

        }

        private void StartVortexForward()
        {

            if (currentState == State.NotStarted)
            {
                currentState = State.Forward;
                iTween.ValueTo(gameObject, iTween.Hash("onupdate", "OnITweenResetValueUpdate",
                    "from", 0,
                    "to", VortexPower,
                    "time", VortexTime,
                    "easetype", iTween.EaseType.easeInOutCirc,
                    "oncomplete", "OnITweenResetValueDone",
                    "oncompletetarget", gameObject));
            }

        }

        private void StartVortexBackward()
        {


            if (currentState == State.Forward)
            {
                currentState = State.Backward;
                iTween.ValueTo(gameObject, iTween.Hash("onupdate", "OnITweenResetValueUpdate",
                        "from", VortexPower,
                        "to", 0,
                         "time", VortexTime,
                        "easetype", iTween.EaseType.easeInOutCirc,
                        "oncomplete", "OnITweenResetValueDone",
                        "oncompletetarget", gameObject));
            }
        }

        private void OnITweenResetValueUpdate(float newValue)
        {
            vortex.radius = new Vector2(newValue, newValue);
        }

        private void OnITweenResetValueDone()
        {
            if (currentState == State.Forward)
            {
                StartVortexBackward();
            }
            else if (currentState == State.Backward)
            {
                currentState = State.NotStarted;
            }
        }

        protected override void OnLevelReset()
        {
            if (currentState == State.NotStarted)
            {
                StartVortexForward();
            }
            base.OnLevelReset();
        }
    }
}
