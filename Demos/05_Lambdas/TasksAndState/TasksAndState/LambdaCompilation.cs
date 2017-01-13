using System;

namespace TasksAndState
{
    class Helper
    {
        public void Foo() { }
    }

    class LambdaCompilation
    {
        private void Goo()
        {
            var h = new Helper();
            Action f = () => h.Foo();
            f();
        }
    }
}
