using System.Collections.Generic;
using UnityEngine;

namespace Behavioral.Mediator
{
    public class AnimationSystem
    {
        public class AnimTask
        {
            public Transform TargetTransform;
            public Vector3 TargetPosition;
            public float Speed;
        }

        public bool IsAnimating => tasks.Count > 0;

        private List<AnimTask> tasks = new List<AnimTask>();

        public void NewAnim(Transform targetTransform, Vector3 target, float speed)
        {
            for (int i = 0; i < tasks.Count; ++i)
            {
                if (tasks[i].TargetTransform == targetTransform)
                {
                    tasks[i].TargetPosition = target;
                    tasks[i].Speed = speed;
                    return;
                }
            }
        
            tasks.Add(new AnimTask()
            {
                TargetPosition = target,
                TargetTransform = targetTransform,
                Speed = speed
            });
        }

        public void Update()
        {
            for (int i = 0; i < tasks.Count; ++i)
            {
                var task = tasks[i];
                var t = task.TargetTransform;
                t.position = Vector3.MoveTowards(t.position, task.TargetPosition, task.Speed * Time.deltaTime);

                if (Vector3.Distance(t.position, task.TargetPosition) <= 0.1f)
                {
                    tasks.RemoveAt(i);
                    i--;
                }
            }
        }
    }
    
}