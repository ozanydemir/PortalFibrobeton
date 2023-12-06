using PortalFibrobeton.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalFibrobeton.Models.Class.ARGE.OlgunlukViewModels
{
    public class OlgunlukViewModelDashboard
    {
        public List<OlgunlukCihazi> OlgunlukCihazList { get; set; }
        public List<OlgunlukSensor> OlgunlukSensorList { get; set; }
        public List<OlgunlukViewModelDashboard> SensorCihazList { get; set; }

    }
}