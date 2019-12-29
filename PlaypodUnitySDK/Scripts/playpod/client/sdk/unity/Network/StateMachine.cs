using System;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

namespace playpod.client.sdk.unity.Network
{
    public enum State { NotRunning, Running, Connected, Ping, Pong, Done }

    public class StateMachine/* : MonoBehaviour*/
    {
        private readonly object _syncLock = new object();

        private readonly Queue<State> _pendingTransitions = new Queue<State>();
        private readonly Queue<JSONObject> _pendingMessages = new Queue<JSONObject>();

        private readonly Dictionary<State, Action<JSONObject>> _handlers
            = new Dictionary<State, Action<JSONObject>>();

        [SerializeField] private State currentState = State.NotRunning;
        private JSONObject _message;

        public void Run()
        {
            Console.WriteLine("StateMachine.run");
            Transition(State.Running, new JSONObject());
        }

        public void AddHandler(State state, Action<JSONObject> action)
        {
            _handlers[state] = action;
        }

        public void Transition(State state, JSONObject msg)
        {
            lock (_syncLock)
            {
                _pendingTransitions.Enqueue(state);
                _pendingMessages.Enqueue(msg);
            }
        }

        public void Update()
        {
            while (_pendingTransitions.Count > 0)
            {
                currentState = _pendingTransitions.Dequeue();
                _message = _pendingMessages.Dequeue();
                //Debug.Log("Transitioned to state " + currentState);

                Action<JSONObject> action;
                if (_handlers.TryGetValue(currentState, out action))
                {
                    action(_message);
                }
            }
        }

    }
}
