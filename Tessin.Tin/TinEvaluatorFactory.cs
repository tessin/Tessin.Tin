using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using Tessin.Tin.Models;

namespace Tessin.Tin
{

    public class Problematic<T>
    {
        public Exception Exception { get; set; }
        public T Instance { get; set; }

        public Problematic(Exception ex, T instance)
        {
            Exception = ex;
            Instance = instance;
        }

    }

    public class TinEvaluatorFactory
    {

        public static TinEvaluatorFactory Default => DefaultLazy.Value;

        private static readonly ThreadLocal<TinEvaluatorFactory> DefaultLazy = new ThreadLocal<TinEvaluatorFactory>(() =>
            new TinEvaluatorFactory(AppDomain.CurrentDomain.GetAssemblies().ToArray()));

        // TODO: Upgrade to Dictionary of TinCountry and ITinEvaluator 
        // TODO: once the collection of languages grows sufficiently.
        public List<ITinEvaluator> Instances { get; }

        public List<Problematic<Assembly>> ProblematicAssemblies { get; set; }

        public List<Problematic<Type>> ProblematicTypes { get; set; }

        public TinEvaluatorFactory()
        {
            ProblematicAssemblies = new List<Problematic<Assembly>>();
            ProblematicTypes = new List<Problematic<Type>>();
        }

        public ITinEvaluator Create(TinCountry country)
        {
            var instance = Instances.FirstOrDefault(p => p.Country == country);
            if (instance == null) throw new ArgumentException($"Unsupported country '{country}'.");
            return instance;
        }

        public TinEvaluatorFactory(params Assembly[] domain)
        {
            if (domain == null)
            {
                throw new ArgumentNullException(nameof(domain));
            }

            var instances = new List<ITinEvaluator>();
            foreach (var assembly in domain)
            {
                try
                {
                    var types = assembly.GetTypes().Where((x) => typeof(ITinEvaluator).IsAssignableFrom(x) && !x.IsAbstract);
                    foreach (var type in types)
                    {
                        ITinEvaluator instance;
                        try
                        {
                            instance = Activator.CreateInstance(type) as ITinEvaluator;
                        }
                        catch (Exception ex)
                        {
                            ProblematicTypes.Add(new Problematic<Type>(ex, type));
                            continue;
                        }
                        instances.Add(instance);
                    }
                }
                catch (Exception ex)
                {
                    ProblematicAssemblies.Add(new Problematic<Assembly>(ex, assembly));
                }
            }
            Instances = instances;
        }
    }
}
