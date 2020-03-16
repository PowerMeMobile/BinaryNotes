using System;
using System.Collections.Generic;
using mmwl.AMQP.ASN1Mappers;

namespace mmwl.AMQP
{
    public class ThroughputResponse : IASN1Decodable<ThroughputResponse>
    {
        public List<Slice> Slices { get; set; }

        public ThroughputResponse DecodeFromASN1(byte[] body)
        {
            var asn1Throughput = ASN1Coder.Decode<ASN1Classes.Just.ThroughputResponse>(body);
            //return ThroughputResponseMapper.Convert(asn1Throughput);
            return new ThroughputResponse();
        }
    }

    [Serializable]
    public class Slice
    {
        public Slice() {}

        public DateTime PeriodStart { get; set; }

        public List<Counter> Counters { get; set; }
    }

    [Serializable]
    public class Counter
    {
        public Counter() { }

        public Guid GatewayId { get; set; }

        public String ConnectionName { get; set; }

        public CounterType CounterType { get; set; }

        public Int32 Value { get; set; }

        public Guid CustomerId { get; set; }
    }

    public enum CounterType
    {
        SmsOut = 1,
        SmsIn = 2,
        MmsOut = 3,
        MmsIn = 4,
        SmscStatusSuccess = 5,
        SmscStatusDelivered = 6,
        SmscStatusExpired = 7,
        SmscStatusUndeliverable = 8
   }
}
