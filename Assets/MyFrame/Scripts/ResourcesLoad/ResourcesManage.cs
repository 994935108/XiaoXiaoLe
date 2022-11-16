
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

    public class ResourcesManage : SingletonGameObject<ResourcesManage>
    {
        /// <summary>
        /// 同步加载资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public T Load<T>(string path) where T : Object
        {
            T res = Resources.Load<T>(path);
            if (res is GameObject)
            {
                return Instantiate(res);
            }
            else
            {
                return res;
            }
        }
        public void LoadAsync<T>(string path, UnityAction<T> finishCallBack) where T : Object
        {
            StartCoroutine(IE_LoadAsync(path, finishCallBack));
        }

        private IEnumerator IE_LoadAsync<T>(string path, UnityAction<T> finishCallBack) where T : Object
        {
            ResourceRequest resourceRequest = Resources.LoadAsync<T>(path);
            yield return resourceRequest;
            if (resourceRequest.asset is GameObject)
            {
                finishCallBack.Invoke(Instantiate(resourceRequest.asset) as T);
            }
            else
            {
                if (finishCallBack != null)
                {
                    finishCallBack.Invoke(resourceRequest.asset as T);
                }
            }
        }
    }

