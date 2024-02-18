﻿namespace BackEndInfrastructure.DynamicLinqCore.ToLinqDataHttpPostRequestBaseClasses
{
    public class RootObject
    {
        public int? Draw { get; set; }
        public List<Column>? Columns { get; set; }
        public List<Order>? Order { get; set; }
        public int? Start { get; set; }
        public int? Length { get; set; }
        public Search? Search { get; set; }
    }
}
