﻿using AttendanceTracker.Models.DB;
using System.ComponentModel.DataAnnotations;

namespace AttendanceTracker.Models
{
	public class TimeTableViewModel
	{
		public Group group;

		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
		public DateTime RangeStart { get; set; }

		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
		public DateTime RangeEnd { get; set; }
	}
}
