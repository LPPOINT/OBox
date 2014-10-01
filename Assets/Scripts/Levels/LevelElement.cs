using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Assets.Scripts.Map;
using Assets.Scripts.Map.Items;
using UnityEngine;

namespace Assets.Scripts.Levels
{
    public class LevelElement : MonoBehaviour
    {

        public Level Level
        {
            get { return Level.Current; }
        }

        private List<MethodInfo> eventFilters;
        private IEnumerable<MethodInfo> EventFilters
        {
            get {
                return eventFilters ??
                       (eventFilters =
                           new List<MethodInfo>(
                               GetType()
                                   .GetMethods()
                                   .Where(
                                       info => info.GetCustomAttributes(typeof (LevelEventHandlerAttribute), true).Any())));
            }
        }

        public virtual void OnLevelReset()
        {
            
        }

        public virtual void OnLevelStarted()
        {
            
        }

        public virtual void OnMenuOpen()
        {
            
        }

        public virtual void OnMenuClosed()
        {
            
        }

        public virtual void OnLevelEnded()
        {
            
        }

        public virtual void OnPlayerMoveBegin(Player player, MapItemMove move)
        {
            
        }

        public virtual void OnPlayerMoveEnd(Player player, MapItemMove move)
        {
            
        }

        public virtual void OnLevelStateChanged(LevelState oldState, LevelState newState)
        {
            
        }

        public void FireEvent(LevelEvent e)
        {
            e.Element = this;
        }

        public virtual void OnLevelEvent(LevelEvent e)
        {
            
        }


        private LevelEventHandlerAttribute GetHandlerAttributeForMethod(MethodInfo m)
        {
            return m.GetCustomAttributes(typeof (LevelEventHandlerAttribute), true).FirstOrDefault() as LevelEventHandlerAttribute;
        }

        internal void ProcessEvent(LevelEvent e)
        {

            var filter = EventFilters.FirstOrDefault(info =>
                                                     {
                                                         var attribute = GetHandlerAttributeForMethod(info);
                                                         return attribute.EventType == e.GetType() &&
                                                                (attribute.CustomSenderType == null ||
                                                                 attribute.CustomSenderType == e.Element.GetType());
                                                     });


            if (filter == null)
            {
                OnLevelEvent(e);
            }
            else
            {
                filter.Invoke(this, new object[]{e});
                var a = GetHandlerAttributeForMethod(filter);
                if (a.ShouldProcessInMainHandler)
                {
                    OnLevelEvent(e);
                }
            }
        }


    }
}
