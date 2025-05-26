
using AspectInjector.Broker;

namespace Authen.Server.Aop
{
    [Injection(typeof(LogExceptionAspect))]
    public class LogExceptionAttribute : Attribute
    {
    }
}
