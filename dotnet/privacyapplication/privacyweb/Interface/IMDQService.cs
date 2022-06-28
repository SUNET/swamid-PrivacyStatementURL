using privacyweb.Model;

namespace privacyweb.Interface
{
    public interface IMDQService
    {
        MDQModel GetRequestedAttributes(string entityId);
    }
}
