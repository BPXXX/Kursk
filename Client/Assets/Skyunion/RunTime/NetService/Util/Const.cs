namespace Skyunion
{
    /// <summary>
    /// 2015/12/05 Hyeon
    /// Const
    /// </summary>
    internal class Const<T>
    {
        public T Value { get; private set; }

        public Const() { }

        public Const(T value)
            : this()
        {
            this.Value = value;
        }
    }
}

