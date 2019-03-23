namespace EventBus.RabbitMQ
{
    using System.Reflection;
    using Castle.Facilities.TypedFactory;

    public class IntegrationEventHandlerComponentSelector : DefaultTypedFactoryComponentSelector
    {
        protected override string GetComponentName(MethodInfo method, object[] arguments)
        {
            return arguments[0].ToString();
        }
    }
}