
using System;
using System.Collections.Generic;
using System.Text;
using GQL.Utils.Extensions;
using SimpleDocumentStore;

namespace CustomerManagementStore.Model
{
    public class Customer : DocumentBase,IComparable
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string TelephoneNumber { get; set; }
        public string EmailAddress { get; set; }

        bool Equals(object obj)
        {
            return DeepEquals(obj);
        }
        bool DeepEquals(object obj)
        {
            var other = obj as Customer;
            if (other == null)
            {
                return false;
            }
            if(!base.Equals(obj))
            {
                return false;
            }

            if (!Name.IsEqual(other.Name))
            {
                return false;
            }
            if (!Address.IsEqual(other.Address))
            {
                return false;
            }
            if (!PostalCode.IsEqual(other.PostalCode))
            {
                return false;
            }
            if (!City.IsEqual(other.City))
            {
                return false;
            }
            if (!TelephoneNumber.IsEqual(other.TelephoneNumber))
            {
                return false;
            }
            if (!EmailAddress.IsEqual(other.EmailAddress))
            {
                return false;
            }
            
            return true;
        }

        public int CompareTo(object obj)
        {
            return Equals(obj) ? 0 : -1;
        }
        // v2 functionality:
        // public int? LoyaltyLevel
        // {
        //     get
        //     {
        //         switch (City.ToLowerInvariant())
        //         {
        //             case "amsterdam":
        //                 return 3;
        //             case "den haag":
        //                 return 2;
        //             default:
        //                 return 1;
        //         }
        //     }
        // }
    }
}
