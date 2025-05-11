using System;
using System.Collections.Generic;

using UnityEngine;

namespace Project47
{
    public partial class ObjectPooling<ObjectType> where ObjectType : MonoBehaviour, IPlayable, IResettable
    {
        [NonSerialized()] public ObjectType poolingObject;
        [NonSerialized()] public Transform poolingContainer;

        [NonSerialized()] public List<ObjectType> objects;
        [NonSerialized()] public List<ObjectType> objectsFree;

        protected virtual ObjectType AddObject()
        {
            var localInstance = GameObject.Instantiate(poolingObject, poolingContainer);
            {
                localInstance.gameObject.SetActive(false);
                localInstance.ResetData();
            }
            return localInstance;
        }

        public virtual ObjectType GetObjectFree()
        {
            if (objectsFree.Count != 0)
            {
                var objectFree = objectsFree[0];
                {
                    objectFree.gameObject.SetActive(true);
                    objectFree.StartPlay();
                    objects.Add(objectFree);
                    objectsFree.RemoveAt(0);
                }
                return objectFree;
            }

            var localInstance = AddObject();
            {
                localInstance.gameObject.SetActive(true);
                localInstance.StartPlay();
                objects.Add(localInstance);
            }

            return localInstance;
        }

        public virtual bool Free(ObjectType instance)
        {
            if (objects.Remove(instance))
            {
                instance.gameObject.SetActive(false);
                instance.ResetData();
                objectsFree.Add(instance);
                return true;
            }
            return false;
        }

        public virtual void Free()
        {
            for (int i = 0, n = objects.Count; i != n; i++)
            {
                var localObject = objects[i];
                localObject.gameObject.SetActive(false);
                localObject.ResetData();
                objectsFree.Add(localObject);
            }
            objects.Clear();
        }

        public virtual void Initialize(ObjectType original, Transform container, int objectsInStock)
        {
            poolingObject = original;

            var containerObject = new GameObject("ObjectPooling" + ":" + " " + typeof(ObjectType).Name);
            poolingContainer = containerObject.transform;
            poolingContainer.parent = container;

            var poolingObjects = new ObjectType[objectsInStock];
            {
                for (int i = 0, n = objectsInStock; i != n; i++)
                    poolingObjects[i] = AddObject();
            }
            objectsFree.AddRange(poolingObjects);
        }

        public ObjectPooling()
        {
            objects = new List<ObjectType>();
            objectsFree = new List<ObjectType>();
        }
    }
}