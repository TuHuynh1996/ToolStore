using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSC_CreateDataTestTool.ExcelDTO
{
    public class InputItemDTO : ICloneable
    {
        public int No { get; set; }
        public string Name { get; set; }
        public int Rank { get; set; }
        public string Type { get; set; }
        public int Digit { get; set; }
        public int RepeatNumber { get; set; }
        public int Potision { get; set; }
        public string ID { get; set; }
        public string IOInfo { get; set; }
        public InputItemDTO Clone()
        {
            return (InputItemDTO)this.MemberwiseClone();
        }

        object ICloneable.Clone()
        {
            throw new NotImplementedException();
        }
    }

    public class OutputItemDTO
    {
        public int No { get; set; }
        public string Name { get; set; }
        public string Attributes { get; set; }
        public int Digit { get; set; }
        public string IOInfo { get; set; }
        public string JsonName { get; set; }
    }

    //static class Attribute
    //{
    //    public static string letter = "";
    //    public static string number = "";
    //    public static string data = "";
    //    public static string time = "";
    //    public static string number = "";
    //    public static string number = "";
    //}
}


