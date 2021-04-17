using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;

namespace Codility.WarehouseApi
{
    public class CapacityRecord
    {
        public int ProductId { get; set; }
        public int Capacity { get; set; }
    }

    public class ProductRecord
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
    public class WarehouseController : Controller
    {
        private IEnumerable<ProductRecord> productRecords;
        private IEnumerable<CapacityRecord> capacityRecords;
        public interface IWarehouseRepository
        {
            void SetCapacityRecord(int productId, int capacity);
            IEnumerable<CapacityRecord> GetCapacityRecords();
            IEnumerable<CapacityRecord> GetCapacityRecords(Func<CapacityRecord, bool> filter);

            void SetProductRecord(int productId, int quantity);
            IEnumerable<ProductRecord> GetProductRecords();
            IEnumerable<ProductRecord> GetProductRecords(Func<ProductRecord, bool> filter);
        }

        public class NotPositiveQuantityMessage
        {
            public string Message => "A positive quantity was not provided";
        }

        // QuantityTooLowMessage should be returned by
        // SetProductCapacity, ReceiveProduct and DispatchProduct methods
        public class QuantityTooLowMessage
        {
            public string Message => "Too low a quantity was provided";
        }

        // QuantityTooHighMessage should be returned by
        // SetProductCapacity, ReceiveProduct and DispatchProduct methods
        public class QuantityTooHighMessage
        {
            public string Message => "Too high a quantity was provided";
        }

        private readonly IWarehouseRepository _warehouseRepository;

        public WarehouseController(IWarehouseRepository warehouseRepository)
        {
            _warehouseRepository = warehouseRepository;
            productRecords = _warehouseRepository.GetProductRecords();
            capacityRecords = _warehouseRepository.GetCapacityRecords();
        }

        // Return OkObjectResult(IEnumerable<WarehouseEntry>)
        public IActionResult GetProducts()
        {
            return (IActionResult)Json(productRecords.Where(i=>i.Quantity > 0));
        }

        // Return OkResult, BadRequestObjectResult(NotPositiveQuantityMessage), or BadRequestObjectResult(QuantityTooLowMessage)
        public IActionResult SetProductCapacity(int productId, int capacity)
        {
            if (capacity <= 0)
            {
                return  ( (IActionResult)(new HttpStatusCodeResult(HttpStatusCode.BadRequest, (new NotPositiveQuantityMessage().Message))));
            }
            else if (capacity < capacityRecords.Count(i => i.ProductId == productId))
            {
                return (IActionResult)(new HttpStatusCodeResult(HttpStatusCode.BadRequest, (new QuantityTooLowMessage().Message)));
            }
            return (IActionResult)(new HttpStatusCodeResult(HttpStatusCode.OK));
        }

        // Return OkResult, BadRequestObjectResult(NotPositiveQuantityMessage), or BadRequestObjectResult(QuantityTooHighMessage)
        public IActionResult ReceiveProduct(int productId, int qty)
        {
            if (qty <= 0)
            {
                return (IActionResult)(new HttpStatusCodeResult(HttpStatusCode.BadRequest, (new NotPositiveQuantityMessage().Message)));

            }
            else if (qty < productRecords.Where(i => i.ProductId == productId).FirstOrDefault().Quantity)
            {
                return (IActionResult)(new HttpStatusCodeResult(HttpStatusCode.BadRequest, (new QuantityTooLowMessage().Message)));
            }
            return (IActionResult)(new HttpStatusCodeResult(HttpStatusCode.OK));
        }

        // Return OkResult, BadRequestObjectResult(NotPositiveQuantityMessage), or BadRequestObjectResult(QuantityTooHighMessage)
        public IActionResult DispatchProduct(int productId, int qty)
        {
            if (qty <= 0)
            {
                return (IActionResult)(new HttpStatusCodeResult(HttpStatusCode.BadRequest, (new NotPositiveQuantityMessage().Message))); ;
            }
            else if (qty > productRecords.Where(i => i.ProductId == productId).FirstOrDefault().Quantity)
            {
                return (IActionResult)(new HttpStatusCodeResult(HttpStatusCode.BadRequest, (new QuantityTooLowMessage().Message)));
            }
            return (IActionResult)(new HttpStatusCodeResult(HttpStatusCode.OK));
        }
    }
}