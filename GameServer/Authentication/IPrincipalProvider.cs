using System.Linq;
using System.Security.Principal;
using System.Text;

namespace GameServer.Authentication
{
    public interface IPrincipalProvider
    {
        IPrincipal CreatePrincipal(string username, string password);
    }
}
