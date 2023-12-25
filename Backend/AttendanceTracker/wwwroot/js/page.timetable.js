function SetupCellEvents(){
	let nodeList = document.querySelectorAll(".entry-cell");
	for (let node of nodeList) {
		node.addEventListener('change', function () {
			let student = node.getAttribute('student');
			let date = node.getAttribute('date');
			let urlParams = new URLSearchParams({ studentId: student, day: date, value: this.value });
			fetch("/api/DayEntry/Edit?" + urlParams, { method: "POST" })
		});
	}
}

function SetupDatepicker()
{
	let buttonPrevMonth = document.querySelector("#prev-month");
	let buttonNextMonth = document.querySelector("#next-month");
	let monthSelector = document.querySelector("#month-select");
	let yearSelector = document.querySelector("#year-select");

	let params = GetCurrentPageParams();
	let startRange = params.rangeStart.split('-'); // server returns ISO formatted time, we are only interested in date
	let year = parseInt(startRange[0], 10);
	let month = parseInt(startRange[1], 10);

	yearSelector.value = year;
	monthSelector.value = month;


	function GetDaysInMonth(date)
	{
		return new Date(date.getFullYear(), date.getMonth() + 1, 0).getDate();
	}

	function EvaluateDate()
	{
		console.log(monthSelector.value, yearSelector.value);

		let date = new Date(yearSelector.value, monthSelector.value - 1);
		let year = date.getFullYear();
		let month = date.getMonth() + 1;
		let daysInMonth = GetDaysInMonth(date);
		
		let rangeStart = year + "-" + month + "-" + 1;
		let rangeEnd = year + "-" + month + "-" + daysInMonth;

		let params = GetCurrentPageParams();
		params.rangeStart = rangeStart;
		params.rangeEnd = rangeEnd;

		let newURL = GetPageUrl(params);
		console.log(newURL);
		window.location = newURL;
	}

	monthSelector.addEventListener("change", function () {
		EvaluateDate();
	});

	yearSelector.addEventListener("change", function () {
		EvaluateDate();
	});

	buttonPrevMonth.addEventListener("click", function () {
		let month = Number(monthSelector.value) - 1;

		if (month == 0) {
			let prev_year = (Number(yearSelector.value) - 1);

			if (yearSelector.querySelector('[value="' + prev_year + '"]')) {
				yearSelector.value = prev_year;
				month = 12;
			}
			else
			{
				month = 1;
			}

		}
		

		monthSelector.value = month;
		EvaluateDate();
	});

	buttonNextMonth.addEventListener("click", function () {
		let month = Number(monthSelector.value) + 1;

		if (month == 13)
		{
			let next_year = (Number(yearSelector.value) + 1);

			if (yearSelector.querySelector('[value="' + next_year + '"]'))
			{
				yearSelector.value = next_year;
				month = 1;
			}
			else
			{
				month = 12;
			}
		}

		monthSelector.value = month;
		EvaluateDate();
	});
}

function GetCurrentPageParams() {
	let currentPageQuery = new URLSearchParams(window.location.search);
	let groupId = location.pathname.split('/').pop();
	let rangeStart = currentPageQuery.get('rangeStart');
	let rangeEnd = currentPageQuery.get('rangeEnd');

	return { groupId, rangeStart, rangeEnd };
}

function GetEditApiUrl(params)
{
	return `/api/DayEntry/GetEntries/${params.groupId}?` + new URLSearchParams({ rangeStart: params.rangeStart, rangeEnd: params.rangeEnd})
}

function GetPageUrl(params) {
	return `/TimeTable/${params.groupId}?` + new URLSearchParams({ rangeStart: params.rangeStart, rangeEnd: params.rangeEnd })
}

function RestoreFromServer(){

	let params = GetCurrentPageParams();

	fetch(GetEditApiUrl(params))
		.then(request => request.json())
		.then(response => {
			let dayEntries = response.dayEntries;
			for (let entry of dayEntries) {
				let studentId = entry.studentId;
				let day = entry.timestamp.split('T')[0]

				let query = `input.entry-cell[student="${studentId}"][date*="${day}"]`;
				let node = document.querySelector(query);

				node.value = entry.value;
			}
		});
}

SetupCellEvents();
SetupDatepicker();
RestoreFromServer();