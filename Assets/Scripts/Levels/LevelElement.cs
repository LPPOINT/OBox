using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Assets.Scripts.Map;
using Assets.Scripts.Map.Items;
using Assets.Scripts.UI;
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

        /// <summary>
        /// Вызывается при перезагрузке уровня
        /// </summary>
        protected virtual void OnLevelReset()
        {
            
        }

        /// <summary>
        /// Вызывается при начале уровня, после проигрывания начальных декораций
        /// </summary>
        protected virtual void OnLevelStarted()
        {
            
        }

        /// <summary>
        /// Вызывается при открытии меню паузы, когда экземпляр <see cref="MenuUI"/> уже создан и отображён.
        /// </summary>
        public virtual void OnPauseMenuOpen()
        {
            
        }

        /// <summary>
        /// Вызывается при закрытии меню паузы, сразу после удаления объекта <see cref="MenuUI"/> со сцены
        /// </summary>
        protected virtual void OnPauseMenuClosed()
        {
            
        }

        /// <summary>
        /// Вызывается при завершении миссии, сразу после проигрывания конечных декораций и до открытия экрана результатов уровня
        /// </summary>
        protected virtual void OnLevelEnded()
        {
            
        }

        /// <summary>
        /// Вызывается, когда игрок начал движение
        /// </summary>
        /// <param name="player"></param>
        /// <param name="move"></param>
        protected virtual void OnPlayerMoveBegin(Player player, MapItemMove move)
        {
            
        }

        /// <summary>
        /// Вызывается, когда игрок закончил движение
        /// </summary>
        /// <param name="player"></param>
        /// <param name="move"></param>
        protected virtual void OnPlayerMoveEnd(Player player, MapItemMove move)
        {
            
        }

        /// <summary>
        /// Вызывается, когда уровень изменил состояние
        /// </summary>
        /// <param name="oldState"></param>
        /// <param name="newState"></param>
        protected virtual void OnLevelStateChanged(LevelState oldState, LevelState newState)
        {
            
        }

        /// <summary>
        /// Вызывается в конце метода Start()
        /// </summary>
        protected virtual void OnLevelInitialized()
        {
            
        }

        /// <summary>
        /// Вызывается когда пройзошло какое - либо из действий, описанных в <see cref="Levels.Level.LevelActionEvent.LevelActionType"/>
        /// </summary>
        /// <param name="actionType"></param>
        protected virtual void OnLevelAction(Level.LevelActionEvent.LevelActionType actionType)
        {
            
        }

        protected void FireEvent(LevelEvent e)
        {
            e.Element = this;
            Level.ProcessEvent(e, null);
        }

        protected void FireEvent(LevelEvent e, params Type[] targets)
        {
            e.Element = this;
            Level.ProcessEvent(e, targets);
        }

        protected void FireEvent(string eventName)
        {
            FireEvent(new LevelEvent(eventName, null));
        }
        protected void FireEvent(string eventName, Dictionary<string, object> data)
        {
            FireEvent(new LevelEvent(eventName, data));
        }

        /// <summary>
        /// Вызывается когда какой - либо элемент уровня отправил событие
        /// </summary>
        /// <param name="e"></param>
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
                    case Level.LevelActionEvent.LevelActionType.LevelInitialized:
                        OnLevelInitialized();
                        break;
                    case Level.LevelActionEvent.LevelActionType.LevelStarted:
                        OnLevelStarted();
                        break;
                    case Level.LevelActionEvent.LevelActionType.LevelReset:
                        OnLevelReset();
                        break;
                    case Level.LevelActionEvent.LevelActionType.LevelEnd:
                        OnLevelEnded();
                        break;
                    case Level.LevelActionEvent.LevelActionType.PauseMenuOpen:
                        OnPauseMenuOpen();
                        break;
                    case Level.LevelActionEvent.LevelActionType.PauseMenuClosed:
                        OnPauseMenuClosed();
                        break;
                }
                OnLevelAction(ae.Type);
            }
            
        }


    }
}
