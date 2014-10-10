using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Common
{
    public class ITweenHash
    {

        public ITweenHash()
        {
            hashtable = new Hashtable();
        }

        public ITweenHash(Hashtable hashtable)
        {
            this.hashtable = hashtable;
        }


        private readonly Hashtable hashtable;

        public Hashtable GetHashtable()
        {
            return new Hashtable(hashtable);
        }

        public ITweenHash AddValue(string name, object value)
        {
            if(!hashtable.Contains(name ))
                hashtable.Add(name, value);
            else 
                hashtable[name] = value;

            return this;
        }

        public ITweenHash Time(float time)
        {
            return AddValue("time", time);
        }

        public ITweenHash EaseType(iTween.EaseType easeType)
        {
            return AddValue("easetype", easeType);
        }

        public ITweenHash LoopType(iTween.LoopType loopType)
        {
            return AddValue("looptype", loopType);
        }

        public ITweenHash Position(Vector3 position)
        {
            return AddValue("position", position);
        }

        public ITweenHash Transform(Transform transform)
        {
            return AddValue("transform", transform);
        }

        public ITweenHash Delay(float delay)
        {
            return AddValue("delay", delay);
        }

        public ITweenHash OnComplete(Action onCompliteAction)
        {
            return OnComplete(onCompliteAction.Method.Name);
        }

        public ITweenHash OnComplete(string onCompliteMethod)
        {
            return AddValue("oncomplete", onCompliteMethod);
        }

        public ITweenHash OnCompleteTarget(GameObject target)
        {
            return AddValue("oncompletetarget", target);
        }

        public ITweenHash OnCompleteParams(object args)
        {
            return AddValue("oncompleteparams", args);
        }

        public void Clear()
        {
            hashtable.Clear();
        }

        public void Remove(string key)
        {
            hashtable.Remove(key);
        }

    }
}
