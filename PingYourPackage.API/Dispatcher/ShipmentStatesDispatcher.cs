using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Routing;
using PingYourPackage.API.Http;
using PingYourPackage.Domain.Services;

namespace PingYourPackage.API.Dispatcher
{
    public class ShipmentStatesDispatcher : DelegatingHandler {

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken) {

            // We know at this point that the key route variable has 
            // been supplied. Otherwise, we wouldn't be here.
            IHttpRouteData routeData = request.GetRouteData();
            Guid shipmentKey = Guid.ParseExact(routeData.Values["key"].ToString(), "D");

            IShipmentService shipmentService = request.GetShipmentService();
            if (shipmentService.GetShipment(shipmentKey) == null) {

                return Task.FromResult(
                    request.CreateResponse(HttpStatusCode.NotFound));
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}