﻿using AbiokaApi.Infrastructure.Common.IoC;
using Castle.Core;
using Castle.MicroKernel.ModelBuilder.Descriptors;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using System;
using System.Collections.Generic;

namespace AbiokaApi.Infrastructure.Framework.IoC
{
    public class CastleContainer : IDependencyContainer
    {
        public CastleContainer() {
            container = new WindsorContainer();
            container.Kernel.Resolver.AddSubResolver(new CollectionResolver(container.Kernel, true));
        }

        /// <summary>
        /// Resolve the target type with necessary dependencies.
        /// </summary>
        public object Resolve(Type targetType) {
            if (container.Kernel.HasComponent(targetType))
            {
                return container.Resolve(targetType);
            }
            return null;
        }

        /// <summary>
        /// Resolves all registered instances for a specific service type.
        /// </summary>
        public IList<object> ResolveAll(Type serviceType) {
            if (container.Kernel.HasComponent(serviceType))
            {
                return new List<object>((object[])container.ResolveAll(serviceType));
            }
            return null;
        }

        public readonly IWindsorContainer container;

        public T Resolve<T>() => container.Resolve<T>();

        public IEnumerable<T> ResolveAll<T>() => container.ResolveAll<T>();

        public IDependencyContainer Register<T>(LifeStyle lifeStyle) => Register(typeof(T), lifeStyle);

        public IDependencyContainer Register(Type type, LifeStyle lifeStyle) {
            RegisterComponent(Component.For(type), lifeStyle);
            return this;
        }

        public IDependencyContainer RegisterServices<T>() => RegisterServices(typeof(T));

        private IDependencyContainer RegisterServices(Type type) {
            container.Register(Classes.FromAssemblyContaining(type).BasedOn(type).WithService.FromInterface().Configure(c => c.LifestyleSingleton().Interceptors<ServiceInterceptor>()));
            return this;
        }

        public IDependencyContainer RegisterService<T1, T2>(LifeStyle lifeStyle) => RegisterService(typeof(T1), typeof(T2), lifeStyle);

        public IDependencyContainer RegisterService(Type type1, Type type2, LifeStyle lifeStyle) {
            RegisterComponent(Component.For(type1).ImplementedBy(type2).Interceptors<ServiceInterceptor>(), lifeStyle);
            return this;
        }

        public IDependencyContainer RegisterWithDefaultInterfaces<T1, T2>() => RegisterWithDefaultInterfaces(typeof(T1), typeof(T2));

        public IDependencyContainer RegisterWithDefaultInterfaces(Type type1, Type type2) {
            container.Register(Classes.FromAssemblyContaining(type2).BasedOn(type1).WithService.DefaultInterfaces().Configure(c => c.LifestyleSingleton().Named(Guid.NewGuid().ToString())));
            return this;
        }

        public IDependencyContainer RegisterWithBase<T1, T2>() => RegisterWithBase(typeof(T1), typeof(T2));

        public IDependencyContainer RegisterWithBase(Type type1, Type type2) {
            container.Register(Classes.FromAssemblyContaining(type2).BasedOn(type1).WithService.Base().Configure(c => c.LifestyleSingleton().Named(Guid.NewGuid().ToString())));
            return this;
        }

        public IDependencyContainer Register<T1, T2>(LifeStyle lifeStyle) => Register(typeof(T1), typeof(T2), lifeStyle);

        public IDependencyContainer Register(Type type1, Type type2, LifeStyle lifeStyle) {
            RegisterComponent(Component.For(type1).ImplementedBy(type2), lifeStyle);
            return this;
        }

        public void Release(object instance) {
            container.Release(instance);
        }

        public void Dispose() {
            container.Dispose();
        }

        public IDependencyContainer UsingFactoryMethod<T>(Func<T> func) {
            container.Register(Component.For(typeof(T)).UsingFactoryMethod(func).LifeStyle.Singleton);
            return this;
        }

        private void RegisterComponent<T>(ComponentRegistration<T> componentRegistration, LifeStyle lifeStyle) where T: class {
            var lifestyleDescriptor = new LifestyleDescriptor<T>(GetLifestyleType(lifeStyle));
            componentRegistration.AddDescriptor(lifestyleDescriptor);
            container.Register(componentRegistration);
        }

        private LifestyleType GetLifestyleType(LifeStyle lifeStyle) {
            switch (lifeStyle)
            {
                case LifeStyle.PerThread:
                    return LifestyleType.Thread;
                case LifeStyle.PerWebRequest:
                    return LifestyleType.PerWebRequest;
                case LifeStyle.Singleton:
                    return LifestyleType.Singleton;
                case LifeStyle.Transient:
                    return LifestyleType.Transient;
                default:
                    throw new NotSupportedException($"{lifeStyle} is not a supported life style.");
            }
        }
    }
}
