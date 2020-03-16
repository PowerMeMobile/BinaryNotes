using System;

namespace mmwl.AMQP
{
    public interface IASN1Decodable<T>
    {
        T DecodeFromASN1(Byte[] body);
    }
}
