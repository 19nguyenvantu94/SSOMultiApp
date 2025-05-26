using AspectInjector.Broker;

namespace Authen.Server.Aop
{
    [Injection(typeof(ApiResponseExceptionAspect))]
    public class ApiResponseExceptionAttribute : Attribute
    {
    }
}
