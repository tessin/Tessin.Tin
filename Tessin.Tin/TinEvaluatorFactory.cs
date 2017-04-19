using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Tessin.Tin.Denmark;
using Tessin.Tin.Finland;
using Tessin.Tin.Models;
using Tessin.Tin.Norway;
using Tessin.Tin.Sweden;

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

        public static TinEvaluatorFactory Default => new TinEvaluatorFactory();

        public List<ITinEvaluator> Instances = new List<ITinEvaluator>
        {
            new TinEvaluatorSe(),
            new TinEvaluatorDk(),
            new TinEvaluatorNo(),
            new TinEvaluatorFi(),
            new TinEvaluatorUnknown()
        };

        public List<Problematic<Assembly>> ProblematicAssemblies { get; set; }

        public List<Problematic<Type>> ProblematicTypes { get; set; }

        public TinEvaluatorFactory()
        {
            ProblematicAssemblies = new List<Problematic<Assembly>>();
            ProblematicTypes = new List<Problematic<Type>>();
        }

        public ITinEvaluator Create(TinCountry country)
        {
            try
            {
                var instance = Instances.FirstOrDefault(p => p.Country == country);
                if (instance == null) throw new ArgumentException($"Unsupported country '{country}'.");
                return instance;
            }
            catch (Exception ex)
            {
                var sb = new StringBuilder();
                sb.AppendLine($"Could not resolve '{nameof(country)}'.");
                foreach (var assembly in ProblematicAssemblies)
                {
                    sb.AppendLine($"Assembly: {assembly.Instance?.FullName}");
                }
                foreach (var assembly in ProblematicTypes)
                {
                    sb.AppendLine($"Type: {nameof(assembly.Instance)}");
                }
                throw new Exception(sb.ToString(), ex);
            }
        }

        
    }
}
