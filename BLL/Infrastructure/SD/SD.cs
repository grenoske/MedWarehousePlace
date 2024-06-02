using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Infrastructure.SD
{
    public static class SD
    {
        public const string StatusReceived = "Receiving";
        public const string StatusTransferring = "Transferring";
        public const string StatusInventory = "Inventory";
        public const string StatusPacking = "Packing";
        public const string StatusShipped = "Shipping";
        public const string StatusCancelled = "Cancelled";

        public const string LocationReceiving = "ReceivingArea";
        public const string LocationPacking = "PackingArea";
        public const string LocationTransferring = "Transferring";
        public const string LocationInventory = "InventoryArea";

        public const string RoleWorker = "worker";
        public const string RoleAdmin = "admin";

        public const int HighTurnOverRatePerDay = 10;
    }
}
