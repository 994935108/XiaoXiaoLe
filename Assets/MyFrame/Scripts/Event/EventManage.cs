using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManage : SingletonGameObject<EventManage> {
        private Dictionary<string, UnityAction<object>> eventDir = new Dictionary<string, UnityAction<object>>();
        public void AddEventListener(string eventName,UnityAction<object> action) {
           
            if (eventDir.ContainsKey(eventName))
            {
                //如果已经包含该事件则直接添加事件
                eventDir[eventName] += action;
            }
            else {
                eventDir.Add(eventName, action);
                if (eventName.Equals("AddPanel")) {

                }
            }
        }
        public void RemoveEventListener(string eventName, UnityAction<object> action)
        {
            if (eventDir.ContainsKey(eventName))
            {
                //如果已经包含该事件则直接移除该事件
                  eventDir[eventName] -= action;
            }
        }
        public void OnTriggerEvent(string eventName, object info) {
            if (eventDir.ContainsKey(eventName)&& eventDir[eventName]!=null)
            {
               
                eventDir[eventName].Invoke(info);
            }
          
        }
      
        public void Clear() {
           
            eventDir.Clear();
        }
    }

