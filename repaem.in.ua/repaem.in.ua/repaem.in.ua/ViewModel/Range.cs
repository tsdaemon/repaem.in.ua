using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace aspdev.repaem.ViewModel
{
    /// <summary>
    /// ВьюМодел для типів в данних, котрі виражають інтервал. Час репетиції наприклад.
    /// </summary>
    public class Range
    {
        public int Begin { get; set; }

        public int End { get; set; }

        public Range(int i, int i2)
        {
            Begin = i;
            End = i2;
        }

        public Range()
        {

        }
    }

    public class TimeRange : Range
    {
        public TimeRange()
        {

        }

        public TimeRange(int a, int b)
            : base(a, b)
        {
        
        }
    }

    public class RangeAttribute : Attribute, IMetadataAware
    {
        public RangeAttribute(int i1, int i2)
        {
            Min = i1;
            Max = i2;
        }

        public RangeAttribute()
        {

        }

        public int Min { get; set; }

        public int Max { get; set; }

        public void OnMetadataCreated(ModelMetadata metadata)
        {
            metadata.AdditionalValues["Min"] = Min;
            metadata.AdditionalValues["Max"] = Max;
        }
    }
}