namespace BreakOut.Modifiers
{
    public static class Modifiers
    {
        static readonly List<WeakReference> handlers = new(0);
        static GameFlag prevFlag = GameFlag.None;

        /// <summary>
        /// The flags added since last Apply() call
        /// </summary>
        public static GameFlag Added(GameFlag flag)
        {
            return flag & ~prevFlag;
        }

        /// <summary>
        /// The flags removed since last Apply() call
        /// </summary>
        public static GameFlag Removed(GameFlag flag)
        {
            return prevFlag & ~flag;
        }

        /// <summary>
        /// Applies modifiers to each active handler 
        /// </summary>
        public static void Apply(GameFlag flag)
        {
            foreach (WeakReference handler in handlers)
            {
                if (handler.IsAlive)
                {
                    (handler.Target as IFlagHandler)?.ApplyModifiers(flag);
                }
            }

            // has to be after foreach loop
            prevFlag = flag;
        }
        
        /// <summary>
        /// Adds a new handler to the list
        /// </summary>
        public static void Subscribe(IFlagHandler handle)
        {
            handlers.Add(new WeakReference(handle));
        }

        /// <summary>
        /// Removes all elements from list
        /// </summary>
        public static void Clear()
        {
            handlers.Clear();
        }
    }
}


