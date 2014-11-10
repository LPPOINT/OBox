using System;
using UnityEngine;

using Holoville.HOTween;
using Holoville.HOTween.Plugins.Core;

/// <summary>
/// TODO: figure out using Play elegantly
/// </summary>
public class AMPlugAnimation : ABSTweenPlugin {
    Animation anim;
    AnimationState animState;
    WrapMode wrap;
    bool fadeIn;
    float fadeInTime;

    protected override object startVal { get { return _startVal; } set { _startVal = value; } }

    protected override object endVal { get { return _endVal; } set { _endVal = value; } }

    public AMPlugAnimation(Animation aAnim, string clipName, WrapMode aWrap, bool aFadeIn, float aFadeInTime)
        : base(null, false) {
        ignoreAccessor = true;
        anim = aAnim;
        animState = anim[clipName];
        wrap = aWrap;
        fadeIn = aFadeIn;
        fadeInTime = aFadeInTime;
    }

    protected override float GetSpeedBasedDuration(float p_speed) {
        return p_speed;
    }

    protected override void SetChangeVal() {
    }

    protected override void SetIncremental(int p_diffIncr) { }
    protected override void SetIncrementalRestart() { }

    protected override void DoUpdate(float p_totElapsed) {
        animState.enabled = true;
        animState.wrapMode = wrap;
        animState.time = p_totElapsed;

        if(fadeIn && p_totElapsed < fadeInTime)
            animState.weight = p_totElapsed/fadeInTime;
        else
            animState.weight = 1.0f;

        anim.Sample();
        animState.enabled = false;
    }

    protected override void SetValue(object p_value) { }
    protected override object GetValue() { return null; }
}

/// <summary>
/// TODO: figure out using Play elegantly
/// </summary>
public class AMPlugAnimationCrossFade : ABSTweenPlugin {
    Animation anim;

    AnimationState animState;
    WrapMode wrap;
    float startTime;

    AnimationState prevAnimState;
    WrapMode prevWrap;
    float prevStartTime;

    float crossFadeTime;

    protected override object startVal { get { return _startVal; } set { _startVal = value; } }

    protected override object endVal { get { return _endVal; } set { _endVal = value; } }

    public AMPlugAnimationCrossFade(Animation aAnim, float aCrossFadeTime, string prevClip, WrapMode aPrevWrap, float aPrevStartTime, string clip, WrapMode aWrap, float aStartTime)
        : base(null, false) {
        ignoreAccessor = true;

        anim = aAnim;

        prevAnimState = anim[prevClip];
        prevWrap = aPrevWrap;
        prevStartTime = aPrevStartTime;

        animState = anim[clip];
        wrap = aWrap;
        startTime = aStartTime;

        crossFadeTime = aCrossFadeTime;
    }

    protected override float GetSpeedBasedDuration(float p_speed) {
        return p_speed;
    }

    protected override void SetChangeVal() {
    }

    protected override void SetIncremental(int p_diffIncr) { }
    protected override void SetIncrementalRestart() { }

    protected override void DoUpdate(float p_totElapsed) {
        if(p_totElapsed < crossFadeTime) {
            float weight = p_totElapsed / crossFadeTime;

            prevAnimState.enabled = true;
            prevAnimState.wrapMode = prevWrap;
            prevAnimState.weight = 1.0f - weight;
            prevAnimState.time = (startTime + p_totElapsed) - prevStartTime;

            animState.enabled = true;
            animState.wrapMode = wrap;
            animState.weight = weight;
            animState.time = p_totElapsed;

            anim.Sample();

            prevAnimState.enabled = false;
            animState.enabled = false;
        }
        else {
            animState.enabled = true;
            animState.wrapMode = wrap;
            animState.weight = 1.0f;
            animState.time = p_totElapsed;

            anim.Sample();

            animState.enabled = false;
        }
    }

    protected override void SetValue(object p_value) { }
    protected override object GetValue() { return null; }
}

//TODO: PreviewFrame isn't quite ideal, but it's the most guaranteed way to synchronize play
//Also, no crossfade, which is a whole can of worms to deal with in the actual system
public class AMPlugMateAnimator : ABSTweenPlugin {
    public enum LoopType {
        None,
        Restart,
        Yoyo
    }

    AMITarget parent;
    AnimatorData anim;
    AMTakeData take;
    LoopType loopMode;
    int endFrame;
    bool started;

    float prevFrame;

    protected override object startVal { get { return _startVal; } set { _startVal = value; } }

    protected override object endVal { get { return _endVal; } set { _endVal = value; } }

    public AMPlugMateAnimator(AMITarget aParent, AnimatorData aAnim, AMTakeData aTake, LoopType aLoopMode)
        : base(null, false) {
        ignoreAccessor = true;
        parent = aParent;
        anim = aAnim;
        take = aTake;
        loopMode = aLoopMode;
        endFrame = take.getLastFrame();
    }

    protected override float GetSpeedBasedDuration(float p_speed) {
        return p_speed;
    }

    protected override void SetChangeVal() {
        started = false;
    }

    protected override void SetIncremental(int p_diffIncr) { }
    protected override void SetIncrementalRestart() { }

    protected override void DoUpdate(float p_totElapsed) {
        if(!started) { started = true; prevFrame = 0;  return; } //TODO: for some reason DoUpdate is called with end elapse during startup

        float frame = p_totElapsed*take.frameRate;

        switch(loopMode) {
            case LoopType.Restart:
                frame %= (float)endFrame;
                break;
            case LoopType.Yoyo:
                int count = Mathf.FloorToInt(frame)/endFrame;
                if(count % 2 == 0)
                    frame %= (float)endFrame;
                else {
                    float flf = (float)endFrame;
                    frame = flf - (frame%endFrame);
                }
                break;
        }

        //only play audio when moving forward
        //remove any audio loops if we reached the end
        take.runFrame(anim, frame, parent.TargetAnimScale(), frame > prevFrame, (tweenObj.isReversed && p_totElapsed <= 0.0f) || (p_totElapsed >= _duration));
        prevFrame = frame;
    }

    protected override void SetValue(object p_value) { }
    protected override object GetValue() { return null; }
}

public class AMPlugDouble : ABSTweenPlugin {
    internal static Type[] validPropTypes = { typeof(double) };
    internal static Type[] validValueTypes = { typeof(double) };

    double typedStartVal;
    double typedEndVal;
    double changeVal;

    protected override object startVal {
        get {
            return _startVal;
        }
        set {
            if(tweenObj.isFrom && isRelative) {
                _startVal = typedStartVal = typedEndVal + Convert.ToDouble(value);
            }
            else {
                _startVal = typedStartVal = Convert.ToDouble(value);
            }
        }
    }

    protected override object endVal {
        get {
            return _endVal;
        }
        set {
            _endVal = typedEndVal = Convert.ToDouble(value);
        }
    }

    public AMPlugDouble(double p_endVal)
        : base(p_endVal, false) {
    }

    public AMPlugDouble(double p_endVal, EaseType p_easeType)
        : base(p_endVal, p_easeType, false) {
    }

    public AMPlugDouble(double p_endVal, bool p_isRelative)
        : base(p_endVal, p_isRelative) {
    }

    public AMPlugDouble(double p_endVal, EaseType p_easeType, bool p_isRelative)
        : base(p_endVal, p_easeType, p_isRelative) {
    }

    public AMPlugDouble(double p_endVal, AnimationCurve p_easeAnimCurve, bool p_isRelative)
        : base(p_endVal, p_easeAnimCurve, p_isRelative) { }

    protected override float GetSpeedBasedDuration(float p_speed) {
        float speedDur = Convert.ToSingle(changeVal / (double)p_speed);
        if(speedDur < 0) {
            speedDur = -speedDur;
        }
        return speedDur;
    }

    protected override void SetChangeVal() {
        if(isRelative && !tweenObj.isFrom) {
            changeVal = typedEndVal;
        }
        else {
            changeVal = typedEndVal - typedStartVal;
        }
    }

    protected override void SetIncremental(int p_diffIncr) {
        typedStartVal += changeVal * p_diffIncr;
    }
    protected override void SetIncrementalRestart() { }

    protected override void DoUpdate(float p_totElapsed) {
        float t = ease(p_totElapsed, 0.0f, 1.0f, _duration, tweenObj.easeOvershootOrAmplitude, tweenObj.easePeriod);
        SetValue(typedStartVal + ((double)t) * (typedEndVal - typedStartVal));
    }
}

public class AMPlugLong : ABSTweenPlugin {
    internal static Type[] validPropTypes = { typeof(long) };
    internal static Type[] validValueTypes = { typeof(long) };

    double typedStartVal;
    double typedEndVal;
    double changeVal;

    protected override object startVal {
        get {
            return _startVal;
        }
        set {
            if(tweenObj.isFrom && isRelative) {
                _startVal = typedStartVal = typedEndVal + Convert.ToDouble(value);
            }
            else {
                _startVal = typedStartVal = Convert.ToDouble(value);
            }
        }
    }

    protected override object endVal {
        get {
            return _endVal;
        }
        set {
            _endVal = typedEndVal = Convert.ToDouble(value);
        }
    }

    public AMPlugLong(long p_endVal)
        : base(p_endVal, false) {
    }

    public AMPlugLong(long p_endVal, EaseType p_easeType)
        : base(p_endVal, p_easeType, false) {
    }

    public AMPlugLong(long p_endVal, bool p_isRelative)
        : base(p_endVal, p_isRelative) {
    }

    public AMPlugLong(long p_endVal, EaseType p_easeType, bool p_isRelative)
        : base(p_endVal, p_easeType, p_isRelative) {
    }

    public AMPlugLong(long p_endVal, AnimationCurve p_easeAnimCurve, bool p_isRelative)
        : base(p_endVal, p_easeAnimCurve, p_isRelative) { }

    protected override float GetSpeedBasedDuration(float p_speed) {
        float speedDur = Convert.ToSingle(changeVal / (double)p_speed);
        if(speedDur < 0) {
            speedDur = -speedDur;
        }
        return speedDur;
    }

    protected override void SetChangeVal() {
        if(isRelative && !tweenObj.isFrom) {
            changeVal = typedEndVal;
        }
        else {
            changeVal = typedEndVal - typedStartVal;
        }
    }

    protected override void SetIncremental(int p_diffIncr) {
        typedStartVal += changeVal * p_diffIncr;
    }
    protected override void SetIncrementalRestart() { }

    protected override void DoUpdate(float p_totElapsed) {
        float t = ease(p_totElapsed, 0.0f, 1.0f, _duration, tweenObj.easeOvershootOrAmplitude, tweenObj.easePeriod);
        SetValue(Convert.ToInt64(typedStartVal + ((double)t) * (typedEndVal - typedStartVal)));
    }
}