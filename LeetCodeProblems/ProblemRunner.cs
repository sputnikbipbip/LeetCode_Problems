using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public static class ProblemRunner
{
    public static IEnumerable<IProblem> DiscoverProblems()
    {
        var asm = Assembly.GetExecutingAssembly();
        var types = asm.GetTypes()
            .Where(t => typeof(IProblem).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

        foreach (var t in types)
        {
            if (Activator.CreateInstance(t) is IProblem p)
                yield return p;
        }
    }

    public static IProblem? FindByName(string name)
    {
        return DiscoverProblems().FirstOrDefault(p => string.Equals(p.Name, name, StringComparison.OrdinalIgnoreCase));
    }
}
