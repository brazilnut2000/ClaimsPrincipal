using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FunWithClaimsIdentity
{
    class Program
    {
        static void Main(string[] args)
        {
            SetupPrincipal();

            UsePrincipalLegacy();
            UsePrincipalNewCode();

        }

        private static void UsePrincipalNewCode()
        {
            var cp = ClaimsPrincipal.Current;

            var roles = cp.FindAll(ClaimTypes.Role).ToList();
            foreach (var role in roles)
            {
                Console.WriteLine(role.Value);
            }
        }

        private static void UsePrincipalLegacy()
        {
            var p = Thread.CurrentPrincipal;
            Console.WriteLine(p.Identity.Name);
            Console.WriteLine(p.IsInRole("Geek"));
        }

        private static void SetupPrincipal()
        {
            
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "Jon"),
                new Claim(ClaimTypes.Email, "foo@bar.net"),
                new Claim(ClaimTypes.Role, "Geek"),
                new Claim(ClaimTypes.Role, "Dad"),
                new Claim(ClaimTypes.Role, "Husband"),
                new Claim(ClaimTypes.Role, "Son"),
                new Claim(ClaimTypes.Role, "Brother"),
                new Claim("http://myclaims/location", "Dallas")
            };

            // add some claims from a string array
            var roles = new[] { "coach", "player", "left-handed" };
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var id = new ClaimsIdentity(claims, "Console App");
            Console.WriteLine(id.IsAuthenticated);

            var p = new ClaimsPrincipal(id);
            Thread.CurrentPrincipal = p;
        }
    }

    sealed class CorpIdentity : ClaimsIdentity
    {
        public CorpIdentity(string name, string reportsTo, string office)
        {
            AddClaim(new Claim(ClaimTypes.Name, name));
            AddClaim(new Claim("reportsTo", reportsTo));
            AddClaim(new Claim("office", office));
        }

        public string ReportsTo
        {
            get { return FindFirst("reportsTo").Value; }
        }
    }
}
