using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassionProject.Models.ViewModels
{
    public class DetailsReception
    {

        public ReceptionDto SelectedReception { get; set; }
        public IEnumerable<AttendeeDto> RegAttendees { get; set; }

        public IEnumerable<AttendeeDto> PotenialAttendees { get; set; }
    }
}