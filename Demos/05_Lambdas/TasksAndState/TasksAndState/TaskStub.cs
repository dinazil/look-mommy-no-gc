using System;

namespace TasksAndState
{
    class TaskStub
    {
        private object _action;
        private object _state;

        public static TaskStub StartNew(Action action)
        {
            return new TaskStub(action);
        }

        public static TaskStub StartNew(Action<object> action, object state)
        {
            return new TaskStub(action, state);
        }

        public TaskStub(Action action)
        {
            _action = action;
            _state = null;
        }

        public TaskStub(Action<object> action, object state)
        {
            _action = action;
            _state = state;
        }
    }

    struct STaskStub
    {
        private object _action;
        private object _state;

        public static STaskStub StartNew(Action action)
        {
            return new STaskStub(action);
        }

        public static STaskStub StartNew(Action<object> action, object state)
        {
            return new STaskStub(action, state);
        }

        public STaskStub(Action action)
        {
            _action = action;
            _state = null;
        }

        public STaskStub(Action<object> action, object state)
        {
            _action = action;
            _state = state;
        }
    }
}
