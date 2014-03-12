namespace PingYourPackage.API.Formatting
{
    using System.Net.Http.Formatting;
    using System.Reflection;

    public class SuppressedRequiredMemberSelector : IRequiredMemberSelector
    {
        public bool IsRequiredMember(MemberInfo member)
        {
            return false;
        }
    }
}
