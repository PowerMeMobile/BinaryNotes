using System;
using System.IO;
using org.bn;

namespace mmwl.AMQP
{
    public class ASN1Coder
    {
        private const String ENCODING_SCHEMA = "BER";

        public static Byte[] Encode(Object obj)
        {
            var encoder = CoderFactory.getInstance().newEncoder(ENCODING_SCHEMA);
            using (var stream = new MemoryStream())
            {
                encoder.encode(obj, stream);
                return stream.ToArray();
            }
        }

        public static T Decode<T>(Byte[] data)
        {
            var decoder = CoderFactory.getInstance().newDecoder(ENCODING_SCHEMA);
            using (var stream = new MemoryStream(data))
            {
                return decoder.decode<T>(stream);
            }
        }
    }
}
