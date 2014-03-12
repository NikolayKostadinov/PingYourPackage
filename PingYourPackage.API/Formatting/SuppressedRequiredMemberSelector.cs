namespace PingYourPackage.API.Formatting
{

    using System.Reflection;
    using System.Net.Http.Formatting;

    public class SuppressedRequiredMemberSelector : IRequiredMemberSelector
    {
        public bool IsRequiredMember(MemberInfo member)
        {
            return false;
        }
    }
}
