using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Routing;
using PingYourPackage.API.Http;
using PingYourPackage.Domain.Services;

namespace PingYourPackage.API.Dispatcher
{
    public class AffiliateShipmentsDispatcher : DelegatingHandler {

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, 
            CancellationToken cancellationToken) {

            // We know at this point that the {key} route variable has 
            // been supplied. Otherwise, we wouldn't be here. So, just get it.
            IHttpRouteData routeData = request.GetRouteData();
            Guid affiliateKey = Guid.ParseExact(routeData.Values["key"].ToString(), "D");

            IShipmentService shipmentService = request.GetShipmentService();
            if (shipmentService.GetAffiliate(affiliateKey) == null) {

                return Task.FromResult(
                    request.CreateResponse(HttpStatusCode.NotFound));
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}