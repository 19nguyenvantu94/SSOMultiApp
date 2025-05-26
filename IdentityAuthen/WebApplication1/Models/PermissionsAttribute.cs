using Authen.Users;

namespace Models
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PermissionsAttribute : Attribute
    {
        public Actions Actions;

        public PermissionsAttribute(Actions actions)
        {
            Actions = actions;
        }
    }
}
