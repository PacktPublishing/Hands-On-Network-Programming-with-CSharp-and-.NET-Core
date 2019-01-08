using System;
using System.Collections.Generic;

namespace StreamsAndAsync {
    public class ComplexModel {
        public string ComplexModelId { get; set; } = Guid.NewGuid().ToString();
        public int NumberDemonstration { get; set; } = 12354;
        public InnerModel smallInnerModel { get; set; }
        public List<InnerModel> listOfInnerModels { get; set; } = new List<InnerModel>() {
            new InnerModel(),
            new InnerModel() 
        };
    }

    public class InnerModel {
        public string randomId { get; set; } = Guid.NewGuid().ToString();
        public string nonRandomString { get; set; } = "I wrote this here.";
    }
}
