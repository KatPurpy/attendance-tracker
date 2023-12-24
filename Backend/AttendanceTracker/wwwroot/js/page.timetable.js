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

function RestoreFromServer(){
	let currentPageQuery = new URLSearchParams(window.location.search);
	let groupId = location.pathname.split('/').pop();
	let rangeStart = currentPageQuery.get('rangeStart');
	let rangeEnd = currentPageQuery.get('rangeEnd');

	fetch(`/api/DayEntry/GetEntries/${groupId}?` + new URLSearchParams({ rangeStart, rangeEnd }))
		.then(request => request.json())
		.then(response => {
			let dayEntries = response.dayEntries;
			console.log(dayEntries);

			for (let entry of dayEntries) {
				console.log(entry);

				let studentId = entry.studentId;
				let day = entry.timestamp.split('T')[0]

				let query = `input.entry-cell[student="${studentId}"][date*="${day}"]`;
				let node = document.querySelector(query);

				node.value = entry.value;
			}
		});
}

SetupCellEvents();
RestoreFromServer();