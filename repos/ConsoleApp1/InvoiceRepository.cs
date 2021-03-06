using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ConsoleApp1
{

    public class InvoiceRepository : IInvoiceRepository
    {
        private static IQueryable<Invoice> invoicesList;
        public InvoiceRepository(IQueryable<Invoice> invoices)
        {
            if (invoices == null)
            {
                throw new ArgumentNullException();
            }
            else
            {
                invoicesList = invoices;
            }
        }

        /// <summary>
        /// Should return a total value of an invoice with a given id. If an invoice does not exist null should be returned.
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <returns></returns>
        public decimal? GetTotal(int invoiceId)
        {
            if (invoicesList.FirstOrDefault(i => i.Id == invoiceId) == null)
            {
                return null;
            }

            var firstItem = invoicesList.Where(i => i.Id == invoiceId).FirstOrDefault();

            return firstItem.InvoiceItems.Sum(i => (i.Count * i.Price));
        }

        /// <summary>
        /// Should return a total value of all unpaid invoices.
        /// </summary>
        /// <returns></returns>
        public decimal GetTotalOfUnpaid()
        {

            decimal tot = 0;
            var unpaidlist = invoicesList.Where(i => i.AcceptanceDate == null).ToList();

            foreach (var item in unpaidlist)
            {
                tot = tot + item.InvoiceItems.Sum(i => (i.Price * i.Count));
            }
            return tot;
            //.Sum(i => i.InvoiceItems.Sum(i => (i.Price * i.Count)));
        }

        /// <summary>
        /// Should return a dictionary where the name of an invoice item is a key and the number of bought items is a value.
        /// The number of bought items should be summed within a given period of time (from, to). Both the from date and the end date can be null.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public IReadOnlyDictionary<string, long> GetItemsReport(DateTime? from, DateTime? to)
        {
            Dictionary<string, long> dict = new Dictionary<string, long>();

            if (from != null && to != null)
            {
                invoicesList = invoicesList.AsQueryable().Where(i => (i.CreationDate >= from) && (i.CreationDate <= to));
            }
            else if (from == null)
            {
                invoicesList = invoicesList.AsQueryable().Where(i => (i.CreationDate >= from));
            }
            else if (to == null)
            {
                invoicesList = invoicesList.AsQueryable().Where(i => (i.CreationDate <= to));
            }

            foreach (var items in invoicesList.Select(i => i.InvoiceItems))
            {
                foreach (var invitem in items.GroupBy(i => i.Name))
                {
                    
                    var firstItem = invitem.FirstOrDefault();

                    if (!dict.ContainsKey(firstItem.Name))
                    {
                        dict.Add(firstItem.Name, (long)(invitem.Count() + items.Where(i => i.Name == invitem.Key).Count()));
                    }
                }
            }

            return dict;
        }
    }


    public interface IInvoiceRepository
    {
        public decimal? GetTotal(int invoiceId);
        public decimal GetTotalOfUnpaid();
        public IReadOnlyDictionary<string, long> GetItemsReport(DateTime? from, DateTime? to);
    }



    public class Invoice
    {
        // A unique numerical identifier of an invoice (mandatory)
        public int Id { get; set; }
        // A short description of an invoice (optional).
        public string Description { get; set; }
        // A number of an invoice e.g. 134/10/2018 (mandatory).
        public string Number { get; set; }
        // An issuer of an invoice e.g. Metz-Anderson, 600  Hickman Street,Illinois (mandatory).
        public string Seller { get; set; }
        // A buyer of a service or a product e.g. John Smith, 4285  Deercove Drive, Dallas (mandatory).
        public string Buyer { get; set; }
        // A date when an invoice was issued (mandatory).
        public DateTime CreationDate { get; set; }
        // A date when an invoice was paid (optional).
        public DateTime? AcceptanceDate { get; set; }
        // A collection of invoice items for a given invoice (can be empty but is never null).
        public IList<InvoiceItem> InvoiceItems { get; }

        public Invoice()
        {
            InvoiceItems = new List<InvoiceItem>();
        }
    }



    public class InvoiceItem
    {
        // A name of an item e.g. eggs.
        public string Name { get; set; }
        // A number of bought items e.g. 10.
        public int Count { get; set; }
        // A price of an item e.g. 20.5.
        public decimal Price { get; set; }
    }


}
