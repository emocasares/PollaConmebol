namespace PollaEngendrilClientHosted.Server.Services.ScoringStaregies
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    sealed class StrategyNameAttribute : Attribute
    {
        public string Name { get; }

        public StrategyNameAttribute(string name)
        {
            Name = name;
        }
    }
}
