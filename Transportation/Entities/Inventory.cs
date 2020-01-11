﻿using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json.Linq;

namespace Transportation
{
    public class Inventory : Entity
    {
        public DateTime CreatedDate { get; set; }
        [Required]
        public long ProductID { get; set; }
        public virtual Product Product { get; set; }
        public double Quantity { get; set; }
        public long LatestPrice { get; set; }

        public Inventory() {
        }

        public JObject ToJson()
        {
            JObject json = new JObject();
            json["id"] = ID;
            json["productId"] = ProductID;
            json["quantity"] = Quantity;
            json["latestPrice"] = LatestPrice;
            return json;
        }

        public static Inventory FromJson(JObject json)
        {
            Inventory inventory = new Inventory();
            inventory.ApplyJson(json);
            return inventory;
        }

        public void ApplyJson(JObject json)
        {
            ID = json.Value<long>("id");
            ProductID = json.Value<long>("productId");
            Quantity = json.Value<double>("quantity");
            LatestPrice = json.Value<long>("latestPrice");
        }
    }
}
