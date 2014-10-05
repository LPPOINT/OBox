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

        protected virtual bool ShouldCatchEvents
        {
            get
            {
                return true;
            }
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
                                       info => info.GetCustomAttributes(typeof (LevelEventFilter), true).Any())));
            }
        }

        public virtual void OnLevelReset()
        {
            
        }

        public virtual void OnLevelStarted()
        {
            
        }

        public virtual void OnPauseMenuOpen()
        {
            
        }

        public virtual void OnPauseMenuClosed()
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
            Level.ProcessEvent(e, null);
        }

        public void FireEvent(LevelEvent e, params Type[] targets)
        {
            e.Element = this;
            Level.ProcessEvent(e, targets);
        }

        public void FireEvent(string eventName)
        {
            FireEvent(new LevelEvent(eventName, null));
        }
        public void FireEvent(string eventName, Dictionary<string, object> data)
        {
            FireEvent(new LevelEvent(eventName, data));
        }

        public virtual void OnLevelEvent(LevelEvent e)
        {
            
        }


        private LevelEventFilter GetHandlerAttributeForMethod(MethodInfo m)
        {
            return m.GetCustomAttributes(typeof (LevelEventFilter), true).FirstOrDefault() as LevelEventFilter;
        }

        internal void ProcessEvent(LevelEvent e)
        {

            if(!ShouldCatchEvents)
                return;

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

            var eventType = e.GetType();

            if (eventType == typeof (Level.LevelStateChangedEvent))
            {
                var lsce = e as Level.LevelStateChangedEvent;
                OnLevelStateChanged(lsce.OldState, lsce.NewState);
            }
            else if (eventType == typeof (MapItem.MapItemMoveEvent) && e.IsPlayer)
            {
                var me = e as MapItem.MapItemMoveEvent;
                if(me.State == MapItem.MapItemMoveEvent.MoveState.Started)
                    OnPlayerMoveBegin(me.Element as Player, me.Move);
                else if (me.State == MapItem.MapItemMoveEvent.MoveState.Done)
                    OnPlayerMoveEnd(me.Element as Player, me.Move);
            }
            else if (eventType == typeof (Level.LevelActionEvent))
            {
                var ae = e as Level.LevelActionEvent;
                switch (ae.Type)
                {
                    case Level.LevelActionEvent.LevelActionEventType.LevelStarted:
                        OnLevelStarted();
                        break;
                    case Level.LevelActionEvent.LevelActionEventType.LevelReset:
                        OnLevelReset();
                        break;
                    case Level.LevelActionEvent.LevelActionEventType.LevelEnd:
                        OnLevelEnded();
                        break;
                    case Level.LevelActionEvent.LevelActionEventType.PauseMenuOpen:
                        OnPauseMenuOpen();
                        break;
                    case Level.LevelActionEvent.LevelActionEventType.PauseMenuClosed:
                        OnPauseMenuClosed();
                        break;
                }
            }
        }


    }
}
