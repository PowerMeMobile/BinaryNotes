using System;
using System.Linq;

namespace mmwl.AMQP.ASN1Mappers
{
    public class ThroughputResponseMapper
    {
        public static ThroughputResponse Convert(ASN1Classes.Just.ThroughputResponse asn1Response)
        {
            var response = new ThroughputResponse
                               {
                                   Slices = asn1Response.Slices.Select(x => new Slice
                                                                                {
                                                                                    PeriodStart = TimeConvertingUtil.GetLocalDateTime(x.PeriodStart),
                                                                                    Counters = x.Counters.Select(y => new Counter
                                                                                                     {
                                                                                                         ConnectionName = y.ConnectionName,
                                                                                                         CounterType = (CounterType)(y.Type.Value + 1),
                                                                                                         GatewayId = new Guid(y.GatewayId),
                                                                                                         Value = (Int32) y.Count,
                                                                                                         CustomerId = ToGuid(y.CustomerId)
                                                                                                     }).ToList()
                                                                                }).ToList()
                               };

            return response;
        }

        private static Guid ToGuid(String value)
        {
            Guid result;
            if (Guid.TryParse(value, out result))
                return result;
            else
                return Guid.Empty;
        }
    }
}
