﻿namespace TeaMan.Models
{
    public class OrderedListItemEntity : Entity
    {
        public OrderedListItemEntity(string name, int order) => (Name, Order) = (name, order);

        public OrderedListItemEntity() { }

        public string Name { get; set; }

        public int Order { get; set; }
    }
}
