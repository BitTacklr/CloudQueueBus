using System;
using System.Runtime.InteropServices;

namespace CloudQueueBus
{
    internal static class SerialGuid
    {
        [DllImport("rpcrt4.dll", SetLastError = true)]
        private static extern int UuidCreateSequential(out Guid guid);

        private const int RPC_S_OK = 0;
        private const int RPC_UUID_LOCAL_ONLY = 1824;

        public static Guid NewGuid()
        {
            Guid serialGuid;

            var hResult = UuidCreateSequential(out serialGuid);
            if ((hResult == RPC_S_OK) || (hResult == RPC_UUID_LOCAL_ONLY))
            {
                //Match SQL IDENTITY ordering
                var source = serialGuid.ToByteArray();
                var destination = serialGuid.ToByteArray();
                destination[0] = source[3];
                destination[1] = source[2];
                destination[2] = source[1];
                destination[3] = source[0];
                destination[4] = source[5];
                destination[5] = source[4];
                destination[6] = source[7];
                destination[7] = source[6];
                return new Guid(destination);
            }
            return Guid.NewGuid();
        }
    }
}
