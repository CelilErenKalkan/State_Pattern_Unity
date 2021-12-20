using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Assets
{
    public sealed class GameEnvironment
    {
        private static GameEnvironment instance;
        private List<GameObject> checkpointsList = new List<GameObject>();
        private List<GameObject> safepointsList = new List<GameObject>();
        public List<GameObject> CheckpointsList
        {
            get { return checkpointsList; }
        }

        public List<GameObject> SafepointsList
        {
            get { return safepointsList; }
        }

        public static GameEnvironment Singleton
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameEnvironment();

                    instance.CheckpointsList.AddRange(GameObject.FindGameObjectsWithTag("Checkpoint"));
                    instance.checkpointsList = instance.checkpointsList.OrderBy(waypoint => waypoint.name).ToList();

                    instance.SafepointsList.AddRange(GameObject.FindGameObjectsWithTag("Safe"));
                    instance.safepointsList = instance.safepointsList.OrderBy(waypoint => waypoint.name).ToList();
                }

                return instance;
            }
        }

        public GameObject RandomGoal()
        {
            return checkpointsList[Random.Range(0, checkpointsList.Count)];
        }
    }
}