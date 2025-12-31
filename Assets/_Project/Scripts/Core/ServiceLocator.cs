using System;
using System.Collections.Generic;

public static class ServiceLocator
{
    private static readonly Dictionary<Type, IService> s_services = new();

    public static IReadOnlyList<T> GetServices<T>() where T : class
    {
        List<T> results = new();

        foreach (KeyValuePair<Type, IService> servicePair in s_services)
            if (servicePair.Value is T service)
                results.Add(service);

        return results;
    }

    public static void Register<T>(T service) where T : class, IService
    {
        Type type = typeof(T);

        if (s_services.ContainsKey(type))
            throw new InvalidOperationException($"Попытка повторной регистрации сервиса {type.Name}");

        s_services[type] = service;
    }

    public static bool TryGet<T>(out T service) where T : class, IService
    {
        if (s_services.TryGetValue(typeof(T), out IService existingService))
        {
            service = (T)existingService;

            return true;
        }

        service = null;

        return false;
    }

    public static T Get<T>() where T : class, IService
    {
        Type type = typeof(T);

        if (s_services.TryGetValue(type, out IService existingService))
            return (T)existingService;

        throw new Exception($"Не удалось получить тип {type}");
    }
}